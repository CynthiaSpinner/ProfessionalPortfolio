import React from "react";
import { PortfolioProvider } from "./context/PortfolioContext";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import "bootstrap/dist/css/bootstrap.min.css";

import "./styles/App.css";

function App() {
  return (
    <PortfolioProvider>
      <div className="App">
        <Navbar />
        <main>
          <Home />
        </main>
      </div>
    </PortfolioProvider>
  );
}

export default App;
