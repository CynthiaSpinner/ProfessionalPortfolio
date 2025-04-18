import React from "react";

function Home() {
  return (
    <div className="container mt-5">
      <h1>Welcome to My Portfolio</h1>
      <p>This is a simple portfolio website built with React and .NET.</p>
      <div className="row mt-4">
        <div className="col-md-6">
          <h2>About Me</h2>
          <p>I am a passionate developer with experience in web development.</p>
        </div>
        <div className="col-md-6">
          <h2>My Skills</h2>
          <ul>
            <li>React</li>
            <li>.NET</li>
            <li>MySQL</li>
            <li>JavaScript</li>
          </ul>
        </div>
      </div>
    </div>
  );
}

export default Home;
