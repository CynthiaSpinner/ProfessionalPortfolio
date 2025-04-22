import React, { useState } from "react";
import { Navbar as BootstrapNavbar, Nav, Container } from "react-bootstrap";
import { Link } from "react-router-dom";
import "./NavMobile.css";

const Navbar = () => {
  const [expanded, setExpanded] = useState(false);

  return (
    <BootstrapNavbar
      bg="dark"
      variant="dark"
      expand="lg"
      expanded={expanded}
      onToggle={() => setExpanded(!expanded)}
    >
      <Container>
        <BootstrapNavbar.Brand as={Link} to="/">
          Portfolio
        </BootstrapNavbar.Brand>
        <BootstrapNavbar.Toggle aria-controls="navbar-nav" />
        <BootstrapNavbar.Collapse id="navbar-nav">
          <Nav className="ms-auto">
            <Nav.Link as={Link} to="/" onClick={() => setExpanded(false)}>
              Home
            </Nav.Link>
            <Nav.Link as={Link} to="/about" onClick={() => setExpanded(false)}>
              About
            </Nav.Link>
            <Nav.Link
              as={Link}
              to="/projects"
              onClick={() => setExpanded(false)}
            >
              Projects
            </Nav.Link>
            <Nav.Link
              as={Link}
              to="/graphic-design"
              onClick={() => setExpanded(false)}
            >
              Graphic Design
            </Nav.Link>
            <Nav.Link as={Link} to="/design" onClick={() => setExpanded(false)}>
              Design
            </Nav.Link>
          </Nav>
        </BootstrapNavbar.Collapse>
      </Container>
    </BootstrapNavbar>
  );
};

export default Navbar;
