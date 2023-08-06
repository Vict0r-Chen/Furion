import { Divider, Image, Space } from "antd";
import logo from "../../assets/logo.png";
import Function from "../sider/function";
import { Container, Explore, Line, Logo, Main, Menus } from "./style";

export default function SiderPanel() {
  return (
    <Container width={60}>
      <Main>
        <Menus>
          <Logo>
            <Image src={logo} width={36} preview={false} />
          </Logo>
          <Line>
            <Divider />
          </Line>
          <Space direction="vertical" align="center" size={20}>
            <Function type="icon-panel" active title="面板" />
            <Function type="icon-component" title="组件" />
            <Function
              type="icon-exception"
              title="异常"
              badge={5}
              badgeStatus="error"
            />
            <Function type="icon-openapi" title="开放" />
          </Space>
          <Line>
            <Divider style={{ margin: "20px 0" }} />
          </Line>
          <Space direction="vertical" align="center" size={20}>
            <Function type="icon-starter" title="启动" />
            <Function type="icon-code-generate" title="代码" />
            <Function type="icon-system-info" title="系统" />
          </Space>
          <Line>
            <Divider style={{ margin: "20px 0" }} />
          </Line>
          <Space direction="vertical" align="center" size={20}>
            <Function
              type="icon-explore"
              title="探索"
              style={{ color: "#52c41a" }}
            />
          </Space>
        </Menus>
        <Explore>
          <Space direction="vertical" align="center" size={20}>
            <Function type="icon-setting" title="设置" />
          </Space>
        </Explore>
      </Main>
    </Container>
  );
}
