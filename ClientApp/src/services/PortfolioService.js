// Mock data for development
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
    return mockData.hero;
  },
};
