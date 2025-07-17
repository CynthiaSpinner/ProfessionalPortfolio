import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../HeadingGroup";
import Card from "../Card";
import { PortfolioService } from "../../services/PortfolioService";

const FeaturesSection = () => {
  const [featuresData, setFeaturesData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchFeaturesData = async () => {
      try {
        const data = await PortfolioService.getFeatures();
        setFeaturesData(data);
      } catch (error) {
        console.error("Error fetching features data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchFeaturesData();

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
          console.log('WebSocket connected for features updates');
          reconnectAttempts = 0;
          
          if (fallbackPollingInterval) {
            clearInterval(fallbackPollingInterval);
            fallbackPollingInterval = null;
          }
        };
        
        ws.onmessage = (event) => {
          const data = JSON.parse(event.data);
          if (data.type === 'featuresDataUpdated') {
            console.log('Real-time update received, refreshing features data...');
            fetchFeaturesData();
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
              console.log('Fallback polling: checking for features updates...');
              fetchFeaturesData();
            }, 5000);
          }
        };
      } catch (error) {
        console.error('Failed to create WebSocket connection:', error);
        
        fallbackPollingInterval = setInterval(() => {
          console.log('Fallback polling: checking for features updates...');
          fetchFeaturesData();
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
  }, []);

  if (loading) {
    return (
      <section className="features-section py-5">
        <Container>
          <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "200px" }}>
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Loading features section...</span>
            </div>
          </div>
        </Container>
      </section>
    );
  }

  return (
    <section className="features-section py-5">
      <Container>
        <HeadingGroup
          title={featuresData?.title || "Key Skills & Technologies"}
          subtitle={featuresData?.subtitle}
          className="text-center mb-5"
        />
        {featuresData?.description && (
          <p className="text-center mb-4 text-muted">{featuresData.description}</p>
        )}
        <Row className="g-4">
          {featuresData?.features?.map((feature, idx) => (
            <Col md={4} key={idx}>
              <Card>
                <div className="text-center mb-3">
                  {feature.icon && <i className={`${feature.icon} fa-2x mb-2`} style={{ color: "#818cf8" }}></i>}
                </div>
                <HeadingGroup
                  title={feature.title}
                  subtitle={feature.description}
                />
              </Card>
            </Col>
          ))}
        </Row>
      </Container>
    </section>
  );
};

export default FeaturesSection; 