import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import Footer from "../components/Footer";
import Button from "../components/Button";
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
  const [refreshing, setRefreshing] = useState(false);
  const [lastCheckTime, setLastCheckTime] = useState(null);

  useEffect(() => {
    const fetchHeroData = async () => {
      try {
        if (!loading) {
          setRefreshing(true);
        }
        
        const data = await PortfolioService.getHeroSection();
        
        // Only update if data has actually changed
        if (data.lastModified !== heroData.lastModified) {
          setHeroData(data);
          console.log("Hero data updated:", new Date(data.lastModified));
        } else {
          console.log("No changes detected, skipping update");
        }
        
        setLastCheckTime(new Date());
      } catch (error) {
        console.error("Error fetching hero data:", error);
        // Keep default values if fetch fails
      } finally {
        setLoading(false);
        setRefreshing(false);
      }
    };

    // Initial fetch
    fetchHeroData();

    // Set up auto-refresh every 3 seconds
    const intervalId = setInterval(fetchHeroData, 3000);

    // Cleanup interval on component unmount
    return () => clearInterval(intervalId);
  }, [heroData.lastModified]);

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
        backgroundImageUrl={heroData.backgroundImageUrl}
        backgroundVideoUrl={heroData.backgroundVideoUrl}
        overlayColor={heroData.overlayColor}
        overlayOpacity={heroData.overlayOpacity}
        primaryButtonText={heroData.primaryButtonText}
        primaryButtonUrl={heroData.primaryButtonUrl}
        showButtons={true}
        refreshing={refreshing}
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
