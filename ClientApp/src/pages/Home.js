import React, { useCallback } from "react";
import Footer from "../components/Footer";
import "../styles/Home.css";
import { HeroSection, FeaturesSection, CTASection } from "../components/home";
import { PortfolioService } from "../services/PortfolioService";
import { usePortfolioData, useWebSocket } from "../hooks";

const Home = () => {
  const fetchHomepageData = useCallback(async () => {
    try {
      const data = await PortfolioService.getHomepageData();
      return data;
    } catch (error) {
      console.warn("Failed to load homepage data, using fallback:", error);
      // Return fallback data structure if API fails
      return {
        hero: {
          title: "Crafting Digital Experiences That Work",
          subtitle: "Creating elegant applications with precision and patience",
          description: "Beautiful user interfaces and resilient backend systems with meticulous attention to detail - where beauty and strength intertwine, delivering seamless user experiences and scalable business solutions.",
          backgroundImageUrl: "",
          backgroundVideoUrl: "",
          primaryButtonText: "Explore My Creations",
          primaryButtonUrl: "/projects",
          overlayColor: "#000000",
          overlayOpacity: 0.5
        },
        features: {
          title: "Key Skills & Technologies",
          subtitle: "Explore my expertise across different domains",
          features: [
            {
              title: "Frontend Development",
              subtitle: "React, JavaScript, HTML5, CSS3, Bootstrap",
              description: "Building responsive and interactive user interfaces with modern frameworks and best practices.",
              icon: "fas fa-code",
              link: "/projects?category=frontend"
            },
            {
              title: "Backend Development", 
              subtitle: ".NET Core, C#, RESTful APIs, MySQL",
              description: "Creating robust server-side applications and APIs with enterprise-grade technologies.",
              icon: "fas fa-server",
              link: "/projects?category=backend"
            },
            {
              title: "Design & Tools",
              subtitle: "Adobe Creative Suite, UI/UX Design, Git, Docker", 
              description: "Crafting beautiful designs and managing development workflows with professional tools.",
              icon: "fas fa-palette",
              link: "/projects?category=design"
            }
          ]
        }
      };
    }
  }, []);

  const { data: homepageData, refetch } = usePortfolioData(fetchHomepageData, []);

  // Set up WebSocket for real-time updates
  useWebSocket('homepage', refetch);

  return (
    <div className="home-page">
      <HeroSection heroData={homepageData?.hero} />
      <FeaturesSection featuresData={homepageData?.features} />
      <CTASection />
      <Footer />
    </div>
  );
};

export default Home;
