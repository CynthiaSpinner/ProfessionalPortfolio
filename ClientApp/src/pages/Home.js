import React from "react";
import Footer from "../components/Footer";
import "../styles/Home.css";
import { HeroSection, FeaturesSection, CTASection } from "../components/home";

const Home = () => {
  return (
    <div className="home-page">
      <HeroSection />
      <FeaturesSection />
      <CTASection />
      <Footer />
    </div>
  );
};

export default Home;
