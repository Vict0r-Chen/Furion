import { Divider, Layout, Watermark } from "antd";

const { Sider, Content, Header, Footer } = Layout;

const headerStyle: React.CSSProperties = {
  height: 35,
  backgroundColor: "#ffffff",
};

const contentStyle: React.CSSProperties = {
  backgroundColor: "#ffffff",
};

const siderStyle: React.CSSProperties = {
  backgroundColor: "rgb(247, 248, 251)",
};

const footerStyle: React.CSSProperties = {
  height: 30,
  backgroundColor: "#ffffff",
};

function App() {
  return (
    <Watermark content={[]}>
      <Layout
        style={{ height: "100vh", width: "100vw", backgroundColor: "#ffffff" }}
      >
        <Sider style={siderStyle} width={60}></Sider>
        <Divider type="vertical" style={{ margin: 0, height: "100vh" }} />
        <Layout>
          <Header style={headerStyle}></Header>
          <Divider style={{ margin: 0 }} />
          <Content style={contentStyle}></Content>
          <Divider style={{ margin: 0 }} />
          <Footer style={footerStyle}></Footer>
        </Layout>
      </Layout>
    </Watermark>
  );
}

export default App;
