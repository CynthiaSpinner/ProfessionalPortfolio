import React, { useState, useEffect } from "react";
import Header from "../Header";
import { PortfolioService } from "../../services/PortfolioService";

const HeroSection = () => {
  const [heroData, setHeroData] = useState({
    title: "Welcome to My Portfolio",
    subtitle: "I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications.",
    description: "",
    backgroundImageUrl: "",
    backgroundVideoUrl: "",
    primaryButtonText: "View Projects",
    primaryButtonUrl: "/projects",
    overlayColor: "#000000",
    overlayOpacity: 0.5,
    lastModified: null
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchHeroData = async () => {
      try {
        const data = await PortfolioService.getHeroSection();
        
        // Only update if data has actually changed to prevent unnecessary re-renders
        if (data.lastModified !== heroData.lastModified) {
          setHeroData(prevData => {
            // Deep comparison to avoid unnecessary updates
            if (JSON.stringify(prevData) !== JSON.stringify(data)) {
              console.log("Hero data updated:", new Date(data.lastModified));
              return data;
            }
            return prevData;
          });
        }
      } catch (error) {
        console.error("Error fetching hero data:", error);
        // Keep default values if fetch fails
      } finally {
        setLoading(false);
      }
    };

    // Initial fetch
    fetchHeroData();

    // Set up WebSocket connection for real-time updates
    const getWebSocketUrl = () => {
      if (process.env.NODE_ENV === 'production') {
        return `wss://portfolio-app-1776-hkdfazazd5cqfzbk.centralus-01.azurewebsites.net/client/hubs/portfolio`;
      } else {
        return `wss://localhost:7094/client/hubs/portfolio`;
      }
    };
    
    let ws = null;
    let reconnectAttempts = 0;
    const maxReconnectAttempts = 3;
    let reconnectTimeout = null;
    let fallbackPollingInterval = null;
    
    const connectWebSocket = () => {
      try {
        ws = new WebSocket(getWebSocketUrl());
        
        ws.onopen = () => {
          console.log('WebSocket connected for real-time updates');
          reconnectAttempts = 0;
          
          if (fallbackPollingInterval) {
            clearInterval(fallbackPollingInterval);
            fallbackPollingInterval = null;
          }
        };
        
        ws.onmessage = (event) => {
          const data = JSON.parse(event.data);
          if (data.type === 'heroDataUpdated') {
            console.log('Real-time update received, refreshing hero data...');
            fetchHeroData();
          }
        };
        
        ws.onerror = (error) => {
          console.error('WebSocket error:', error);
        };
        
        ws.onclose = (event) => {
          console.log('WebSocket connection closed');
          
          if (event.code !== 1000 && reconnectAttempts < maxReconnectAttempts) {
            reconnectAttempts++;
            console.log(`WebSocket reconnection attempt ${reconnectAttempts}/${maxReconnectAttempts}`);
            
            reconnectTimeout = setTimeout(() => {
              connectWebSocket();
            }, 2000 * reconnectAttempts);
          } else if (reconnectAttempts >= maxReconnectAttempts) {
            console.log('WebSocket reconnection failed, falling back to polling');
            
            fallbackPollingInterval = setInterval(() => {
              console.log('Fallback polling: checking for hero updates...');
              fetchHeroData();
            }, 5000);
          }
        };
      } catch (error) {
        console.error('Failed to create WebSocket connection:', error);
        
        fallbackPollingInterval = setInterval(() => {
          console.log('Fallback polling: checking for hero updates...');
          fetchHeroData();
        }, 5000);
      }
    };
    
    // Start WebSocket connection
    connectWebSocket();

    // Cleanup on component unmount
    return () => {
      if (ws) {
        ws.close();
      }
      if (reconnectTimeout) {
        clearTimeout(reconnectTimeout);
      }
      if (fallbackPollingInterval) {
        clearInterval(fallbackPollingInterval);
      }
    };
  }, [heroData.lastModified]);

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "50vh" }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading hero section...</span>
        </div>
      </div>
    );
  }

  return (
    <Header
      title={heroData.title}
      subtitle={heroData.subtitle}
      description={heroData.description}
      backgroundImageUrl={heroData.backgroundImageUrl}
      backgroundVideoUrl={heroData.backgroundVideoUrl}
      overlayColor={heroData.overlayColor}
      overlayOpacity={heroData.overlayOpacity}
      primaryButtonText={heroData.primaryButtonText}
      primaryButtonUrl={heroData.primaryButtonUrl}
      showButtons={true}
    />
  );
};

export default HeroSection; 