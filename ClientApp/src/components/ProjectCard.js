import React from "react";
import { Card, Row, Col } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import Button from "./Button";

const ProjectCard = ({ project }) => {
  return (
    <Card className="project-card">
      <Row className="align-items-center">
        <Col lg={6}>
          <div className="video-container">
            <iframe
              src={project.videoUrl}
              title={project.title}
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowFullScreen
              className="project-video"
            ></iframe>
          </div>
        </Col>
        <Col lg={6}>
          <HeadingGroup title={project.title} subtitle={project.subtitle} />
          <div className="project-description">
            <p>{project.description}</p>
            <ul>
              {project.features.map((feature, index) => (
                <li key={index}>{feature}</li>
              ))}
            </ul>
            <div className="tech-stack">
              {project.technologies.map((tech, index) => (
                <span key={index} className="tech-tag">
                  {tech}
                </span>
              ))}
            </div>
            <Button href={project.projectUrl} className="mt-3">
              View Project
            </Button>
          </div>
        </Col>
      </Row>
    </Card>
  );
};

export default ProjectCard;
