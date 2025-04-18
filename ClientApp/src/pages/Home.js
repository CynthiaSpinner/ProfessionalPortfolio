import React from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import "./Home.css";

const Home = () => {
  return (
    <div className="home-page">
      {/* Hero Header Section */}
      <section className="hero-section">
        <Container>
          <Row className="align-items-center min-vh-100">
            <Col lg={8}>
              <HeadingGroup
                title="Welcome to My Portfolio"
                subtitle="I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications."
              />
              <Button variant="primary" size="lg" href="/projects">
                View My Work
              </Button>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Features List Section */}
      <section className="features-section py-5">
        <Container>
          <HeadingGroup
            title="Key Skills & Technologies"
            className="text-center mb-5"
          />
          <Row className="g-4">
            <Col md={4}>
              <div className="feature-card">
                <HeadingGroup
                  title="Frontend Development"
                  subtitle="React, JavaScript, HTML5, CSS3, Bootstrap"
                />
              </div>
            </Col>
            <Col md={4}>
              <div className="feature-card">
                <HeadingGroup
                  title="Backend Development"
                  subtitle=".NET Core, C#, RESTful APIs, MySQL"
                />
              </div>
            </Col>
            <Col md={4}>
              <div className="feature-card">
                <HeadingGroup
                  title="Design & Tools"
                  subtitle="Adobe Creative Suite, UI/UX Design, Git, Docker"
                />
              </div>
            </Col>
          </Row>
        </Container>
      </section>

      {/* CTA Section */}
      <section className="cta-section py-5 bg-dark">
        <Container className="text-center">
          <HeadingGroup
            title="Ready to Start a Project?"
            subtitle="Let's work together to bring your ideas to life."
            className="mb-4"
          />
          <Button variant="primary" size="lg" href="/contact">
            Get in Touch
          </Button>
        </Container>
      </section>

      {/* Contact Section */}
      <section className="contact-section py-5">
        <Container>
          <Row className="justify-content-center">
            <Col md={8} className="text-center">
              <HeadingGroup
                title="Let's Connect"
                subtitle="I'm always interested in hearing about new projects and opportunities."
                className="mb-4"
              />
              <Button variant="outline-primary" size="lg" href="/contact">
                Contact Me
              </Button>
            </Col>
          </Row>
        </Container>
      </section>
    </div>
  );
};

export default Home;
