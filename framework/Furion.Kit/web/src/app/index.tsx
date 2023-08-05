import { Divider, Layout, Watermark } from "antd";
import ContentPanel from "./content";
import FooterPanel from "./footer";
import HeaderPanel from "./header";
import SiderPanel from "./sider";
import { LayoutPanel } from "./style";

function App() {
  return (
    <Watermark content={[]}>
      <LayoutPanel>
        <SiderPanel />
        <Divider type="vertical" style={{ margin: 0, height: "100vh" }} />
        <Layout>
          <HeaderPanel />
          <Divider style={{ margin: 0 }} />
          <ContentPanel />
          <Divider style={{ margin: 0 }} />
          <FooterPanel />
        </Layout>
      </LayoutPanel>
    </Watermark>
  );
}

export default App;
