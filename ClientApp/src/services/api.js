import axios from "axios";

// For development, use mock data
const USE_MOCK_DATA = false;

const API_URL = process.env.REACT_APP_API_URL || "https://portfolio-app-1776-hkdfazazd5cqfzbk.centralus-01.azurewebsites.net/api";

const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

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
  about: {
    title: "About Me",
    description:
      "I am a passionate developer with experience in web development.",
    imageUrl: "https://via.placeholder.com/300",
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
};

// Portfolio API endpoints
export const portfolioApi = {
  // Hero section endpoints
  getHeroSection: () =>
    USE_MOCK_DATA
      ? Promise.resolve({ data: mockData.hero })
      : api.get("/portfolio/hero"),
  updateHeroSection: (data) => api.put("/portfolio/hero", data),

  // About endpoints
  getAbout: () =>
    USE_MOCK_DATA
      ? Promise.resolve({ data: mockData.about })
      : api.get("/portfolio/about"),
  updateAbout: (data) => api.put("/portfolio/about", data),

  // Projects endpoints
  getProjects: () =>
    USE_MOCK_DATA
      ? Promise.resolve({ data: mockData.projects })
      : api.get("/portfolio/projects"),
  getProject: (id) => api.get(`/portfolio/projects/${id}`),
  createProject: (data) => api.post("/portfolio/projects", data),
  updateProject: (id, data) => api.put(`/portfolio/projects/${id}`, data),
  deleteProject: (id) => api.delete(`/portfolio/projects/${id}`),

  // Skills endpoints
  getSkills: () =>
    USE_MOCK_DATA
      ? Promise.resolve({ data: mockData.skills })
      : api.get("/portfolio/skills"),
  updateSkills: (data) => api.put("/portfolio/skills", data),
};
