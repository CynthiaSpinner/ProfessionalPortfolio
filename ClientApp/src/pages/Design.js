import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import Button from "../components/Button";
import Card from "../components/Card";
import HeadingGroup from "../components/HeadingGroup";
import "./Design.css";

const Design = () => {
  return (
    <div className="design-page">
      <Header
        title="Design Projects"
        subtitle="Explore my creative design work and visual projects."
      />

      {/* Database Schema Design Section */}
      <section className="project-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Database Schema Design"
                subtitle="Detailed database schema designs showcasing efficient data organization and relationships"
              />
              <div className="image-container">
                {/* Placeholder for image */}
                <div className="image-placeholder">
                  Image will be displayed here
                </div>
              </div>
              <a href="/portfolio/database-design" className="btn btn-primary">
                View Project
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Software Design Section */}
      <section className="project-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Software Design"
                subtitle="Software architecture and system design projects"
              />
              <div className="image-container">
                {/* Placeholder for image */}
                <div className="image-placeholder">
                  Image will be displayed here
                </div>
              </div>
              <a href="/portfolio/software-design" className="btn btn-primary">
                View Project
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Web Design Section */}
      <section className="project-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Web Design"
                subtitle="Modern and responsive web design projects"
              />
              <div className="image-container">
                {/* Placeholder for image */}
                <div className="image-placeholder">
                  Image will be displayed here
                </div>
              </div>
              <a href="/portfolio/web-design" className="btn btn-primary">
                View Project
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Sitemapping Section */}
      <section className="project-section">
        <Container>
          <Row>
            <Col>
              <h2>Sitemapping</h2>
              <div className="image-container">
                {/* Placeholder for image */}
                <div className="image-placeholder">
                  Image will be displayed here
                </div>
              </div>
              <p>Information architecture and site structure planning</p>
              <a href="/portfolio/sitemapping" className="btn btn-primary">
                View Project
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      <Footer />
    </div>
  );
};

export default Design;
