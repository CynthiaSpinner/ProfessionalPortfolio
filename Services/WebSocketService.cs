using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Portfolio.Services
{
    public class WebSocketService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
        private readonly ILogger<WebSocketService> _logger;

        public WebSocketService(ILogger<WebSocketService> logger)
        {
            _logger = logger;
        }

        public void AddConnection(string connectionId, WebSocket webSocket)
        {
            _connections.TryAdd(connectionId, webSocket);
            _logger.LogInformation($"WebSocket connection added: {connectionId}. Total connections: {_connections.Count}");
        }

        public void RemoveConnection(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
            _logger.LogInformation($"WebSocket connection removed: {connectionId}. Total connections: {_connections.Count}");
        }

        public async Task BroadcastMessageAsync(string message)
        {
            if (_connections.IsEmpty)
            {
                _logger.LogInformation("No WebSocket connections to broadcast to");
                return;
            }

            var buffer = Encoding.UTF8.GetBytes(message);
            var deadConnections = new List<string>();

            foreach (var connection in _connections)
            {
                try
                {
                    if (connection.Value.State == WebSocketState.Open)
                    {
                        await connection.Value.SendAsync(
                            new ArraySegment<byte>(buffer),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                        
                        _logger.LogInformation($"Message broadcasted to connection: {connection.Key}");
                    }
                    else
                    {
                        deadConnections.Add(connection.Key);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error broadcasting message to connection: {connection.Key}");
                    deadConnections.Add(connection.Key);
                }
            }

            // Clean up dead connections
            foreach (var deadConnection in deadConnections)
            {
                RemoveConnection(deadConnection);
            }
        }

        public async Task BroadcastHeroDataUpdatedAsync()
        {
            var message = System.Text.Json.JsonSerializer.Serialize(new
            {
                type = "heroDataUpdated",
                timestamp = DateTime.UtcNow
            });

            await BroadcastMessageAsync(message);
        }

        public int GetConnectionCount()
        {
            return _connections.Count;
        }

        public async Task CloseAllConnections()
        {
            if (_connections.IsEmpty)
            {
                _logger.LogInformation("No WebSocket connections to close");
                return;
            }

            var closeTasks = new List<Task>();
            
            foreach (var connection in _connections)
            {
                try
                {
                    if (connection.Value.State == WebSocketState.Open)
                    {
                        closeTasks.Add(connection.Value.CloseAsync(
                            WebSocketCloseStatus.NormalClosure, 
                            "Application shutting down", 
                            CancellationToken.None));
                        
                        _logger.LogInformation($"Closing WebSocket connection: {connection.Key}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error closing WebSocket connection: {connection.Key}");
                }
            }

            // Wait for all connections to close (with timeout)
            if (closeTasks.Count > 0)
            {
                try
                {
                    await Task.WhenAll(closeTasks).WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (TimeoutException)
                {
                    _logger.LogWarning("Timeout waiting for WebSocket connections to close");
                }
            }

            _connections.Clear();
            _logger.LogInformation("All WebSocket connections closed");
        }
    }
} 