import React, { useState, useEffect, useRef } from "react";

const JUMBLE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789<>/\\";
const CHAR_REVEAL_INTERVAL_MS = 40;

function randomChar() {
  return JUMBLE_CHARS[Math.floor(Math.random() * JUMBLE_CHARS.length)];
}

function SkillReveal({ text, delayMs = 0, className = "" }) {
  const [display, setDisplay] = useState("");
  const [done, setDone] = useState(false);
  const indexRef = useRef(0);

  useEffect(() => {
    if (!text) return;
    const len = text.length;
    indexRef.current = 0;
    setDisplay(text.split("").map(() => randomChar()).join(""));
    setDone(false);

    let intervalId = null;
    const startReveal = () => {
      intervalId = setInterval(() => {
        const i = indexRef.current;
        if (i >= len) {
          clearInterval(intervalId);
          intervalId = null;
          setDisplay(text);
          setDone(true);
          return;
        }
        const revealed = text.slice(0, i + 1);
        const jumbled = text.slice(i + 1).split("").map(() => randomChar()).join("");
        setDisplay(revealed + jumbled);
        indexRef.current = i + 1;
      }, CHAR_REVEAL_INTERVAL_MS);
    };

    const timeoutId = setTimeout(startReveal, delayMs);
    return () => {
      clearTimeout(timeoutId);
      if (intervalId) clearInterval(intervalId);
    };
  }, [text, delayMs]);

  return (
    <span className={`skill-reveal ${done ? "skill-reveal--done" : ""} ${className}`.trim()}>
      {display || "\u00A0"}
    </span>
  );
}

export default SkillReveal;
