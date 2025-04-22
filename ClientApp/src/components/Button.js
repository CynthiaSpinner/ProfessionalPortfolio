import React from "react";
import { Button as BootstrapButton } from "react-bootstrap";
import "./Button.css";

const Button = ({ children, href, className = "", ...props }) => {
  return (
    <BootstrapButton
      as={href ? "a" : "button"}
      href={href}
      className={`custom-button ${className}`}
      {...props}
    >
      {children}
    </BootstrapButton>
  );
};

export default Button;
