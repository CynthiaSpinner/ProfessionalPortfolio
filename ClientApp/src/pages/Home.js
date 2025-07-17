import React, { useCallback } from "react";
import Footer from "../components/Footer";
import "../styles/Home.css";
import { HeroSection, FeaturesSection, CTASection } from "../components/home";
import { PortfolioService } from "../services/PortfolioService";
import { usePortfolioData } from "../hooks";

const Home = () => {
  const fetchHomepageData = useCallback(async () => {
    const data = await PortfolioService.getHomepageData();
    return data;
  }, []);

  const fallbackData = {
    hero: {
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
          subtitle: ".NET Core, C#, RESTful APIs, SQL Server",
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
    },
    projects: [],
    skills: [],
    about: null
  };

  const { data: homepageData, loading } = usePortfolioData(fetchHomepageData, [], fallbackData);

  if (loading) {
    return (
      <div className="home-page">
        <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "100vh" }}>
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading homepage...</span>
          </div>
        </div>
      </div>
    );
  }

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
