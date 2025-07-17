import React, { useCallback } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../HeadingGroup";
import Card from "../Card";
import { PortfolioService } from "../../services/PortfolioService";
import { useWebSocket, usePortfolioData } from "../../hooks";

const FeaturesSection = ({ featuresData }) => {
  const fallbackFeatures = {
    title: "Key Skills & Technologies",
    subtitle: "Explore my expertise across different domains",
    features: [
      {
        title: "Frontend Development",
        subtitle: "React, JavaScript, HTML5, CSS3, Bootstrap",
        description: "Building responsive and interactive user interfaces with modern frameworks and best practices.",
        icon: "fas fa-code",
        link: "/projects?category=frontend"
      },
      {
        title: "Backend Development", 
        subtitle: ".NET Core, C#, RESTful APIs, SQL Server",
        description: "Creating robust server-side applications and APIs with enterprise-grade technologies.",
        icon: "fas fa-server",
        link: "/projects?category=backend"
      },
      {
        title: "Design & Tools",
        subtitle: "Adobe Creative Suite, UI/UX Design, Git, Docker",
        description: "Crafting beautiful designs and managing development workflows with professional tools.",
        icon: "fas fa-palette",
        link: "/projects?category=design"
      }
    ]
  };

  // Use provided data or fallback to default
  const data = featuresData || fallbackFeatures;

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