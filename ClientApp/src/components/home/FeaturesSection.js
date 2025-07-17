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

  const { data: featuresData, loading, refetch: fetchFeaturesData } = usePortfolioData(fetchFeaturesData);

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
              <Card>
                <div className="text-center mb-3">
                  {feature.icon && <i className={`${feature.icon} fa-2x mb-2`} style={{ color: "#818cf8" }}></i>}
                </div>
                <HeadingGroup
                  title={feature.title}
                  subtitle={feature.description}
                />
              </Card>
            </Col>
          ))}
        </Row>
      </Container>
    </section>
  );
};

export default FeaturesSection; 