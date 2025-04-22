import React from "react";
import { Container } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import ProjectCard from "../components/ProjectCard";
import Button from "../components/Button";
import "./Projects.css";

// Sample database content - replace with your actual data source
const projects = [
  {
    id: 1,
    title: "Ecommerce Platform",
    subtitle: "A full-featured online shopping experience",
    description:
      "A modern ecommerce platform built with React and .NET Core, featuring:",
    features: [
      "User authentication and authorization",
      "Product catalog with search and filtering",
      "Shopping cart and checkout process",
      "Payment integration",
      "Admin dashboard",
    ],
    technologies: ["React", ".NET Core", "SQL Server", "Stripe"],
    videoUrl: "https://sora.chatgpt.com/g/gen_01jn1tws19fp28j0ardpd8cxn5",
    projectUrl: "#",
  },
  {
    id: 2,
    title: "RESTful API Service",
    subtitle: "Scalable backend architecture",
    description: "A robust API service with advanced features:",
    features: [
      "RESTful endpoints with versioning",
      "Authentication and rate limiting",
      "Data validation and error handling",
      "Documentation with Swagger",
      "Performance monitoring",
    ],
    technologies: [".NET Core", "Entity Framework", "JWT", "Redis"],
    videoUrl: "https://sora.chatgpt.com/g/gen_01jn1tws19fp28j0ardpd8cxn5",
    projectUrl: "#",
  },
  {
    id: 3,
    title: "Database Management System",
    subtitle: "Efficient data handling and optimization",
    description: "A comprehensive database solution featuring:",
    features: [
      "Advanced query optimization",
      "Data warehousing",
      "ETL processes",
      "Backup and recovery",
      "Performance monitoring",
    ],
    technologies: ["SQL Server", "SSIS", "Power BI", "Azure"],
    videoUrl: "https://sora.chatgpt.com/g/gen_01jn1tws19fp28j0ardpd8cxn5",
    projectUrl: "#",
  },
  {
    id: 4,
    title: "Interactive Game",
    subtitle: "Engaging user experience",
    description: "An immersive game with modern features:",
    features: [
      "Real-time multiplayer",
      "Physics-based gameplay",
      "Custom game engine",
      "Leaderboard system",
      "Cross-platform support",
    ],
    technologies: ["Unity", "C#", "WebGL", "Photon"],
    videoUrl: "https://sora.chatgpt.com/g/gen_01jn1tws19fp28j0ardpd8cxn5",
    projectUrl: "#",
  },
  {
    id: 5,
    title: "Content Management System",
    subtitle: "Flexible content management",
    description: "A powerful CMS with extensive features:",
    features: [
      "Custom content types",
      "Role-based access control",
      "Media library",
      "SEO optimization",
      "Multi-site support",
    ],
    technologies: ["React", "Node.js", "MongoDB", "GraphQL"],
    videoUrl: "https://sora.chatgpt.com/g/gen_01jn1tws19fp28j0ardpd8cxn5",
    projectUrl: "#",
  },
];

const Projects = () => {
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

      {projects.map((project) => (
        <section key={project.id} className="project-item py-5">
          <Container>
            <ProjectCard project={project} />
          </Container>
        </section>
      ))}

      <Footer />
    </div>
  );
};

export default Projects;
