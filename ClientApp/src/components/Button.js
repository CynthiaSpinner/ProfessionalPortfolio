import React from "react";
import { Button as BootstrapButton } from "react-bootstrap";
import { Link } from "react-router-dom";
import "../styles/Button.css";

const Button = ({ children, href, className = "", ...props }) => {
  if (href) {
    return (
      <Link to={href} className={`custom-button ${className}`} {...props}>
        {children}
      </Link>
    );
  }

  return (
    <BootstrapButton
      className={`custom-button ${className}`}
      {...props}
    >
      {children}
    </BootstrapButton>
  );
};

export default Button;
