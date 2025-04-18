import React from "react";
import { Container, Row, Col, Card, CardBody, Progress } from "reactstrap";

const Skills = () => {
  return (
    <Container className="py-5">
      <h1 className="text-center mb-5">My Skills</h1>

      <Row>
        <Col md="6" className="mb-4">
          <Card>
            <CardBody>
              <h3>Programming Languages</h3>
              <div className="skill-item">
                <div className="d-flex justify-content-between">
                  <span>JavaScript</span>
                  <span>90%</span>
                </div>
                <Progress value={90} />
              </div>
              {/* Add more skills */}
            </CardBody>
          </Card>
        </Col>

        <Col md="6" className="mb-4">
          <Card>
            <CardBody>
              <h3>Frameworks & Libraries</h3>
              <div className="skill-item">
                <div className="d-flex justify-content-between">
                  <span>React</span>
                  <span>85%</span>
                </div>
                <Progress value={85} />
              </div>
              {/* Add more skills */}
            </CardBody>
          </Card>
        </Col>

        <Col md="6" className="mb-4">
          <Card>
            <CardBody>
              <h3>Tools & Technologies</h3>
              <div className="skill-item">
                <div className="d-flex justify-content-between">
                  <span>Git</span>
                  <span>80%</span>
                </div>
                <Progress value={80} />
              </div>
              {/* Add more skills */}
            </CardBody>
          </Card>
        </Col>

        <Col md="6" className="mb-4">
          <Card>
            <CardBody>
              <h3>Soft Skills</h3>
              <div className="skill-item">
                <div className="d-flex justify-content-between">
                  <span>Communication</span>
                  <span>95%</span>
                </div>
                <Progress value={95} />
              </div>
              {/* Add more skills */}
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default Skills;
