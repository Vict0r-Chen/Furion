import React from "react";
import { Outlet } from "react-router-dom";
import { styled } from "styled-components";
import ContentTitle from "./components/content-title";

const Container = styled.div`
  flex: 1;
  overflow-y: auto;
`;

const ContentDefault: React.FC = () => {
  return (
    <Container>
      <Outlet />
    </Container>
  );
};

type ContentComponent = typeof ContentDefault & {
  Title: typeof ContentTitle;
};

const Content = ContentDefault as ContentComponent;
Content.Title = ContentTitle;

export default Content;
