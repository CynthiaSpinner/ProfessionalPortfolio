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
      const [aboutData, projectsData, skillsData] = await Promise.all([
        portfolioApi.getAbout(),
        portfolioApi.getProjects(),
        portfolioApi.getSkills(),
      ]);

      setAbout(aboutData.data);
      setProjects(projectsData.data);
      setSkills(skillsData.data);
      setError(null);
    } catch (err) {
      setError("Failed to fetch portfolio data");
      console.error("Error fetching portfolio data:", err);
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
