import { Divider, Image, Space } from "antd";
import logo from "../../assets/logo.png";
import Function from "../sider/function";
import { Container, Line, Logo } from "./style";

export default function SiderPanel() {
  return (
    <Container width={60}>
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
        <Function type="icon-starter" title="启动" />
        <Function type="icon-code-generate" title="生成" />
      </Space>
      <Line>
        <Divider style={{ margin: "20px 0" }} />
      </Line>
      <Space direction="vertical" align="center" size={20}>
        <Function type="icon-system-info" title="系统" />
        <Function type="icon-setting" title="设置" />
      </Space>
    </Container>
  );
}
