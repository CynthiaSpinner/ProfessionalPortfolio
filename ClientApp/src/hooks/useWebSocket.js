import { useEffect, useRef, useCallback } from 'react';

export const useWebSocket = (messageType, onMessage) => {
  const wsRef = useRef(null);
  const reconnectAttemptsRef = useRef(0);
  const reconnectTimeoutRef = useRef(null);
  const fallbackPollingIntervalRef = useRef(null);
  const maxReconnectAttempts = 3;

  const getWebSocketUrl = () => {
    if (process.env.NODE_ENV === 'production') {
      return `wss://portfolio-app-1776-hkdfazazd5cqfzbk.centralus-01.azurewebsites.net/client/hubs/portfolio`;
    } else {
      return `wss://localhost:7094/client/hubs/portfolio`;
    }
  };

  const connectWebSocket = useCallback(() => {
    try {
      wsRef.current = new WebSocket(getWebSocketUrl());
      
      wsRef.current.onopen = () => {
        console.log(`WebSocket connected for ${messageType} updates`);
        reconnectAttemptsRef.current = 0;
        
        if (fallbackPollingIntervalRef.current) {
          clearInterval(fallbackPollingIntervalRef.current);
          fallbackPollingIntervalRef.current = null;
        }
      };
      
      wsRef.current.onmessage = (event) => {
        const data = JSON.parse(event.data);
        if (data.type === messageType) {
          console.log(`Real-time update received for ${messageType}, refreshing data...`);
          onMessage();
        }
      };
      
      wsRef.current.onerror = (error) => {
        console.error('WebSocket error:', error);
      };
      
      wsRef.current.onclose = (event) => {
        console.log('WebSocket connection closed');
        
        if (event.code !== 1000 && reconnectAttemptsRef.current < maxReconnectAttempts) {
          reconnectAttemptsRef.current++;
          console.log(`WebSocket reconnection attempt ${reconnectAttemptsRef.current}/${maxReconnectAttempts}`);
          
          reconnectTimeoutRef.current = setTimeout(() => {
            connectWebSocket();
          }, 2000 * reconnectAttemptsRef.current);
        } else if (reconnectAttemptsRef.current >= maxReconnectAttempts) {
          console.log('WebSocket reconnection failed, falling back to polling');
          
          fallbackPollingIntervalRef.current = setInterval(() => {
            console.log(`Fallback polling: checking for ${messageType} updates...`);
            onMessage();
          }, 5000);
        }
      };
    } catch (error) {
      console.error('Failed to create WebSocket connection:', error);
      
      fallbackPollingIntervalRef.current = setInterval(() => {
        console.log(`Fallback polling: checking for ${messageType} updates...`);
        onMessage();
      }, 5000);
    }
  }, [messageType, onMessage]);

  useEffect(() => {
    connectWebSocket();

    // Cleanup on component unmount
    return () => {
      if (wsRef.current) {
        wsRef.current.close();
      }
      if (reconnectTimeoutRef.current) {
        clearTimeout(reconnectTimeoutRef.current);
      }
      if (fallbackPollingIntervalRef.current) {
        clearInterval(fallbackPollingIntervalRef.current);
      }
    };
  }, [messageType, onMessage, connectWebSocket]);
}; 