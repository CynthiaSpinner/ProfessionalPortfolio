import React, { useState, useEffect } from "react";
import { Container } from "react-bootstrap";
import SkillReveal from "./SkillReveal";
import api from "../services/api";
import "./SkillsTeaser.css";

const SKILLS_PER_COLUMN = 4;

function SkillsTeaser({ seeMoreHref = "/about#skills" }) {
  const [categories, setCategories] = useState([]);
  const [linkText, setLinkText] = useState("View what I spin?");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    api
      .get("/portfolio/skills")
      .then((res) => {
        if (cancelled) return;
        const data = res.data;
        const raw = data?.categories ?? (Array.isArray(data) ? data : []);
        const list = Array.isArray(raw)
          ? raw.map((c) => ({
              id: c.id,
              title: c.title || c.name,
              skills: Array.isArray(c.skills) ? c.skills : (c.skillsJson ? JSON.parse(c.skillsJson || "[]") : []),
            }))
          : [];
        setCategories(list);
        if (data?.linkText != null && data.linkText !== "") setLinkText(data.linkText);
      })
      .catch(() => {
        if (!cancelled) setError(true);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => { cancelled = true; };
  }, []);

  if (loading) {
    return (
      <section className="skills-teaser py-5">
        <Container>
          <div className="skills-teaser-loading text-center">Loading…</div>
        </Container>
      </section>
    );
  }

  if (error || !categories.length) {
    return (
      <section className="skills-teaser py-5">
        <Container>
          <div className="text-center">
            <a href={seeMoreHref} className="skills-teaser-see-more">{linkText}</a>
          </div>
        </Container>
      </section>
    );
  }

  return (
    <section className="skills-teaser py-5">
      <Container>
        <div className="skills-teaser-row">
          {categories.map((cat) => (
            <div key={cat.id} className="skills-teaser-column">
              <h3 className="skills-teaser-column-title">{cat.title}</h3>
              <ul className="skills-teaser-list">
                {(cat.skills || []).slice(0, SKILLS_PER_COLUMN).map((skill, i) => (
                  <li key={i}>
                    <SkillReveal text={String(skill).trim()} delayMs={i * 60} />
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
        <div className="skills-teaser-see-more-wrap text-center mt-4">
          <a href={seeMoreHref} className="skills-teaser-see-more">{linkText}</a>
        </div>
      </Container>
    </section>
  );
}

export default SkillsTeaser;
