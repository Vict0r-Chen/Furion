import { FloatButton } from "antd";
import React from "react";
import { Outlet } from "react-router-dom";
import styled from "styled-components";
import ContentMain from "./components/content-main";
import ContentMenu from "./components/content-menu";
import ContentTitle from "./components/content-title";

const Container = styled.div`
  flex: 1;
  overflow-y: auto;
`;

const contentId = "scroll-content";

const Content: React.FC & {
  Title: typeof ContentTitle;
  Menu: typeof ContentMenu;
  Main: typeof ContentMain;
} = () => {
  return (
    <Container id={contentId}>
      <Outlet />
      <FloatButton.BackTop target={() => document.getElementById(contentId)!} />
    </Container>
  );
};

Content.Title = ContentTitle;
Content.Menu = ContentMenu;
Content.Main = ContentMain;

export default Content;
