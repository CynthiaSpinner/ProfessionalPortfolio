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
  const isImage = !!backgroundImageUrl && !backgroundVideoUrl;
  const isVideo = !!backgroundVideoUrl;
  const hasMedia = isImage || isVideo;

  const backgroundLayerStyle = isImage
    ? {
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundImage: `url(${backgroundImageUrl})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        backgroundRepeat: 'no-repeat',
        zIndex: 0
      }
    : {};

  return (
    <header
      className="page-header"
      style={{
        position: 'relative',
        overflow: 'hidden',
        ...(!hasMedia && {
          background: "linear-gradient(135deg, rgba(75, 75, 90, 0.3), rgba(85, 85, 105, 0.2), rgba(227, 235, 255, 0.1))"
        })
      }}
    >
      {/* Image background layer - fills hero, contained */}
      {isImage && <div className="page-header__bg" style={backgroundLayerStyle} aria-hidden="true" />}

      {/* Nav shadow: soft gradient over the image at the top, blends into the image */}
      {hasMedia && (
        <div
          className="page-header__nav-shadow"
          style={{
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            height: '32%',
            background: 'linear-gradient(to bottom, rgba(0,0,0,0.42) 0%, rgba(0,0,0,0.18) 40%, transparent 100%)',
            zIndex: 1,
            pointerEvents: 'none'
          }}
          aria-hidden="true"
        />
      )}

      {/* Video Background */}
      {isVideo && (
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
      {hasMedia && (
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
