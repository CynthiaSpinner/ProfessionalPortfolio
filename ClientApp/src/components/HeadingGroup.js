import React from "react";

const HeadingGroup = ({ title, subtitle, className = "" }) => {
  return (
    <div className={`heading-group ${className}`}>
      <h2 style={{ color: "#818cf8" }}>{title}</h2>
      {subtitle && <p className="heading-group__subtitle">{subtitle}</p>}
    </div>
  );
};

export default HeadingGroup;
