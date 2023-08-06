import { Divider, Layout, Watermark } from "antd";
import ContentPanel from "./content";
import FooterPanel from "./footer";
import HeaderPanel from "./header";
import SiderPanel from "./sider";
import { LayoutPanel } from "./style";
import Toolbar from "./toolbar";

function App() {
  return (
    <Watermark content={["Furion"]}>
      <LayoutPanel>
        <SiderPanel />
        <Divider type="vertical" style={{ margin: 0, height: "100vh" }} />
        <Layout>
          <HeaderPanel />
          <Divider style={{ margin: 0 }} />
          <Layout style={{ backgroundColor: "#ffffff" }}>
            <ContentPanel />
            <Divider type="vertical" style={{ margin: 0, height: "100%" }} />
            <Toolbar />
          </Layout>
          <Divider style={{ margin: 0 }} />
          <FooterPanel />
        </Layout>
      </LayoutPanel>
    </Watermark>
  );
}

export default App;
