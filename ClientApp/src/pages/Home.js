import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import Footer from "../components/Footer";
import Card from "../components/Card";
import Header from "../components/Header";
import "../styles/Home.css";
import CTA from "../components/CTA";
import { PortfolioService } from "../services/PortfolioService";

const Home = () => {
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

    // Set up WebSocket connection for real-time updates (NO POLLING!)
    const getWebSocketUrl = () => {
      if (process.env.NODE_ENV === 'production') {
        // Prefer explicit WS URL (e.g. Netlify env); otherwise use Render backend
        if (process.env.REACT_APP_WS_URL) {
          return process.env.REACT_APP_WS_URL;
        }
        return `wss://professionalportfolio-9a6n.onrender.com/ws/portfolio`;
      }
      // In development, connect to the ASP.NET Core backend directly
      return `wss://localhost:7094/ws/portfolio`;
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
          reconnectAttempts = 0; // Reset reconnect attempts on successful connection
          
          // Clear any fallback polling if WebSocket is working
          if (fallbackPollingInterval) {
            clearInterval(fallbackPollingInterval);
            fallbackPollingInterval = null;
          }
        };
        
        ws.onmessage = (event) => {
          const data = JSON.parse(event.data);
          if (data.type === 'heroDataUpdated') {
            console.log('Real-time update received, refreshing data...');
            fetchHeroData();
          }
        };
        
        ws.onerror = (error) => {
          console.error('WebSocket error:', error);
        };
        
        ws.onclose = (event) => {
          console.log('WebSocket connection closed');
          
          // Try to reconnect if not a normal closure
          if (event.code !== 1000 && reconnectAttempts < maxReconnectAttempts) {
            reconnectAttempts++;
            console.log(`WebSocket reconnection attempt ${reconnectAttempts}/${maxReconnectAttempts}`);
            
            reconnectTimeout = setTimeout(() => {
              connectWebSocket();
            }, 2000 * reconnectAttempts); // Exponential backoff
          } else if (reconnectAttempts >= maxReconnectAttempts) {
            console.log('WebSocket reconnection failed, falling back to polling');
            
            // Fallback to polling if WebSocket fails
            fallbackPollingInterval = setInterval(() => {
              console.log('Fallback polling: checking for updates...');
              fetchHeroData();
            }, 5000); // Poll every 5 seconds as fallback
          }
        };
      } catch (error) {
        console.error('Failed to create WebSocket connection:', error);
        
        // Fallback to polling if WebSocket creation fails
        fallbackPollingInterval = setInterval(() => {
          console.log('Fallback polling: checking for updates...');
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
  }, [heroData.lastModified]); // Add heroData.lastModified as dependency

  if (loading) {
    return (
      <div className="home-page">
        <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "100vh" }}>
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="home-page">
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

      {/* Features List Section */}
      <section className="features-section py-5">
        <Container>
          <HeadingGroup
            title="Key Skills & Technologies"
            className="text-center mb-5"
          />
          <Row className="g-4">
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Frontend Development"
                  subtitle="React, JavaScript, HTML5, CSS3, Bootstrap"
                />
              </Card>
            </Col>
 
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Backend Development"
                  subtitle=".NET Core, C#, RESTful APIs, MySQL"
                />
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Design & Tools"
                  subtitle="Adobe Creative Suite, UI/UX Design, Git, Docker"
                />
              </Card>
            </Col>
          </Row>
        </Container>
      </section>

      {/* CTA Section */}
      <CTA
        title="Ready to Start a Project?"
        subtitle="Let's work together to bring your ideas to life."
      />

      
      <Footer />
    </div>
  );
};

export default Home;
