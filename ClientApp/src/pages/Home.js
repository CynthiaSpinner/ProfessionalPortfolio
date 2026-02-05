import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "../components/HeadingGroup";
import Footer from "../components/Footer";
import Card from "../components/Card";
import Header from "../components/Header";
import "../styles/Home.css";
import CTA from "../components/CTA";

const Home = () => {
  return (
    <div className="home-page">
      <Header
        title="Welcome to My Portfolio"
        subtitle="I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications."
        showButtons={true}
      />

      {/* Features List Section */}
      <section className="features-section py-5">
        <Container>
          <HeadingGroup
            title="Key Skills & Technologies"
            className="text-center mb-5"
          />
          <Row className="g-4">
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Frontend Development"
                  subtitle="React, JavaScript, HTML5, CSS3, Bootstrap"
                />
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Backend Development"
                  subtitle=".NET Core, C#, RESTful APIs, MySQL"
                />
              </Card>
            </Col>
            <Col md={4}>
              <Card>
                <HeadingGroup
                  title="Design & Tools"
                  subtitle="Adobe Creative Suite, UI/UX Design, Git, Docker"
                />
              </Card>
            </Col>
          </Row>
        </Container>
      </section>

      {/* CTA Section */}
      <CTA
        title="Ready to Start a Project?"
        subtitle="Let's work together to bring your ideas to life."
      />

      
      <Footer />
    </div>
  );
};

export default Home;
