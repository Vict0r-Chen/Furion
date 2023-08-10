import { Watermark } from "antd";
import React from "react";
import { styled } from "styled-components";
import Footer from "../footer";
import Header from "../header";
import Sider from "../sider";
import Toolbar from "../toolbar";

const Container = styled.div`
  display: flex;
  flex-direction: row;
  width: 100vw;
  height: 100vh;
`;

const MainLayout = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
`;

const ContentLayout = styled.div`
  display: flex;
  flex: 1;
  flex-direction: row;
`;

interface LayoutProps {
  children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Watermark content="Furion">
      <Container>
        <Sider />
        <MainLayout>
          <Header />
          <ContentLayout>
            {children}
            <Toolbar />
          </ContentLayout>
          <Footer />
        </MainLayout>
      </Container>
    </Watermark>
  );
};

export default Layout;
