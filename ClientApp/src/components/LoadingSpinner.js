import React from 'react';
import { Spinner } from 'react-bootstrap';
import '../styles/LoadingSpinner.css';

const LoadingSpinner = ({ message = "Loading..." }) => {
  return (
    <div className="loading-container">
      <div className="loading-content">
        <Spinner animation="border" variant="primary" />
        <p className="loading-message">{message}</p>
      </div>
    </div>
  );
};

export default LoadingSpinner;
