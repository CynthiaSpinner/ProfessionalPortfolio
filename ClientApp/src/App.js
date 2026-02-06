import React, { useEffect } from "react";
import { Routes, Route } from "react-router-dom";
// Frontend deployment test - remove startup command
import { PortfolioProvider } from "./context/PortfolioContext";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import About from "./pages/About";
import Projects from "./pages/Projects";
import Contact from "./pages/Contact";
import GraphicDesign from "./pages/GraphicDesign";
import Design from "./pages/Design";
import "bootstrap/dist/css/bootstrap.min.css";

import "./styles/App.css";

const MAIN_FAVICON = "https://professionalportfolio-9a6n.onrender.com/favicon.svg";
const MAIN_TITLE = "CodeSpinner & Design";

function App() {
  useEffect(() => {
    document.title = MAIN_TITLE;
    const favicon = document.getElementById("favicon") || document.querySelector('link[rel="icon"]');
    if (favicon && favicon.href !== MAIN_FAVICON) {
      favicon.href = MAIN_FAVICON;
    }
    const shortcuts = document.querySelectorAll('link[rel="shortcut icon"], link[rel="apple-touch-icon"]');
    shortcuts.forEach((link) => { link.href = MAIN_FAVICON; });
  }, []);

  return (
    <PortfolioProvider>
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
    </PortfolioProvider>
  );
}

export default App;
