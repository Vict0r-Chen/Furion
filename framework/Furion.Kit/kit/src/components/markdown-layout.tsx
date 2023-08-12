import React from "react";
import { styled } from "styled-components";
import { CSS } from "styled-components/dist/types";

const Container = styled.div`
  box-sizing: border-box;
`;

const Layout: React.FC<{
  children?: React.ReactNode;
  style?: CSS.Properties;
}> = ({ children, style }) => {
  return (
    <Container className="markdown-body" style={style}>
      {children}
    </Container>
  );
};

export default Layout;
