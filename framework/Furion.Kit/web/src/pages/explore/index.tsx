import {
  Anchor,
  Button,
  Divider,
  Input,
  Layout,
  Popover,
  Rate,
  Space,
  Typography,
} from "antd";
import { AnchorContainer } from "antd/es/anchor/Anchor";
import React from "react";
import logo from "../../assets/logo.png";
import IconFont from "../../components/iconfont";
import {
  CardDescription,
  CardIcon,
  CardItem,
  CardMain,
  CardPanel,
  CardTitle,
  CardViewIcon,
  Container,
  Panel,
} from "./style";

const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Explore() {
  return (
    <Panel>
      <Title level={3}>探索</Title>
      <Container id="explore-container">
        <Sider
          theme="light"
          style={{ position: "sticky", overflowY: "auto", top: 0 }}
        >
          <div style={{ marginTop: 20 }}>
            <Anchor
              onClick={(ev) => ev.preventDefault()}
              getContainer={() =>
                document.getElementById("explore-container") as AnchorContainer
              }
              affix={false}
              items={[
                {
                  key: "application",
                  href: "#explore-application",
                  title: (
                    <Space>
                      <IconFont type="icon-local" />
                      <Text>本地应用</Text>
                    </Space>
                  ),
                },
                {
                  key: "tools",
                  href: "#explore-tools",
                  title: (
                    <Space>
                      <IconFont type="icon-internet" />
                      <Text>联网应用</Text>
                    </Space>
                  ),
                },
              ]}
            />
          </div>
        </Sider>
        <Content style={{ marginLeft: 15 }}>
          <div
            style={{
              padding: "0 0 25px 0",
            }}
          >
            <Input
              style={{ width: 180 }}
              placeholder="ERP WMS OA"
              suffix={<IconFont type="icon-search" />}
            />
          </div>
          <Space>
            <IconFont type="icon-tag" style={{ fontSize: 18 }} />
            <Title level={5} style={{ marginTop: 0 }} id="explore-application">
              本地应用
            </Title>
          </Space>
          <CardPanel>
            <Application
              title="看板"
              icon={
                <IconFont
                  type="icon-panel"
                  style={{ fontSize: "36px", color: "#389e0d" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={5}
              install={true}
            />

            <Application
              title="输出"
              icon={
                <IconFont
                  type="icon-console"
                  style={{ fontSize: "36px", color: "#389e0d" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
              install={true}
            />

            <Application
              title="诊断"
              icon={
                <IconFont
                  type="icon-diagnosis"
                  style={{ fontSize: "36px", color: "#389e0d" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
              install={true}
            />

            <Application
              title="开放"
              icon={
                <IconFont
                  type="icon-openapi"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />

            <Application
              title="系统"
              icon={
                <IconFont
                  type="icon-system-info"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />

            <Application
              title="组件"
              icon={
                <IconFont
                  type="icon-component"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />

            <Application
              title="配置"
              icon={
                <IconFont
                  type="icon-configuration"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />

            <Application
              title="启动"
              icon={
                <IconFont
                  type="icon-starter"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />

            <Application
              title="代码"
              icon={
                <IconFont
                  type="icon-code-generate"
                  style={{ fontSize: "36px", color: "#1677ff" }}
                />
              }
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={4.5}
            />
          </CardPanel>
          <Divider />

          <Space>
            <IconFont type="icon-tag" style={{ fontSize: 18 }} />
            <Title level={5} style={{ marginTop: 0 }} id="explore-tools">
              联网应用
            </Title>
          </Space>
          <CardPanel>
            <Application
              title="Furion"
              icon={<img src={logo} alt="" height={40} />}
              version="v5.0.0"
              description="一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。"
              rate={5}
            />
          </CardPanel>
          <Divider />
        </Content>
      </Container>
    </Panel>
  );
}

interface ApplicationProps {
  title: string;
  rate: number;
  description?: string;
  icon: React.ReactNode;
  version?: string;
  install?: boolean;
}

const Application: React.FC<ApplicationProps> = ({
  title,
  rate,
  description,
  icon,
  version,
  install = false,
}) => {
  return (
    <Popover
      content={
        <Space>
          <Button type="primary" disabled={install}>
            安装
          </Button>
          <Button type="primary" danger disabled={!install}>
            卸载
          </Button>
        </Space>
      }
      title={
        <Space direction="vertical">
          <Space size={5}>
            <Text>版本：</Text>
            <Text style={{ color: "#595959" }}>{version}</Text>
          </Space>
          <Space size={5}>
            <Text>推荐指数</Text>
            <IconFont type="icon-info" style={{ color: "#8c8c8c" }} />
          </Space>
          <Rate allowHalf defaultValue={rate} disabled />
        </Space>
      }
      placement="bottom"
    >
      <CardItem $install={install}>
        <CardIcon>{icon}</CardIcon>
        <CardMain>
          <CardTitle>{title}</CardTitle>
          <CardDescription>{description}</CardDescription>
        </CardMain>
        <CardViewIcon>
          <IconFont type="icon-view-arrow-right" style={{ color: "#8c8c8c" }} />
        </CardViewIcon>
      </CardItem>
    </Popover>
  );
};
