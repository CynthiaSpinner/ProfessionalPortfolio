import React from "react";

const HeadingGroup = ({ title, subtitle, className = "", headingLevel = "h2" }) => {
  const HeadingTag = headingLevel;
  
  return (
    <div className={`heading-group ${className}`}>
      <HeadingTag style={{ color: "#818cf8" }}>{title}</HeadingTag>
      {subtitle && <p className="text-secondary">{subtitle}</p>}
    </div>
  );
};

export default HeadingGroup;
