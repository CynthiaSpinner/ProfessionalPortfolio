import React from "react";
import { Routes, Route } from "react-router-dom";
// Frontend deployment test - remove startup command
import { PortfolioProvider, usePortfolio } from "./context/PortfolioContext";
import Navbar from "./components/Navbar";
import LoadingSpinner from "./components/LoadingSpinner";
import Home from "./pages/Home";
import About from "./pages/About";
import Projects from "./pages/Projects";
import Contact from "./pages/Contact";
import GraphicDesign from "./pages/GraphicDesign";
import Design from "./pages/Design";
import "bootstrap/dist/css/bootstrap.min.css";

import "./styles/App.css";

function AppContent() {
  const { loading } = usePortfolio();

  if (loading) {
    return <LoadingSpinner message="Loading portfolio..." />;
  }

  return (
    <div className="App">
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/projects" element={<Projects />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/graphic-design" element={<GraphicDesign />} />
          <Route path="/design" element={<Design />} />
        </Routes>
      </main>
    </div>
  );
}

function App() {
  return (
    <PortfolioProvider>
      <AppContent />
    </PortfolioProvider>
  );
}

export default App;
