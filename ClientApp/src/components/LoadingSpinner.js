import React from 'react';
import { Spinner } from 'react-bootstrap';
import '../styles/LoadingSpinner.css';

const LoadingSpinner = ({ 
  message = "Loading Portfolio", 
  subtitle = "Preparing your experience..." 
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
