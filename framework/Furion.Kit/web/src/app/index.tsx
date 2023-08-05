import { Divider, Image, Layout, Space, Watermark } from "antd";
import { styled } from "styled-components";
import logo from "../assets/logo.png";
import Framework from "./header/framework";
import Function from "./sider/function";

const { Sider, Content, Header, Footer } = Layout;

const headerStyle: React.CSSProperties = {
  height: 35,
  backgroundColor: "#ffffff",
  lineHeight: "22px",
  padding: 0,
  display: "flex",
  alignItems: "center",
  justifyContent: "flex-end",
};

const contentStyle: React.CSSProperties = {
  backgroundColor: "#ffffff",
};

const siderStyle: React.CSSProperties = {
  backgroundColor: "rgb(247, 248, 251)",
  display: "flex",
  flexDirection: "column",
  textAlign: "center",
};

const footerStyle: React.CSSProperties = {
  height: 30,
  backgroundColor: "#ffffff",
};

const Logo = styled.div`
  margin: 15px 0 0 0;
`;

const Line = styled.div`
  padding: 0 15px;
`;

function App() {
  return (
    <Watermark content={[]}>
      <Layout
        style={{
          height: "100vh",
          width: "100vw",
          backgroundColor: "#ffffff",
          lineHeight: "nomarl",
        }}
      >
        <Sider style={siderStyle} width={60}>
          <Logo>
            <Image src={logo} width={36} />
          </Logo>
          <Line>
            <Divider />
          </Line>
          <Space direction="vertical" align="center" size={20}>
            <Function type="icon-panel" active title="面板" />
            <Function type="icon-component" title="组件" />
            <Function type="icon-exception" title="异常" />
            <Function type="icon-openapi" title="开放" />
          </Space>
          <Line>
            <Divider style={{ margin: "20px 0" }} />
          </Line>
          <Space direction="vertical" align="center" size={20}>
            <Function type="icon-setting" title="设置" />
          </Space>
        </Sider>
        <Divider type="vertical" style={{ margin: 0, height: "100vh" }} />
        <Layout>
          <Header style={headerStyle}>
            <Framework />
          </Header>
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
