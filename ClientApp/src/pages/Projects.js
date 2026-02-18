import React, { useState, useEffect, useRef } from "react";
import { Container } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import ProjectCard from "../components/ProjectCard";
import Button from "../components/Button";
import CTA from "../components/CTA";
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

const defaultHero = { title: "My Projects", subtitle: "", buttonText: "About me", buttonUrl: "/about" };
const defaultCTA = { title: "Ready to work together?", subtitle: "Let's build something.", buttonText: "Get in Touch", buttonLink: "/contact" };

const Projects = () => {
  const [projects, setProjects] = useState([]);
  const [hero, setHero] = useState(defaultHero);
  const [cta, setCta] = useState(defaultCTA);
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

    const load = async () => {
      try {
        const [projectsRes, heroRes, ctaRes] = await Promise.all([
          portfolioApi.getProjects(),
          portfolioApi.getProjectsPageHero().catch(() => ({ data: null })),
          portfolioApi.getProjectsPageCTA().catch(() => ({ data: null })),
        ]);
        if (cancelled) return;
        setProjects(Array.isArray(projectsRes.data) ? projectsRes.data : []);
        if (heroRes?.data) setHero({ ...defaultHero, ...heroRes.data });
        if (ctaRes?.data) setCta({ ...defaultCTA, ...ctaRes.data });
        setError(null);
      } catch (err) {
        if (!cancelled) {
          setError("Failed to load projects.");
          setProjects([]);
        }
      } finally {
        if (!cancelled) setLoading(false);
      }
    };
    load();

    const connectWebSocket = () => {
      try {
        const ws = new WebSocket(getWebSocketUrl());
        wsRef.current = ws;
        ws.onopen = () => {};
        ws.onmessage = (event) => {
          try {
            const data = JSON.parse(event.data);
            if (data.type === "projectsDataUpdated") fetchProjects();
          } catch (e) {}
        };
        ws.onerror = () => {};
        ws.onclose = (event) => {
          if (event.code !== 1000) reconnectTimeoutRef.current = setTimeout(connectWebSocket, 3000);
        };
      } catch (err) {
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
        <Header title="My Projects" subtitle="Loading..." className="page-header--texture" />
        <Container><p className="text-center py-5">Loading projects...</p></Container>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="projects-page">
        <Header title="My Projects" subtitle="" className="page-header--texture" />
        <Container><p className="text-center py-5 text-danger">{error}</p></Container>
        <Footer />
      </div>
    );
  }

  return (
    <div className="projects-page">
      <Header
        title={hero.title}
        subtitle={hero.subtitle || ""}
        className="page-header--texture"
      >
        {hero.buttonText && hero.buttonUrl && (
          <Button href={hero.buttonUrl} className="mt-4">
            {hero.buttonText}
          </Button>
        )}
      </Header>

      <section className="projects-section features-section py-5">
        <Container>
          {projects.length === 0 ? (
            <p className="text-center py-5 text-muted">No projects yet.</p>
          ) : (
            projects.map((project) => (
              <div key={project.id} className="project-item feature-card-wrapper py-4">
                <ProjectCard project={project} />
              </div>
            ))
          )}
        </Container>
      </section>

      <CTA
        title={cta.title}
        subtitle={cta.subtitle}
        buttonText={cta.buttonText}
        buttonHref={cta.buttonLink}
      />

      <Footer />
    </div>
  );
};

export default Projects;
