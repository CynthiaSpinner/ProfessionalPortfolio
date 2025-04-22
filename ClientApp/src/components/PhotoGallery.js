import React from "react";
import { Row, Col } from "react-bootstrap";
import "./PhotoGallery.css";

const PhotoGallery = ({ images, layout = "default" }) => {
  const getLayoutClass = () => {
    switch (layout) {
      case "landscape4":
        return "layout-landscape4";
      case "mixed5":
        return "layout-mixed5";
      case "square3":
        return "layout-square3";
      case "left-large-grid4":
        return "layout-left-large-grid4";
      default:
        return "";
    }
  };

  return (
    <div className={`photo-gallery ${getLayoutClass()}`}>
      <Row className="g-4">
        {images.map((image, index) => (
          <Col
            key={index}
            xs={layout === "landscape4" ? 12 : 6}
            md={
              layout === "landscape4"
                ? 6
                : layout === "left-large-grid4" && index === 0
                ? 6
                : 3
            }
            lg={
              layout === "landscape4"
                ? 3
                : layout === "mixed5" && index === 0
                ? 12
                : layout === "left-large-grid4" && index === 0
                ? 6
                : 3
            }
          >
            <div className="gallery-item">
              <img
                src={image.src}
                alt={image.alt || `Gallery image ${index + 1}`}
                className="img-fluid"
              />
              <div className="overlay">
                <div className="overlay-content">
                  <p>{image.title}</p>
                </div>
              </div>
            </div>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default PhotoGallery;
