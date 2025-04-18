import React from "react";
import { Container, Row, Col, Progress } from "reactstrap";
import "./SkillsOverview.css";

function SkillsOverview({ skills }) {
  return (
    <section className="skills-overview py-5">
      <Container>
        <h2 className="text-center mb-5">Skills & Expertise</h2>
        <Row>
          {skills?.categories?.map((category) => (
            <Col md="6" key={category.name} className="mb-4">
              <h3 className="mb-3">{category.name}</h3>
              {category.skills.map((skill) => (
                <div key={skill.name} className="mb-3">
                  <div className="d-flex justify-content-between mb-1">
                    <span>{skill.name}</span>
                    <span>{skill.level}%</span>
                  </div>
                  <Progress
                    value={skill.level}
                    color={getProgressColor(skill.level)}
                  />
                </div>
              ))}
            </Col>
          ))}
        </Row>
      </Container>
    </section>
  );
}

function getProgressColor(level) {
  if (level >= 80) return "success";
  if (level >= 60) return "info";
  if (level >= 40) return "warning";
  return "danger";
}

export default SkillsOverview;
