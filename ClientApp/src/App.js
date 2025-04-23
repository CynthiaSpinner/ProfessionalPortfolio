import React from "react";
import { Routes, Route } from "react-router-dom";
import { PortfolioProvider } from "./context/PortfolioContext";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import About from "./pages/About";
import Projects from "./pages/Projects";
import Contact from "./pages/Contact";
import GraphicDesign from "./pages/GraphicDesign";
import Design from "./pages/Design";
import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "./styles/App.css";

function App() {
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
