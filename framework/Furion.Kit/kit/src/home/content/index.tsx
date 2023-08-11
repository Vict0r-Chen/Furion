import React from "react";
import { Outlet } from "react-router-dom";
import { styled } from "styled-components";
import Flexbox from "../../components/flexbox";
import ContentMain from "./components/content-main";
import ContentMenu from "./components/content-menu";
import ContentTitle from "./components/content-title";

const Container = styled(Flexbox)`
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
  Main: typeof ContentMain;
};

const Content = ContentDefault as ContentComponent;
Content.Title = ContentTitle;
Content.Menu = ContentMenu;
Content.Main = ContentMain;

export default Content;
