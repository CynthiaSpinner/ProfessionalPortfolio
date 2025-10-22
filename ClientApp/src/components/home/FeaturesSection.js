import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import HeadingGroup from "../HeadingGroup";
import Button from "../Button";

const FeaturesSection = ({ featuresData }) => {
  // Use provided data - handle undefined gracefully
  const data = featuresData || {};

  return (
    <section className="features-section py-5">
      <Container>
        <HeadingGroup
          title={data?.title || "Key Skills & Technologies"}
          subtitle={data?.subtitle}
          className="text-center mb-5"
        />
        {data?.description && (
          <p className="text-center mb-4 text-muted">{data.description}</p>
        )}
        <Row className="g-4">
          {data?.features?.map((feature, idx) => (
            <Col md={4} key={idx}>
              <Card 
                className={`feature-card ${feature.link ? "clickable-card" : ""}`}
                onClick={() => {
                  if (feature.link) {
                    window.location.href = feature.link;
                  }
                }}
                style={{ 
                  cursor: feature.link ? 'pointer' : 'default',
                  transition: 'all 0.3s ease',
                  height: '100%'
                }}
                onMouseEnter={(e) => {
                  if (feature.link) {
                    e.currentTarget.style.transform = 'translateY(-5px)';
                    e.currentTarget.style.boxShadow = '0 10px 25px rgba(129, 140, 248, 0.2)';
                  }
                }}
                onMouseLeave={(e) => {
                  if (feature.link) {
                    e.currentTarget.style.transform = 'translateY(0)';
                    e.currentTarget.style.boxShadow = '';
                  }
                }}
              >
                <div className="text-center mb-3">
                  {feature.icon && <i className={`${feature.icon} fa-2x mb-2`} style={{ color: "#818cf8" }}></i>}
                </div>
                <HeadingGroup
                  title={feature.title}
                  subtitle={feature.subtitle}
                  headingLevel="h4"
                />
                {feature.description && (
                  <p className="text-center mt-3" style={{ 
                    fontSize: '0.9rem', 
                    color: '#d1d5db',
                    lineHeight: '1.5'
                  }}>
                    {feature.description}
                  </p>
                )}
                {feature.link && (
                  <div className="text-center mt-3">
                    <Button href={feature.link} className="btn-sm">
                      Explore
                    </Button>
                  </div>
                )}
              </Card>
            </Col>
          ))}
        </Row>
      </Container>
    </section>
  );
};

export default FeaturesSection; 