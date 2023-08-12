import React from "react";
import { Outlet } from "react-router-dom";
import { styled } from "styled-components";
import ContentMain from "./components/content-main";
import ContentMenu from "./components/content-menu";
import ContentTitle from "./components/content-title";

const Container = styled.div`
  flex: 1;
  overflow-y: auto;
`;

const Content: React.FC & {
  Title: typeof ContentTitle;
  Menu: typeof ContentMenu;
  Main: typeof ContentMain;
} = () => {
  return (
    <Container id="scroll-content">
      <Outlet />
    </Container>
  );
};

Content.Title = ContentTitle;
Content.Menu = ContentMenu;
Content.Main = ContentMain;

export default Content;
