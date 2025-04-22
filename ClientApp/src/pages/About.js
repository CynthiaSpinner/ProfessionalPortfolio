import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import Button from "../components/Button";
import Card from "../components/Card";
import HeadingGroup from "../components/HeadingGroup";
import "./About.css";

const About = () => {
  return (
    <div className="about-page">
      <Header
        title="About Me"
        subtitle="Get to know more about my journey, skills, and what drives me as a developer."
      />

      {/* About Section */}
      <section className="about-section py-5">
        <Container>
          <Row>
            <Col lg={6}>
              <img
                src="https://via.placeholder.com/400x400/333333/FFFFFF?text=Profile"
                alt="Profile"
                className="img-fluid rounded"
              />
            </Col>
            <Col lg={6}>
              <HeadingGroup
                title="About Me"
                subtitle="I am a passionate software engineer with a strong background in full-stack development and a keen eye for design. My journey in technology began with a love for problem-solving and has evolved into a career focused on creating elegant, efficient solutions."
              />
              <div className="social-links mt-4">
                <a
                  href="https://github.com"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <i className="fab fa-github fa-2x me-3"></i>
                </a>
                <a
                  href="https://linkedin.com"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <i className="fab fa-linkedin fa-2x me-3"></i>
                </a>
                <a href="mailto:example@email.com">
                  <i className="fas fa-envelope fa-2x"></i>
                </a>
              </div>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Skills Section */}
      <section className="skills-section py-5">
        <Container>
          <HeadingGroup
            title="My Skills"
            subtitle="A comprehensive overview of my technical expertise and professional capabilities"
            className="text-center mb-5"
          />
          <Row className="g-4">
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Frontend Development"
                  subtitle="React, JavaScript, HTML5, CSS3, Bootstrap"
                />
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Backend Development"
                  subtitle=".NET Core, C#, RESTful APIs, MySQL"
                />
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Design & Tools"
                  subtitle="Adobe Creative Suite, UI/UX Design, Git, Docker"
                />
              </Card>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Team Section */}
      <section className="team-section py-5 bg-light">
        <Container>
          <h2 className="text-center mb-5">Collaborators</h2>
          <Row className="g-4">
            <Col md={4}>
              <Card>
                <img
                  src="https://via.placeholder.com/150x150/333333/FFFFFF?text=Team+1"
                  alt="Team Member"
                  className="img-fluid rounded-circle mb-3"
                />
                <h3>John Doe</h3>
                <p className="text-muted">Frontend Developer</p>
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <img
                  src="https://via.placeholder.com/150x150/333333/FFFFFF?text=Team+2"
                  alt="Team Member"
                  className="img-fluid rounded-circle mb-3"
                />
                <h3>Jane Smith</h3>
                <p className="text-muted">UI/UX Designer</p>
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <img
                  src="https://via.placeholder.com/150x150/333333/FFFFFF?text=Team+3"
                  alt="Team Member"
                  className="img-fluid rounded-circle mb-3"
                />
                <h3>Mike Johnson</h3>
                <p className="text-muted">Backend Developer</p>
              </Card>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Stats Section */}
      <section className="stats-section py-5">
        <Container>
          <Row className="g-4">
            <Col md={3} className="text-center">
              <Card>
                <h3 className="display-4 fw-bold">5+</h3>
                <p className="text-muted">Years Experience</p>
              </Card>
            </Col>
            <Col md={3} className="text-center">
              <Card>
                <h3 className="display-4 fw-bold">50+</h3>
                <p className="text-muted">Projects Completed</p>
              </Card>
            </Col>
            <Col md={3} className="text-center">
              <Card>
                <h3 className="display-4 fw-bold">10+</h3>
                <p className="text-muted">Certifications</p>
              </Card>
            </Col>
            <Col md={3} className="text-center">
              <Card>
                <h3 className="display-4 fw-bold">100%</h3>
                <p className="text-muted">Client Satisfaction</p>
              </Card>
            </Col>
          </Row>
        </Container>
      </section>

      {/* CTA Section */}
      <section className="cta-section py-5 bg-light">
        <Container className="text-center">
          <h2 className="mb-4">Interested in Working Together?</h2>
          <p className="lead mb-4">
            Let's discuss how we can bring your ideas to life.
          </p>
          <Button href="/contact" size="lg">
            Get in Touch
          </Button>
        </Container>
      </section>

      <Footer />
    </div>
  );
};

export default About;
