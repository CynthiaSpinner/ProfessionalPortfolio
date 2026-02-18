import React, { useState, useEffect, useRef } from "react";
import { Container } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import ProjectCard from "../components/ProjectCard";
import Button from "../components/Button";
import { portfolioApi } from "../services/api";
import "../styles/Projects.css";

const getWebSocketUrl = () => {
  if (process.env.NODE_ENV === "production") {
    if (process.env.REACT_APP_WS_URL) return process.env.REACT_APP_WS_URL;
    const apiUrl = process.env.REACT_APP_API_URL || "https://professionalportfolio-9a6n.onrender.com/api";
    return apiUrl.replace(/^https?:\/\//, "wss://").replace(/\/api\/?$/, "") + "/ws/portfolio";
  }
  return "wss://localhost:7094/ws/portfolio";
};

const Projects = () => {
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const wsRef = useRef(null);
  const reconnectTimeoutRef = useRef(null);

  const fetchProjects = async () => {
    try {
      const res = await portfolioApi.getProjects();
      setProjects(Array.isArray(res.data) ? res.data : []);
      setError(null);
    } catch (err) {
      console.error("Failed to load projects:", err);
      setError("Failed to load projects.");
      setProjects([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    let cancelled = false;
    fetchProjects().then(() => {
      if (cancelled) return;
      setLoading(false);
    });

    const connectWebSocket = () => {
      try {
        const ws = new WebSocket(getWebSocketUrl());
        wsRef.current = ws;

        ws.onopen = () => {
          console.log("Projects: WebSocket connected for real-time updates");
        };

        ws.onmessage = (event) => {
          try {
            const data = JSON.parse(event.data);
            if (data.type === "projectsDataUpdated") {
              fetchProjects();
            }
          } catch (e) {
            console.error("Projects: WebSocket message parse error:", e);
          }
        };

        ws.onerror = (err) => console.error("Projects: WebSocket error:", err);

        ws.onclose = (event) => {
          if (event.code !== 1000) {
            reconnectTimeoutRef.current = setTimeout(connectWebSocket, 3000);
          }
        };
      } catch (err) {
        console.error("Projects: WebSocket connect failed:", err);
        reconnectTimeoutRef.current = setTimeout(connectWebSocket, 3000);
      }
    };

    connectWebSocket();

    return () => {
      cancelled = true;
      if (wsRef.current) wsRef.current.close();
      if (reconnectTimeoutRef.current) clearTimeout(reconnectTimeoutRef.current);
    };
  }, []);

  if (loading) {
    return (
      <div className="projects-page">
        <Header title="My Projects" subtitle="Loading..." />
        <Container><p className="text-center py-5">Loading projects...</p></Container>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="projects-page">
        <Header title="My Projects" subtitle="" />
        <Container><p className="text-center py-5 text-danger">{error}</p></Container>
        <Footer />
      </div>
    );
  }

  return (
    <div className="projects-page">
      <Header
        title="My Projects"
        subtitle="Explore my portfolio of diverse projects showcasing my skills in full-stack development, database design, and creative problem-solving."
      >
        <Button href="/contact" className="mt-4">
          Let's Work Together
        </Button>
      </Header>

      {projects.length === 0 ? (
        <Container>
          <p className="text-center py-5 text-muted">No projects yet.</p>
        </Container>
      ) : (
        projects.map((project) => (
          <section key={project.id} className="project-item py-5">
            <Container>
              <ProjectCard project={project} />
            </Container>
          </section>
        ))
      )}

      <Footer />
    </div>
  );
};

export default Projects;
