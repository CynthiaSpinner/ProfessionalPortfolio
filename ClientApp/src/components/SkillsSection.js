import React, { useState, useEffect } from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import SkillReveal from "./SkillReveal";
import api from "../services/api";
import "./SkillsSection.css";

function SkillsSection({ title = "My Skills", subtitle = "A comprehensive overview of my technical expertise." }) {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    api
      .get("/portfolio/skills")
      .then((res) => {
        if (cancelled) return;
        const data = res.data;
        const list = Array.isArray(data)
          ? data.map((c) => ({
              id: c.id,
              title: c.title || c.name,
              description: c.description || "",
              skills: Array.isArray(c.skills) ? c.skills : (c.skillsJson ? JSON.parse(c.skillsJson || "[]") : []),
            }))
          : [];
        setCategories(list);
      })
      .catch((err) => {
        if (!cancelled) setError(err.message || "Failed to load skills");
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => { cancelled = true; };
  }, []);

  if (loading) {
    return (
      <section className="skills-section py-5">
        <Container>
          <HeadingGroup title={title} subtitle={subtitle} className="text-center mb-5" />
          <div className="skills-section-loading text-center text-muted">Loading skills…</div>
        </Container>
      </section>
    );
  }

  if (error) {
    return (
      <section className="skills-section py-5">
        <Container>
          <HeadingGroup title={title} subtitle={subtitle} className="text-center mb-5" />
          <div className="skills-section-error text-center text-muted">{error}</div>
        </Container>
      </section>
    );
  }

  if (!categories.length) {
    return (
      <section className="skills-section py-5">
        <Container>
          <HeadingGroup title={title} subtitle={subtitle} className="text-center mb-5" />
          <div className="skills-section-empty text-center text-muted">No skills categories yet.</div>
        </Container>
      </section>
    );
  }

  return (
    <section className="skills-section py-5">
      <Container>
        <HeadingGroup title={title} subtitle={subtitle} className="text-center mb-5" />
        <Row className="g-4 skills-categories-row">
          {categories.map((cat) => (
            <Col key={cat.id} xs={12} md={6} lg={4} xl={Math.min(12 / categories.length, 12)}>
              <div className="skills-column">
                <h3 className="skills-category-title">{cat.title}</h3>
                {cat.description && <p className="skills-category-desc">{cat.description}</p>}
                <ul className="skills-list">
                  {cat.skills.map((skill, i) => (
                    <li key={i}>
                      <SkillReveal text={String(skill).trim()} delayMs={i * 80} />
                    </li>
                  ))}
                </ul>
              </div>
            </Col>
          ))}
        </Row>
      </Container>
    </section>
  );
}

export default SkillsSection;
