import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import Button from "./Button";
import "../styles/Header.css";

const Header = ({
  title,
  subtitle,
  description,
  backgroundImageUrl,
  backgroundVideoUrl,
  overlayColor = "#000000",
  overlayOpacity = 0.5,
  primaryButtonText = "View Projects",
  primaryButtonUrl = "/projects",
  children,
  showButtons = false,
  refreshing = false
}) => {
  // Build background style
  let backgroundStyle = {};
  
  if (backgroundVideoUrl) {
    backgroundStyle = {
      position: 'relative',
      overflow: 'hidden'
    };
  } else if (backgroundImageUrl) {
    backgroundStyle = {
      backgroundImage: `url(${backgroundImageUrl})`,
      backgroundSize: 'cover',
      backgroundPosition: 'center',
      backgroundRepeat: 'no-repeat',
      position: 'relative'
    };
  } else {
    backgroundStyle = {
      background: "linear-gradient(135deg, rgba(75, 75, 90, 0.3), rgba(85, 85, 105, 0.2), rgba(227, 235, 255, 0.1))"
    };
  }

  return (
    <header className="page-header" style={backgroundStyle}>
      {/* Video Background */}
      {backgroundVideoUrl && (
        <video
          autoPlay
          muted
          loop
          style={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            zIndex: 1
          }}
        >
          <source src={backgroundVideoUrl} type="video/mp4" />
          Your browser does not support the video tag.
        </video>
      )}

      {/* Overlay */}
      {(backgroundImageUrl || backgroundVideoUrl) && (
        <div
          style={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundColor: overlayColor,
            opacity: overlayOpacity,
            zIndex: 2
          }}
        />
      )}

      {/* Content */}
      <Container style={{ position: 'relative', zIndex: 3 }}>
        <Row className="justify-content-center">
          <Col lg={8} className="text-center">
            <HeadingGroup title={title} subtitle={subtitle} />
            {description && (
              <p className="lead mt-3" style={{ color: "#6b7280", fontSize: "1.1rem" }}>
                {description}
              </p>
            )}
            {showButtons && (
              <div className="mt-4">
                <Button href={primaryButtonUrl}>{primaryButtonText}</Button>
              </div>
            )}
            {children}
          </Col>
        </Row>
        
        {/* Refresh Indicator */}
        {refreshing && (
          <div 
            style={{
              position: 'absolute',
              top: '20px',
              right: '20px',
              width: '20px',
              height: '20px',
              border: '2px solid rgba(255, 255, 255, 0.3)',
              borderTop: '2px solid rgba(255, 255, 255, 0.8)',
              borderRadius: '50%',
              animation: 'spin 1s linear infinite',
              zIndex: 4
            }}
          />
        )}
      </Container>
    </header>
  );
};

export default Header;
