import React from "react";
import Header from "../Header";

const HeroSection = ({ heroData, showImageUpload = false, onImageUpload = null }) => {
  // Use provided data - handle undefined gracefully
  const data = heroData || {};

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
      showImageUpload={showImageUpload}
      onImageUpload={onImageUpload}
    />
  );
};

export default HeroSection; 