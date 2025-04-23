import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import HeadingGroup from "./HeadingGroup";
import Button from "./Button";
import "../styles/Header.css";

const Header = ({
  title,
  subtitle,
  background = "linear-gradient(135deg, rgba(75, 75, 90, 0.3), rgba(85, 85, 105, 0.2), rgba(227, 235, 255, 0.1))",
  children,
  showButtons = false
}) => {
  return (
    <header className="page-header" style={{ background }}>
      <Container>
        <Row className="justify-content-center">
          <Col lg={8} className="text-center">
            <HeadingGroup title={title} subtitle={subtitle} />
            {showButtons && (
              <div className="mt-4">
                <Button href="/projects">View Projects</Button>
              </div>
            )}
            {children}
          </Col>
        </Row>
      </Container>
    </header>
  );
};

export default Header;
