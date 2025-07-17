import React, { useCallback } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../HeadingGroup";
import Card from "../Card";
import { PortfolioService } from "../../services/PortfolioService";
import { useWebSocket, usePortfolioData } from "../../hooks";

const FeaturesSection = () => {
  const fetchFeaturesData = useCallback(async () => {
    const data = await PortfolioService.getFeatures();
    return data;
  }, []);

  const { data: featuresData, loading } = usePortfolioData(fetchFeaturesData);

  // Use shared WebSocket hook
  useWebSocket('featuresDataUpdated', fetchFeaturesData);

  if (loading) {
    return (
      <section className="features-section py-5">
        <Container>
          <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "200px" }}>
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Loading features section...</span>
            </div>
          </div>
        </Container>
      </section>
    );
  }

  return (
    <section className="features-section py-5">
      <Container>
        <HeadingGroup
          title={featuresData?.title || "Key Skills & Technologies"}
          subtitle={featuresData?.subtitle}
          className="text-center mb-5"
        />
        {featuresData?.description && (
          <p className="text-center mb-4 text-muted">{featuresData.description}</p>
        )}
        <Row className="g-4">
          {featuresData?.features?.map((feature, idx) => (
            <Col md={4} key={idx}>
              <Card 
                className={feature.link ? "clickable-card" : ""}
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
                />
                {feature.description && (
                  <p className="text-muted text-center mt-3" style={{ fontSize: '0.9rem' }}>
                    {feature.description}
                  </p>
                )}
                {feature.link && (
                  <div className="text-center mt-3">
                    <small className="text-primary" style={{ fontWeight: '500' }}>
                      Click to explore →
                    </small>
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