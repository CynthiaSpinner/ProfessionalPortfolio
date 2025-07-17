import React, { useCallback } from "react";
import Header from "../Header";
import { PortfolioService } from "../../services/PortfolioService";
import { useWebSocket, usePortfolioData } from "../../hooks";

const HeroSection = () => {
  const defaultHeroData = {
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
  };

  const fetchHeroData = useCallback(async () => {
    const data = await PortfolioService.getHeroSection();
    return data;
  }, []);

  const { data: heroData = defaultHeroData, loading, refetch: refetchHero } = usePortfolioData(fetchHeroData);

  // Use shared WebSocket hook
  useWebSocket('heroDataUpdated', fetchHeroData);

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