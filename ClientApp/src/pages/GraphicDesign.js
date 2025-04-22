import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import Button from "../components/Button";
import HeadingGroup from "../components/HeadingGroup";
import PhotoGallery from "../components/PhotoGallery";
import "./GraphicDesign.css";

// Sample images for each section
const photographyImages = [
  {
    src: "https://via.placeholder.com/800x400",
    title: "Nature Photography",
    alt: "Nature scene",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Portrait Work",
    alt: "Portrait photo",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Urban Landscape",
    alt: "Cityscape",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Wildlife",
    alt: "Wildlife photo",
  },
];

const adobeImages = [
  {
    src: "https://via.placeholder.com/1200x400",
    title: "Brand Identity",
    alt: "Brand design",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Print Design",
    alt: "Print material",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Digital Art",
    alt: "Digital artwork",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Packaging Design",
    alt: "Product packaging",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Web Design",
    alt: "Website design",
  },
];

const afterEffectsImages = [
  {
    src: "https://via.placeholder.com/800x800",
    title: "Motion Graphics",
    alt: "Animated graphic",
  },
  {
    src: "https://via.placeholder.com/800x800",
    title: "Title Sequence",
    alt: "Video title",
  },
  {
    src: "https://via.placeholder.com/800x800",
    title: "Visual Effects",
    alt: "VFX shot",
  },
];

const photoshopImages = [
  {
    src: "https://via.placeholder.com/800x400",
    title: "Photo Retouching",
    alt: "Retouched photo",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Digital Painting",
    alt: "Digital art",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Photo Manipulation",
    alt: "Manipulated image",
  },
  {
    src: "https://via.placeholder.com/800x400",
    title: "Collage Art",
    alt: "Photo collage",
  },
];

const lightroomImages = [
  {
    src: "https://via.placeholder.com/1200x400",
    title: "Color Correction",
    alt: "Color corrected photo",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Preset Development",
    alt: "Lightroom preset",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Batch Editing",
    alt: "Edited photos",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Portrait Enhancement",
    alt: "Enhanced portrait",
  },
  {
    src: "https://via.placeholder.com/600x600",
    title: "Black & White",
    alt: "B&W photo",
  },
];

const GraphicDesign = () => {
  return (
    <div className="graphic-design-page">
      <Header
        title="Graphic Design"
        subtitle="Discover my visual design projects and creative work."
      />

      {/* Photography Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Photography Showcase"
                subtitle="A collection of my best photography work"
              />
              <PhotoGallery images={photographyImages} layout="landscape4" />
              <Button href="/portfolio/photography">View Full Portfolio</Button>
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
              <PhotoGallery images={adobeImages} layout="left-large-grid4" />
              <Button href="/portfolio/adobe">View Full Portfolio</Button>
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
              <PhotoGallery images={afterEffectsImages} layout="square3" />
              <Button href="/portfolio/aftereffects">
                View Full Portfolio
              </Button>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Photoshop Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Photoshop Projects"
                subtitle="Professional image editing and manipulation"
              />
              <PhotoGallery images={photoshopImages} layout="landscape4" />
              <Button href="/portfolio/photoshop">View Full Portfolio</Button>
            </Col>
          </Row>
        </Container>
      </section>

      {/* Lightroom Section */}
      <section className="portfolio-section">
        <Container>
          <Row>
            <Col>
              <HeadingGroup
                title="Lightroom Projects"
                subtitle="Professional photo editing and organization"
              />
              <PhotoGallery
                images={lightroomImages}
                layout="left-large-grid4"
              />
              <Button href="/portfolio/lightroom">View Full Portfolio</Button>
            </Col>
          </Row>
        </Container>
      </section>

      <Footer />
    </div>
  );
};

export default GraphicDesign;
