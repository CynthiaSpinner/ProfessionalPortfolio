import React from "react";
import Header from "../Header";

const HeroSection = ({ heroData }) => {
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

  // Use provided data or fallback to default
  const data = heroData || defaultHeroData;

  return (
    <Header
      title={data.title}
      subtitle={data.subtitle}
      description={data.description}
      backgroundImageUrl={data.backgroundImageUrl}
      backgroundVideoUrl={data.backgroundVideoUrl}
      overlayColor={data.overlayColor}
      overlayOpacity={data.overlayOpacity}
      primaryButtonText={data.primaryButtonText}
      primaryButtonUrl={data.primaryButtonUrl}
      showButtons={true}
    />
  );
};

export default HeroSection; 