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

const Projects = () => {
  return (
    <Container className="py-5">
      <h1 className="text-center mb-5">My Projects</h1>
      <Row>
        <Col md="4" className="mb-4">
          <Card className="h-100">
            <img
              src="/path-to-project-image.jpg"
              alt="Project"
              className="card-img-top"
              style={{ height: "200px", objectFit: "cover" }}
            />
            <CardBody>
              <CardTitle tag="h4">Project Title</CardTitle>
              <CardText>
                Brief description of the project, technologies used, and your
                role.
              </CardText>
              <div className="d-flex justify-content-between">
                <Button color="primary" href="/project-url" target="_blank">
                  View Project
                </Button>
                <Button color="secondary" href="/github-url" target="_blank">
                  GitHub
                </Button>
              </div>
            </CardBody>
          </Card>
        </Col>
        {/* Add more project cards here */}
      </Row>
    </Container>
  );
};

export default Projects;
