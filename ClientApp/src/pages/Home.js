import React, { useState, useEffect, useCallback } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import Footer from "../components/Footer";
import Card from "../components/Card";
import Header from "../components/Header";
import "../styles/Home.css";
import CTA from "../components/CTA";
import { PortfolioService } from "../services/PortfolioService";

const defaultFeatures = {
  sectionTitle: "Key Skills & Technologies",
  sectionSubtitle: "Explore my expertise across different domains",
  features: [
    { title: "Frontend Development", subtitle: "React, JavaScript, HTML5, CSS3, Bootstrap", description: "", icon: "fas fa-code", link: "/projects?category=frontend" },
    { title: "Backend Development", subtitle: ".NET Core, C#, RESTful APIs, MySQL", description: "", icon: "fas fa-server", link: "/projects?category=backend" },
    { title: "Design & Tools", subtitle: "Adobe Creative Suite, UI/UX Design, Git, Docker", description: "", icon: "fas fa-palette", link: "/projects?category=design" }
  ],
  lastModified: null
};

const defaultCTA = {
  title: "Ready to Start a Project?",
  subtitle: "Let's work together to bring your ideas to life.",
  buttonText: "Get in Touch",
  buttonLink: "/contact",
  lastModified: null
};

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
  const [featuresData, setFeaturesData] = useState(defaultFeatures);
  const [ctaData, setCtaData] = useState(defaultCTA);
  const [loading, setLoading] = useState(true);

  const fetchHeroData = useCallback(async () => {
    try {
      const data = await PortfolioService.getHeroSection();
      setHeroData(prev => (data.lastModified !== prev.lastModified || JSON.stringify(prev) !== JSON.stringify(data) ? data : prev));
    } catch (error) {
      console.error("Error fetching hero data:", error);
    }
  }, []);

  const fetchFeaturesData = useCallback(async () => {
    try {
      const data = await PortfolioService.getFeatures();
      setFeaturesData(prev => (data.lastModified !== prev.lastModified || JSON.stringify(prev) !== JSON.stringify(data) ? data : prev));
    } catch (error) {
      console.error("Error fetching features data:", error);
    }
  }, []);

  const fetchCTAData = useCallback(async () => {
    try {
      const data = await PortfolioService.getCTA();
      setCtaData(prev => (data.lastModified !== prev.lastModified || JSON.stringify(prev) !== JSON.stringify(data) ? data : prev));
    } catch (error) {
      console.error("Error fetching CTA data:", error);
    }
  }, []);

  useEffect(() => {
    let cancelled = false;

    const loadAll = async () => {
      try {
        await Promise.all([fetchHeroData(), fetchFeaturesData(), fetchCTAData()]);
      } catch (e) {
        console.error("Error loading home data:", e);
      } finally {
        if (!cancelled) setLoading(false);
      }
    };
    loadAll();

    const getWebSocketUrl = () => {
      if (process.env.NODE_ENV === 'production') {
        if (process.env.REACT_APP_WS_URL) return process.env.REACT_APP_WS_URL;
        return `wss://professionalportfolio-9a6n.onrender.com/ws/portfolio`;
      }
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
          reconnectAttempts = 0;
          if (fallbackPollingInterval) {
            clearInterval(fallbackPollingInterval);
            fallbackPollingInterval = null;
          }
        };

        ws.onmessage = (event) => {
          try {
            const data = JSON.parse(event.data);
            if (data.type === 'heroDataUpdated') {
              fetchHeroData();
            } else if (data.type === 'featuresDataUpdated') {
              fetchFeaturesData();
            } else if (data.type === 'ctaDataUpdated') {
              fetchCTAData();
            }
          } catch (e) {
            console.error('WebSocket message parse error:', e);
          }
        };

        ws.onerror = (error) => console.error('WebSocket error:', error);

        ws.onclose = (event) => {
          if (event.code !== 1000 && reconnectAttempts < maxReconnectAttempts) {
            reconnectAttempts++;
            reconnectTimeout = setTimeout(connectWebSocket, 2000 * reconnectAttempts);
          } else if (reconnectAttempts >= maxReconnectAttempts) {
            fallbackPollingInterval = setInterval(() => {
              fetchHeroData();
              fetchFeaturesData();
              fetchCTAData();
            }, 5000);
          }
        };
      } catch (error) {
        console.error('Failed to create WebSocket connection:', error);
        fallbackPollingInterval = setInterval(() => {
          fetchHeroData();
          fetchFeaturesData();
          fetchCTAData();
        }, 5000);
      }
    };

    connectWebSocket();

    return () => {
      cancelled = true;
      if (ws) ws.close();
      if (reconnectTimeout) clearTimeout(reconnectTimeout);
      if (fallbackPollingInterval) clearInterval(fallbackPollingInterval);
    };
  }, [fetchHeroData, fetchFeaturesData, fetchCTAData]);

  if (loading) {
    return (
      <div className="home-page" style={{ minHeight: "100vh" }}>
        <div style={{ position: "fixed", inset: 0, background: "#050510", display: "flex", flexDirection: "column", alignItems: "center", justifyContent: "center", gap: "1.5rem", zIndex: 9999 }}>
          <div
            role="status"
            aria-label="Loading"
            style={{
              width: 56,
              height: 56,
              border: "3px solid rgba(129, 140, 248, 0.25)",
              borderTopColor: "#818cf8",
              borderRadius: "50%",
              animation: "spin 0.9s linear infinite",
            }}
          />
          <p style={{ color: "#a5b4fc", fontSize: "1.1rem", margin: 0, fontWeight: 500 }}>Creating your experience…</p>
        </div>
      </div>
    );
  }

  const { sectionTitle, sectionSubtitle, features } = featuresData;

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

      <section className="features-section py-5">
        <Container>
          <HeadingGroup
            title={sectionTitle}
            subtitle={sectionSubtitle}
            className="text-center mb-5"
          />
          <Row className="g-4">
            {(features || []).slice(0, 3).map((f, i) => (
              <Col key={i} md={4}>
                <Card>
                  {f.icon && <div className="mb-2"><i className={f.icon} aria-hidden="true" /></div>}
                  <HeadingGroup
                    title={f.title}
                    subtitle={f.subtitle}
                  />
                  {f.description && <p className="mb-0 text-muted small">{f.description}</p>}
                  {f.link && (
                    <a href={f.link} className="stretched-link mt-2 d-inline-block small">Learn more</a>
                  )}
                </Card>
              </Col>
            ))}
          </Row>
        </Container>
      </section>

      <CTA
        title={ctaData.title}
        subtitle={ctaData.subtitle}
        buttonText={ctaData.buttonText}
        buttonHref={ctaData.buttonLink}
      />

      <Footer />
    </div>
  );
};

export default Home;
