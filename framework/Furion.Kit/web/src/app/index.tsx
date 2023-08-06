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
          <Layout>
            <ContentPanel />
            <Divider type="vertical" style={{ margin: 0, height: "100%" }} />
            <Toolbar />
          </Layout>
          <Divider style={{ margin: 0 }} />
          <FooterPanel />
        </Layout>
      </LayoutPanel>
      {/* <FloatButton.Group
        trigger="hover"
        type="primary"
        style={{ right: 34 }}
        icon={<QuestionCircleOutlined />}
      >
        <Tooltip placement="right" title="使用文档">
          <FloatButton href="https://furion.net" target="_blank" />
        </Tooltip>
        <Tooltip placement="right" title="商务合作">
          <FloatButton icon={<CommentOutlined />} />
        </Tooltip>
      </FloatButton.Group> */}
    </Watermark>
  );
}

export default App;
