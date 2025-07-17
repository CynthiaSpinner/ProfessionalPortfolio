import { portfolioApi } from './api';

export const PortfolioService = {
  async getProjects() {
    const response = await portfolioApi.getProjects();
    return response.data;
  },

  async getSkills() {
    const response = await portfolioApi.getSkills();
    return response.data;
  },

  async getAbout() {
    const response = await portfolioApi.getAbout();
    return response.data;
  },

  async getHeroSection() {
    const response = await portfolioApi.getHeroSection();
    return response.data;
  },

  async getFeatures() {
    const response = await portfolioApi.getFeatures();
    return response.data;
  },

  async getHomepageData() {
    const response = await portfolioApi.get('/api/portfolio/homepage');
    return response.data;
  },
};
