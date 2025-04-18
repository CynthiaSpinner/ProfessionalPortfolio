import axios from "axios";

const API_BASE_URL = "/api/portfolio";

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
};
