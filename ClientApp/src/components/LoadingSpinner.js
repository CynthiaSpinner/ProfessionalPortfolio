import React from 'react';
import '../styles/LoadingSpinner.css';

const LoadingSpinner = ({ 
  message = "Weaving Digital Experiences", 
  subtitle = "Spinning up your web..." 
}) => {
  return (
    <div className="loading-container">
      <div className="loading-content">
        <div className="loading-spinner"></div>
        <p className="loading-message">{message}</p>
        <p className="loading-subtitle">{subtitle}</p>
      </div>
    </div>
  );
};

export default LoadingSpinner;
