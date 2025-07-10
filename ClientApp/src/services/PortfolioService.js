import axios from "axios";

// In production, this will point to your Azure App Service API
const API_BASE_URL = process.env.NODE_ENV === 'production' 
  ? "https://portfolio-app-1776.azurewebsites.net/api/portfolio"
  : "/api/portfolio";

export const PortfolioService = {
  async getProjects() {
    const response = await axios.get(`${API_BASE_URL}/projects`);
    return response.data;
  },

  async getSkills() {
    const response = await axios.get(`${API_BASE_URL}/skills`);
    return response.data;
  },

  async getAbout() {
    const response = await axios.get(`${API_BASE_URL}/about`);
    return response.data;
  },

  async getHeroSection() {
    const response = await axios.get(`${API_BASE_URL}/hero`);
    return response.data;
  },
};
