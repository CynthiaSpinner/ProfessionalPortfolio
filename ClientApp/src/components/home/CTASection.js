import React from "react";
import CTA from "../CTA";

const CTASection = ({ ctaData }) => {
  // Use provided data - handle undefined gracefully
  const data = ctaData || {};

  return (
    <CTA
      title={data.title || "Ready to Start a Project?"}
      subtitle={data.subtitle || "Let's work together to bring your ideas to life."}
      buttonText={data.buttonText || "Get in Touch"}
      buttonHref={data.buttonUrl || "/contact"}
    />
  );
};

export default CTASection; 