import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import "./GraphicDesign.css";

const GraphicDesign = () => {
  return (
    <div className="graphic-design-page">
      {/* Header Section */}
      <section className="header-section">
        <Container>
          <HeadingGroup
            title="Graphic Design Portfolio"
            subtitle="Showcasing my expertise in various design tools and creative projects"
          />
        </Container>
      </section>

      {/* Photography Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Photography Showcase"
                subtitle="A collection of my best photography work"
              />
              <div className="video-container">
                {/* Placeholder for video embed */}
                <div className="video-placeholder">
                  Video will be embedded here
                </div>
              </div>
              <a href="/portfolio/photography" className="btn btn-primary">
                View Full Portfolio
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Adobe Work Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Adobe Creative Suite Projects"
                subtitle="Professional design work using Adobe's industry-standard tools"
              />
              <div className="video-container">
                {/* Placeholder for video embed */}
                <div className="video-placeholder">
                  Video will be embedded here
                </div>
              </div>
              <a href="/portfolio/adobe" className="btn btn-primary">
                View Full Portfolio
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* AfterEffects Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Motion Graphics & Animation"
                subtitle="Dynamic visual content created with Adobe AfterEffects"
              />
              <div className="video-container">
                {/* Placeholder for video embed */}
                <div className="video-placeholder">
                  Video will be embedded here
                </div>
              </div>
              <a href="/portfolio/aftereffects" className="btn btn-primary">
                View Full Portfolio
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Photoshop Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <h2>Photoshop Projects</h2>
              <div className="video-container">
                {/* Placeholder for video embed */}
                <div className="video-placeholder">
                  Video will be embedded here
                </div>
              </div>
              <a href="/portfolio/photoshop" className="btn btn-primary">
                View Full Portfolio
              </a>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Lightroom Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <h2>Lightroom Projects</h2>
              <div className="video-container">
                {/* Placeholder for video embed */}
                <div className="video-placeholder">
                  Video will be embedded here
                </div>
              </div>
              <a href="/portfolio/lightroom" className="btn btn-primary">
                View Full Portfolio
              </a>
            </Col>
          </Row>
        </Container>
      </section>
    </div>
  );
};

export default GraphicDesign;
