import React from "react";
import { Container, Row, Col, Progress } from "reactstrap";
import HeadingGroup from "./HeadingGroup";
import Card from "./Card";
import "./SkillsOverview.css";

function SkillsOverview({ skills }) {
  return (
    <section className="skills-overview">
      <Container className="skills-container">
        <Card className="skills-card">
          <HeadingGroup
            title="Skills & Expertise"
            subtitle="A comprehensive overview of my technical abilities and professional experience"
          />
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
        </Card>
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
