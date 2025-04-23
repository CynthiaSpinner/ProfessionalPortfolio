import React, { useState } from "react";
import { Container, Row, Col, Form } from "react-bootstrap";
import Header from "../components/Header";
import Footer from "../components/Footer";
import Button from "../components/Button";
import "../styles/Contact.css";

const Contact = () => {
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    message: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // TODO: Implement form submission logic
    console.log("Form submitted:", formData);
  };

  return (
    <div className="contact-page">
      <Header 
        title="Contact Me"
        subtitle="Let's connect and discuss how we can work together."
      />

      <section className="contact-section py-5">
        <Container className="px-4">
          <Row className="justify-content-center">
            <Col md={8} lg={6}>
              <div className="contact-form">
                <h2>Get in Touch</h2>
                <Form onSubmit={handleSubmit}>
                  <Form.Group className="mb-4">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                      type="text"
                      name="name"
                      value={formData.name}
                      onChange={handleChange}
                      required
                      placeholder="Enter your name"
                    />
                  </Form.Group>
                  <Form.Group className="mb-4">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                      type="email"
                      name="email"
                      value={formData.email}
                      onChange={handleChange}
                      required
                      placeholder="Enter your email"
                    />
                  </Form.Group>
                  <Form.Group className="mb-4">
                    <Form.Label>Message</Form.Label>
                    <Form.Control
                      as="textarea"
                      rows={5}
                      name="message"
                      value={formData.message}
                      onChange={handleChange}
                      required
                      placeholder="Enter your message"
                    />
                  </Form.Group>
                  <div className="text-center">
                    <Button type="submit">Send Message</Button>
                  </div>
                </Form>
              </div>
            </Col>
          </Row>
        </Container>
      </section>
      <Footer />
    </div>
  );
};

export default Contact;
