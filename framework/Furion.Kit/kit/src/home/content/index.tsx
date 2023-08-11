import React from "react";
import { Outlet } from "react-router-dom";
import { styled } from "styled-components";
import ContentMenu from "./components/content-menu";
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
  Menu: typeof ContentMenu;
};

const Content = ContentDefault as ContentComponent;
Content.Title = ContentTitle;
Content.Menu = ContentMenu;

export default Content;
