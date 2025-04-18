import React from "react";
import { Container, Row, Col, Button } from "reactstrap";
import { Link } from "react-router-dom";
import "./Hero.css";

function Hero({ about }) {
  return (
    <section className="hero-section py-5">
      <Container>
        <Row className="align-items-center">
          <Col md="6">
            <h1 className="display-4">
              {about?.title || "Welcome to My Portfolio"}
            </h1>
            <p className="lead">
              {about?.description ||
                "I'm a passionate developer creating innovative solutions and beautiful applications."}
            </p>
            <div className="mt-4">
              <Button
                color="primary"
                tag={Link}
                to="/projects"
                className="me-2"
              >
                View Projects
              </Button>
              <Button color="outline-primary" tag={Link} to="/about">
                Learn More
              </Button>
            </div>
          </Col>
          <Col md="6">
            <div className="hero-image">
              {about?.imageUrl && (
                <img
                  src={about.imageUrl}
                  alt="Hero"
                  className="img-fluid rounded"
                />
              )}
            </div>
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Hero;
