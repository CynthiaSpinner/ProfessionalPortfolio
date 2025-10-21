import React, { createContext, useState, useContext, useEffect } from "react";
import { portfolioApi } from "../services/api";

const PortfolioContext = createContext();

export const usePortfolio = () => useContext(PortfolioContext);

export const PortfolioProvider = ({ children }) => {
  const [about, setAbout] = useState(null);
  const [projects, setProjects] = useState([]);
  const [skills, setSkills] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchPortfolioData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Add a small delay to show loading spinner (for testing)
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      // Add timeout to prevent infinite loading
      const timeoutPromise = new Promise((_, reject) => {
        setTimeout(() => reject(new Error('Request timeout')), 10000); // 10 second timeout
      });

      // Load each endpoint individually to handle failures gracefully
      try {
        const aboutData = await Promise.race([
        portfolioApi.getAbout(),
          timeoutPromise
        ]);
        setAbout(aboutData.data);
      } catch (err) {
        console.warn("Failed to load about data:", err.message);
        setAbout(null);
      }

      try {
        const projectsData = await Promise.race([
        portfolioApi.getProjects(),
          timeoutPromise
        ]);
        setProjects(projectsData.data);
      } catch (err) {
        console.warn("Failed to load projects data:", err.message);
        setProjects([]);
      }

      try {
        const skillsData = await Promise.race([
        portfolioApi.getSkills(),
          timeoutPromise
      ]);
      setSkills(skillsData.data);
      } catch (err) {
        console.warn("Failed to load skills data:", err.message);
        setSkills(null);
      }

    } catch (err) {
      console.error("Unexpected error in fetchPortfolioData:", err);
      setError("Failed to fetch portfolio data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPortfolioData();
  }, []);

  const value = {
    about,
    projects,
    skills,
    loading,
    error,
    updateAbout: async (data) => {
      try {
        const response = await portfolioApi.updateAbout(data);
        setAbout(response.data);
        return response.data;
      } catch (err) {
        setError("Failed to update about information");
        throw err;
      }
    },
    addProject: async (data) => {
      try {
        const response = await portfolioApi.createProject(data);
        setProjects([...projects, response.data]);
        return response.data;
      } catch (err) {
        setError("Failed to add project");
        throw err;
      }
    },
    updateProject: async (id, data) => {
      try {
        const response = await portfolioApi.updateProject(id, data);
        setProjects(
          projects.map((project) =>
            project.id === id ? response.data : project
          )
        );
        return response.data;
      } catch (err) {
        setError("Failed to update project");
        throw err;
      }
    },
    deleteProject: async (id) => {
      try {
        await portfolioApi.deleteProject(id);
        setProjects(projects.filter((project) => project.id !== id));
      } catch (err) {
        setError("Failed to delete project");
        throw err;
      }
    },
    updateSkills: async (data) => {
      try {
        const response = await portfolioApi.updateSkills(data);
        setSkills(response.data);
        return response.data;
      } catch (err) {
        setError("Failed to update skills");
        throw err;
      }
    },
  };

  return (
    <PortfolioContext.Provider value={value}>
      {children}
    </PortfolioContext.Provider>
  );
};
