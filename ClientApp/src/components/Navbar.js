import React from "react";
import { Link } from "react-router-dom";
import "./Navbar.css";

const Navbar = () => {
  return (
    <nav
      className="navbar navbar-expand-lg navbar-dark bg-dark"
      style={{ minHeight: "60px" }}
    >
      <div className="container">
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
          }}
        >
          <span>C</span>
          <span className="gradient-text">ynthia </span>
          <span>S</span>
          <span className="gradient-text">pinner</span>
        </h1>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul
            className="navbar-nav"
            style={{ gap: "0.75rem", marginTop: "0.15rem" }}
          >
            <li className="nav-item">
              <Link
                className="nav-link"
                to="/"
                style={{
                  color: "#a5b4fc",
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
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
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
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
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
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
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
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
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
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
                  fontSize: "0.85rem",
                  padding: "0.5rem 0.75rem",
                  lineHeight: "1.2",
                }}
              >
                Contact
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
