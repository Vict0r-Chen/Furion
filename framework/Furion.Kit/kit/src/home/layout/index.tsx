import { Watermark } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../components/flexbox";
import Footer from "../footer";
import Header from "../header";
import Sider from "../sider";
import Toolbar from "../toolbar";

const Container = styled(Flexbox)`
  overflow: hidden;
`;

const ContentContainer = styled(Flexbox)`
  overflow: hidden;
`;

const Main = styled(Flexbox)`
  overflow: hidden;
`;

interface LayoutProps {
  children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Watermark content="Furion">
      <Container $fullscreen>
        <Sider />
        <Main direction="column" $flex={1}>
          <Header />
          <ContentContainer $flex={1}>
            {children}
            <Toolbar />
          </ContentContainer>
          <Footer />
        </Main>
      </Container>
    </Watermark>
  );
};

export default Layout;
