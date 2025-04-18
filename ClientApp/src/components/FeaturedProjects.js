import React from "react";
import {
  Container,
  Row,
  Col,
  Card,
  CardBody,
  CardTitle,
  CardText,
  Button,
} from "reactstrap";
import { Link } from "react-router-dom";
import "./FeaturedProjects.css";

function FeaturedProjects({ projects }) {
  const featuredProjects = projects?.slice(0, 3) || [];

  return (
    <section className="featured-projects py-5 bg-light">
      <Container>
        <h2 className="text-center mb-5">Featured Projects</h2>
        <Row>
          {featuredProjects.map((project) => (
            <Col md="4" key={project.id} className="mb-4">
              <Card className="h-100">
                {project.imageUrl && (
                  <img
                    src={project.imageUrl}
                    alt={project.title}
                    className="card-img-top"
                  />
                )}
                <CardBody>
                  <CardTitle tag="h5">{project.title}</CardTitle>
                  <CardText>{project.description}</CardText>
                  <div className="d-flex justify-content-between align-items-center">
                    <Button
                      color="primary"
                      tag={Link}
                      to={`/projects/${project.id}`}
                    >
                      View Details
                    </Button>
                    {project.githubUrl && (
                      <a
                        href={project.githubUrl}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="btn btn-outline-secondary"
                      >
                        GitHub
                      </a>
                    )}
                  </div>
                </CardBody>
              </Card>
            </Col>
          ))}
        </Row>
        <div className="text-center mt-4">
          <Button color="primary" tag={Link} to="/projects" size="lg">
            View All Projects
          </Button>
        </div>
      </Container>
    </section>
  );
}

export default FeaturedProjects;
