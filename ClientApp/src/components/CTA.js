import React from "react";
import { Container } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import Button from "./Button";
import "../styles/CTA.css";

const CTA = ({ title, subtitle, buttonText = "Get in Touch", buttonHref = "/contact" }) => {
  return (
    <section className="cta-section py-5">
      <Container className="text-center">
        <HeadingGroup
          title={title}
          subtitle={subtitle}
          className="mb-4"
        />
        <Button href={buttonHref}>{buttonText}</Button>
      </Container>
    </section>
  );
};

export default CTA; 