import { Watermark } from "antd";
import React from "react";
import Flexbox from "../../components/flexbox";
import Footer from "../footer";
import Header from "../header";
import Sider from "../sider";
import Toolbar from "../toolbar";

interface LayoutProps {
  children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Watermark content="Furion">
      <Flexbox $fullscreen>
        <Sider />
        <Flexbox direction="column" $flex={1}>
          <Header />
          <Flexbox $flex={1}>
            {children}
            <Toolbar />
          </Flexbox>
          <Footer />
        </Flexbox>
      </Flexbox>
    </Watermark>
  );
};

export default Layout;
