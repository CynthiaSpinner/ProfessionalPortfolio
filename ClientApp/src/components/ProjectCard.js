import React from "react";
import { Card, Row, Col } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import Button from "./Button";

const ProjectCard = ({ project }) => {
  const features = project.features ?? [];
  const technologies = project.technologies ?? [];
  const hasVideo = project.videoUrl && project.videoUrl.trim() !== "";
  const imageUrl = project.imageUrl?.trim() || null;

  return (
    <Card className="project-card">
      <Row className="align-items-center">
        <Col lg={6}>
          <div className="video-container">
            {hasVideo ? (
              <iframe
                src={project.videoUrl}
                title={project.title}
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowFullScreen
                className="project-video"
              />
            ) : imageUrl ? (
              <img src={imageUrl} alt={project.title} className="project-image img-fluid" />
            ) : (
              <div className="project-placeholder bg-light d-flex align-items-center justify-content-center">
                <span className="text-muted">No media</span>
              </div>
            )}
          </div>
        </Col>
        <Col lg={6}>
          <HeadingGroup title={project.title} subtitle={project.subtitle} />
          <div className="project-description">
            <p>{project.description || ""}</p>
            {features.length > 0 && (
              <ul>
                {features.map((feature, index) => (
                  <li key={index}>{feature}</li>
                ))}
              </ul>
            )}
            {technologies.length > 0 && (
              <div className="tech-stack">
                {technologies.map((tech, index) => (
                  <span key={index} className="tech-tag">
                    {tech}
                  </span>
                ))}
              </div>
            )}
            {(project.projectUrl || project.githubUrl) && (
              <div className="mt-3">
                {project.projectUrl && (
                  <Button href={project.projectUrl} className="me-2">
                    View Project
                  </Button>
                )}
                {project.githubUrl && (
                  <a href={project.githubUrl} target="_blank" rel="noopener noreferrer" className="btn btn-outline-primary custom-button ms-2">
                    GitHub
                  </a>
                )}
              </div>
            )}
          </div>
        </Col>
      </Row>
    </Card>
  );
};

export default ProjectCard;
