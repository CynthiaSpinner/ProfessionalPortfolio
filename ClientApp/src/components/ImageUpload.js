import React, { useState } from 'react';
import { portfolioApi } from '../services/api';

const ImageUpload = ({ onUploadSuccess, onUploadError }) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [preview, setPreview] = useState(null);

  const handleFileSelect = (event) => {
    const file = event.target.files[0];
    if (file) {
      // Validate file type
      const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
      if (!allowedTypes.includes(file.type)) {
        alert('Please select a valid image file (JPEG, PNG, GIF, or WebP)');
        return;
      }

      // Validate file size (10MB max)
      const maxSize = 10 * 1024 * 1024; // 10MB
      if (file.size > maxSize) {
        alert('File size must be less than 10MB');
        return;
      }

      setSelectedFile(file);
      
      // Create preview
      const reader = new FileReader();
      reader.onload = (e) => setPreview(e.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleUpload = async () => {
    if (!selectedFile) return;

    setUploading(true);
    try {
      const formData = new FormData();
      formData.append('image', selectedFile);

      const response = await portfolioApi.uploadHeroImage(formData);
      
      if (response.data.success) {
        onUploadSuccess?.(response.data.imageUrl);
        setSelectedFile(null);
        setPreview(null);
        // Reset file input
        const fileInput = document.getElementById('image-upload');
        if (fileInput) fileInput.value = '';
      } else {
        throw new Error(response.data.error || 'Upload failed');
      }
    } catch (error) {
      console.error('Upload error:', error);
      onUploadError?.(error.response?.data?.error || error.message || 'Upload failed');
    } finally {
      setUploading(false);
    }
  };

  const handleCancel = () => {
    setSelectedFile(null);
    setPreview(null);
    const fileInput = document.getElementById('image-upload');
    if (fileInput) fileInput.value = '';
  };

  return (
    <div className="image-upload-container">
      <div className="upload-section">
        <input
          id="image-upload"
          type="file"
          accept="image/*"
          onChange={handleFileSelect}
          style={{ display: 'none' }}
        />
        <label htmlFor="image-upload" className="upload-button">
          Choose Image
        </label>
        
        {selectedFile && (
          <div className="file-info">
            <p>Selected: {selectedFile.name}</p>
            <p>Size: {(selectedFile.size / 1024 / 1024).toFixed(2)} MB</p>
          </div>
        )}
      </div>

      {preview && (
        <div className="preview-section">
          <h4>Preview:</h4>
          <img 
            src={preview} 
            alt="Preview" 
            className="preview-image"
            style={{ maxWidth: '300px', maxHeight: '200px', objectFit: 'cover' }}
          />
        </div>
      )}

      {selectedFile && (
        <div className="upload-actions">
          <button 
            onClick={handleUpload} 
            disabled={uploading}
            className="upload-confirm-btn"
          >
            {uploading ? 'Uploading...' : 'Upload Image'}
          </button>
          <button 
            onClick={handleCancel}
            disabled={uploading}
            className="upload-cancel-btn"
          >
            Cancel
          </button>
        </div>
      )}
    </div>
  );
};

export default ImageUpload;
