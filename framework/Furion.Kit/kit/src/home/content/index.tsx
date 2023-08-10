import React from "react";
import { Outlet } from "react-router-dom";
import { styled } from "styled-components";

const Container = styled.div`
  flex: 1;
`;

const Content: React.FC = () => {
  return (
    <Container>
      <Outlet />
    </Container>
  );
};

export default Content;
