import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import Button from "../components/Button";
import HeadingGroup from "../components/HeadingGroup";
import PhotoGallery from "../components/PhotoGallery";
import "../styles/Design.css";

const Design = () => {
  // Sample images for each section
  const databaseImages = [
    { src: "/images/database/db1.jpg", alt: "Database Schema 1", title: "ERD Design" },
    { src: "/images/database/db2.jpg", alt: "Database Schema 2", title: "Table Structure" },
    { src: "/images/database/db3.jpg", alt: "Database Schema 3", title: "Relationships" }
  ];

  const softwareImages = [
    { src: "/images/software/sw1.jpg", alt: "Software Design 1", title: "System Architecture" },
    { src: "/images/software/sw2.jpg", alt: "Software Design 2", title: "Component Diagram" },
    { src: "/images/software/sw3.jpg", alt: "Software Design 3", title: "Sequence Diagram" },
    { src: "/images/software/sw4.jpg", alt: "Software Design 4", title: "Class Diagram" },
    { src: "/images/software/sw5.jpg", alt: "Software Design 5", title: "Deployment Diagram" }
  ];

  const webImages = [
    { src: "/images/web/web1.jpg", alt: "Web Design 1", title: "Homepage Design" },
    { src: "/images/web/web2.jpg", alt: "Web Design 2", title: "Dashboard Layout" },
    { src: "/images/web/web3.jpg", alt: "Web Design 3", title: "Mobile View" },
    { src: "/images/web/web4.jpg", alt: "Web Design 4", title: "User Flow" }
  ];

  const sitemapImages = [
    { src: "/images/sitemap/sm1.jpg", alt: "Sitemap 1", title: "Main Navigation" },
    { src: "/images/sitemap/sm2.jpg", alt: "Sitemap 2", title: "Content Structure" },
    { src: "/images/sitemap/sm3.jpg", alt: "Sitemap 3", title: "User Flow" }
  ];

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
              <PhotoGallery images={databaseImages} layout="square3" />
              <Button href="/portfolio/database-design" variant="primary">
                View Project
              </Button>
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
              <PhotoGallery images={softwareImages} layout="mixed5" />
              <Button href="/portfolio/software-design" variant="primary">
                View Project
              </Button>
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
              <PhotoGallery images={webImages} layout="mixed5" />
              <Button href="/portfolio/web-design" variant="primary">
                View Project
              </Button>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Sitemapping Section */}
      <section className="project-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Sitemapping"
                subtitle="Information architecture and site structure planning"
              />
              <PhotoGallery images={sitemapImages} layout="square3" />
              <Button href="/portfolio/sitemapping" variant="primary">
                View Project
              </Button>
            </Col>
          </Row>
        </Container>
      </section>

      <Footer />
    </div>
  );
};

export default Design;
