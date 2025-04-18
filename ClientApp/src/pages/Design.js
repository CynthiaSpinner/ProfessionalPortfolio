import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import "./Design.css";

const Design = () => {
  return (
    <div className="design-page">
      {/* Header Section */}
      <section className="header-section">
        <Container>
          <HeadingGroup
            title="Design Projects"
            subtitle="Showcasing my expertise in various design disciplines"
          />
        </Container>
      </section>

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
    </div>
  );
};

export default Design;
