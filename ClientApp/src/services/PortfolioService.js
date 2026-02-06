import api from "./api";

// Mock data for development (used only when API not available or for projects/skills/about if not yet wired)
const mockData = {
  hero: {
    title: "Welcome to My Portfolio",
    subtitle: "I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications.",
    description: "Building innovative solutions with cutting-edge technologies.",
    backgroundImageUrl: "",
    backgroundVideoUrl: "",
    primaryButtonText: "View Projects",
    primaryButtonUrl: "/projects",
    overlayColor: "#000000",
    overlayOpacity: 0.5,
    lastModified: new Date().toISOString()
  },
  projects: [
    {
      id: 1,
      title: "Portfolio Website",
      description: "A personal portfolio website built with React and .NET",
      imageUrl: "https://via.placeholder.com/300",
      technologies: ["React", ".NET", "MySQL"],
    },
  ],
  skills: {
    technical: ["React", ".NET", "MySQL", "JavaScript"],
    soft: ["Communication", "Problem Solving", "Teamwork"],
  },
  about: {
    title: "About Me",
    description: "I am a passionate developer with experience in web development.",
    imageUrl: "https://via.placeholder.com/300",
  }
};

export const PortfolioService = {
  async getProjects() {
    return mockData.projects;
  },

  async getSkills() {
    return mockData.skills;
  },

  async getAbout() {
    return mockData.about;
  },

  async getHeroSection() {
    try {
      const { data } = await api.get("/portfolio/hero");
      return data;
    } catch (err) {
      console.error("Error fetching hero:", err);
      return mockData.hero;
    }
  },

  async getFeatures() {
    try {
      const { data } = await api.get("/portfolio/features");
      return data;
    } catch (err) {
      console.error("Error fetching features:", err);
      return {
        sectionTitle: "Key Skills & Technologies",
        sectionSubtitle: "Explore my expertise across different domains",
        features: [
          { title: "Frontend Development", subtitle: "React, JavaScript, HTML5, CSS3, Bootstrap", description: "", icon: "fas fa-code", link: "/projects?category=frontend" },
          { title: "Backend Development", subtitle: ".NET Core, C#, RESTful APIs, MySQL", description: "", icon: "fas fa-server", link: "/projects?category=backend" },
          { title: "Design & Tools", subtitle: "Adobe Creative Suite, UI/UX Design, Git, Docker", description: "", icon: "fas fa-palette", link: "/projects?category=design" }
        ],
        lastModified: new Date().toISOString()
      };
    }
  },

  async getCTA() {
    try {
      const { data } = await api.get("/portfolio/cta");
      return data;
    } catch (err) {
      console.error("Error fetching CTA:", err);
      return {
        title: "Ready to Start a Project?",
        subtitle: "Let's work together to bring your ideas to life.",
        buttonText: "Get in Touch",
        buttonLink: "/contact",
        lastModified: new Date().toISOString()
      };
    }
  },
};
