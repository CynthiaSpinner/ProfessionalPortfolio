import React, { useCallback } from "react";
import Footer from "../components/Footer";
import "../styles/Home.css";
import { HeroSection, FeaturesSection, CTASection } from "../components/home";
import { PortfolioService } from "../services/PortfolioService";
import { usePortfolioData, useWebSocket } from "../hooks";

const Home = () => {
  const fetchHomepageData = useCallback(async () => {
    const data = await PortfolioService.getHomepageData();
    return data;
  }, []);

  const { data: homepageData, loading, refetch } = usePortfolioData(fetchHomepageData, []);

  // Set up WebSocket for real-time updates
  useWebSocket('homepage', refetch);

  if (loading) {
    return null; // Let the main app loading spinner handle this
  }

  return (
    <div className="home-page">
      <HeroSection 
        heroData={homepageData?.hero} 
        showImageUpload={true}
        onImageUpload={(imageUrl) => {
          console.log('New background image uploaded:', imageUrl);
          // Optionally refresh the page or update state
          window.location.reload();
        }}
      />
      <FeaturesSection featuresData={homepageData?.features} />
      <CTASection />
      <Footer />
    </div>
  );
};

export default Home;
