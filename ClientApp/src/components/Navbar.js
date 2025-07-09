import React, { useState } from "react";
import { Link } from "react-router-dom";
import "../styles/Navbar.css";

const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <nav
      className="navbar navbar-expand-lg navbar-dark bg-dark"
      style={{ minHeight: "60px" }}
    >
      <div className="container" style={{ maxWidth: "1200px", padding: "0 2rem" }}>
        <h1
          className="navbar-brand mb-0"
          style={{
            color: "#818cf8",
            fontFamily: "'Playfair Display', serif",
            fontStyle: "italic",
            fontWeight: "500",
            fontSize: "1.5rem",
            letterSpacing: "0.05em",
            textShadow:
              "0 0 10px rgba(129, 140, 248, 0.3), 0 0 20px rgba(129, 140, 248, 0.2)",
            position: "relative",
            padding: "0 10px",
            transition: "all 0.3s ease",
            display: "flex",
            alignItems: "center",
            marginRight: "auto"
          }}
        >
          <span>C</span>
          <span className="gradient-text">ynthia </span>
          <span>S</span>
          <span className="gradient-text">pinner</span>
        </h1>
        <button
          className={`navbar-toggler ${isMenuOpen ? "menu-open" : ""}`}
          type="button"
          onClick={() => setIsMenuOpen(!isMenuOpen)}
          aria-expanded={isMenuOpen}
          aria-controls="navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div
          className={`collapse navbar-collapse ${isMenuOpen ? "show" : ""}`}
          id="navbarNav"
        >
          <ul
            className="navbar-nav"
            style={{ 
              width: "100%",
              display: "flex",
              justifyContent: "space-between",
              marginTop: "0.15rem",
              padding: "0 2rem"
            }}
          >
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Home
              </Link>
            </li>
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/projects"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Projects
              </Link>
            </li>
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/graphic-design"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Graphic Design
              </Link>
            </li>
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/design"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Design
              </Link>
            </li>
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/about"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                About
              </Link>
            </li>
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/contact"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Contact
              </Link>
            </li>
            <li className="nav-item">
              <a
                className="nav-link"
                href="/Admin/Login"
                target="_blank"
                rel="noopener noreferrer"
                style={{
                  color: "#a5b4fc",
                  fontSize: "1rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
                onClick={() => setIsMenuOpen(false)}
              >
                Login
              </a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
