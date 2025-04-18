import React from "react";
import { Container, Row, Col, Card, CardBody } from "reactstrap";

const About = () => {
  return (
    <Container className="py-5">
      <Row>
        <Col md="4">
          <Card className="mb-4">
            <CardBody className="text-center">
              <img
                src="/path-to-your-image.jpg"
                alt="Profile"
                className="rounded-circle mb-3"
                style={{ width: "200px", height: "200px", objectFit: "cover" }}
              />
              <h2>Your Name</h2>
              <p className="text-muted">Your Title</p>
              <div className="social-links">
                <a
                  href="https://github.com/yourusername"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <i className="fab fa-github"></i>
                </a>
                <a
                  href="https://linkedin.com/in/yourusername"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <i className="fab fa-linkedin"></i>
                </a>
              </div>
            </CardBody>
          </Card>
        </Col>
        <Col md="8">
          <Card className="mb-4">
            <CardBody>
              <h3>About Me</h3>
              <p>
                Your biography goes here. Talk about your passion for
                development, your experience, and what drives you.
              </p>
            </CardBody>
          </Card>

          <Card className="mb-4">
            <CardBody>
              <h3>Work Experience</h3>
              <div className="timeline">
                {/* Work experience items will be mapped here */}
                <div className="timeline-item">
                  <h4>Job Title</h4>
                  <p className="text-muted">
                    Company Name | Start Date - End Date
                  </p>
                  <p>Description of your role and achievements</p>
                </div>
              </div>
            </CardBody>
          </Card>

          <Card>
            <CardBody>
              <h3>Education</h3>
              <div className="timeline">
                {/* Education items will be mapped here */}
                <div className="timeline-item">
                  <h4>Degree</h4>
                  <p className="text-muted">
                    Institution | Start Date - End Date
                  </p>
                  <p>Description of your studies and achievements</p>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default About;
