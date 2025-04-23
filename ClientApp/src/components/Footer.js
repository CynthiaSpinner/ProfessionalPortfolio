import React from "react";
import { Container } from "react-bootstrap";
import "../styles/Footer.css";

const Footer = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="footer">
      <Container>
        <div className="footer-content">
          <p className="footer-text">
            © {currentYear} Cynthia Spinner. All rights reserved.
          </p>
          <div className="footer-links">
            <a
              href="https://github.com/yourusername"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-link"
            >
              <i className="fab fa-github fa-lg"></i>
            </a>
            <a
              href="https://linkedin.com/in/yourusername"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-link"
            >
              <i className="fab fa-linkedin fa-lg"></i>
            </a>
            <a href="/contact" className="contact-button">
              Get in Touch
            </a>
          </div>
        </div>
      </Container>
    </footer>
  );
};

export default Footer;
