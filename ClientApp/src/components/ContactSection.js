import React, { useState } from "react";
import {
  Container,
  Row,
  Col,
  Form,
  FormGroup,
  Label,
  Input,
  Button,
  Alert,
} from "reactstrap";
import { usePortfolio } from "../context/PortfolioContext";
import "./ContactSection.css";

function ContactSection() {
  const { about } = usePortfolio();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    message: "",
  });
  const [status, setStatus] = useState({
    type: "",
    message: "",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // Here you would typically make an API call to your backend
      // For now, we'll just simulate a successful submission
      setStatus({
        type: "success",
        message: "Thank you for your message! I will get back to you soon.",
      });
      setFormData({ name: "", email: "", message: "" });
    } catch (error) {
      setStatus({
        type: "danger",
        message: "There was an error sending your message. Please try again.",
      });
    }
  };

  return (
    <section className="contact-section py-5 bg-light">
      <Container>
        <h2 className="text-center mb-5">Get In Touch</h2>
        <Row>
          <Col md="6" className="mb-4">
            <div className="contact-info">
              <h3>Contact Information</h3>
              <p>
                <strong>Email:</strong>{" "}
                {about?.email || "your.email@example.com"}
              </p>
              <p>
                <strong>Location:</strong> {about?.location || "Your Location"}
              </p>
              <div className="social-links mt-4">
                {about?.socialLinks?.map((link) => (
                  <a
                    key={link.platform}
                    href={link.url}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="me-3"
                  >
                    <i className={`fab fa-${link.platform}`}></i>
                  </a>
                ))}
              </div>
            </div>
          </Col>
          <Col md="6">
            <Form onSubmit={handleSubmit}>
              {status.message && (
                <Alert color={status.type}>{status.message}</Alert>
              )}
              <FormGroup>
                <Label for="name">Name</Label>
                <Input
                  type="text"
                  name="name"
                  id="name"
                  value={formData.name}
                  onChange={handleChange}
                  required
                />
              </FormGroup>
              <FormGroup>
                <Label for="email">Email</Label>
                <Input
                  type="email"
                  name="email"
                  id="email"
                  value={formData.email}
                  onChange={handleChange}
                  required
                />
              </FormGroup>
              <FormGroup>
                <Label for="message">Message</Label>
                <Input
                  type="textarea"
                  name="message"
                  id="message"
                  value={formData.message}
                  onChange={handleChange}
                  required
                />
              </FormGroup>
              <Button color="primary" type="submit">
                Send Message
              </Button>
            </Form>
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default ContactSection;
