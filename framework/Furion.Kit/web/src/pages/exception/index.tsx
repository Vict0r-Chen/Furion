import {
  Anchor,
  Button,
  Divider,
  Input,
  Layout,
  Radio,
  Select,
  Space,
  Typography,
} from "antd";
import { AnchorContainer } from "antd/es/anchor/Anchor";
import IconFont from "../../components/iconfont";
import { Container, Panel } from "./style";

const { TextArea } = Input;
const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Exception() {
  return (
    <Panel>
      <Title level={3}>异常</Title>
      <Container id="exception-container">
        <Sider
          theme="light"
          style={{ position: "sticky", overflowY: "auto", top: 0 }}
        >
          <div style={{ marginTop: 20 }}>
            <Anchor
              onClick={(ev) => ev.preventDefault()}
              getContainer={() =>
                document.getElementById(
                  "exception-container"
                ) as AnchorContainer
              }
              affix={false}
              items={[
                {
                  key: "stack",
                  href: "#exception-stack",
                  title: (
                    <Space>
                      <IconFont type="icon-stack" />
                      <Text>堆栈 (Stack)</Text>
                    </Space>
                  ),
                },
                {
                  key: "query",
                  href: "#exception-query",
                  title: (
                    <Space>
                      <IconFont type="icon-query" style={{ fontSize: 15 }} />
                      <Text>查询 (Query)</Text>
                    </Space>
                  ),
                },
                {
                  key: "cookies",
                  href: "#exception-cookies",
                  title: (
                    <Space>
                      <IconFont type="icon-cookies" style={{ fontSize: 15 }} />
                      <Text>缓存 (Cookies)</Text>
                    </Space>
                  ),
                },
                {
                  key: "request",
                  href: "#exception-request",
                  title: (
                    <Space>
                      <IconFont type="icon-request" style={{ fontSize: 15 }} />
                      <Text>请求体 (Request)</Text>
                    </Space>
                  ),
                },
                {
                  key: "response",
                  href: "#exception-response",
                  title: (
                    <Space>
                      <IconFont type="icon-response" style={{ fontSize: 15 }} />
                      <Text>响应体 (Response)</Text>
                    </Space>
                  ),
                },
                {
                  key: "routing",
                  href: "#exception-routing",
                  title: (
                    <Space>
                      <IconFont type="icon-routing" style={{ fontSize: 15 }} />
                      <Text>路由 (Routing)</Text>
                    </Space>
                  ),
                },
              ]}
            />
          </div>
        </Sider>
        <Content style={{ marginLeft: 15 }}>
          <Title level={5} style={{ marginTop: 0 }} id="exception-stack">
            Stack
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <Space>
                <Button type="primary">登录</Button>
                <Text style={{ color: "#000000a6" }}>还没有账号？</Text>
                <Text style={{ color: "#000000a6" }} underline>
                  注册
                </Text>
              </Space>
            </Space>
          </div>
          <Divider />
          <Title level={5} id="exception-query">
            Query
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <Radio.Group defaultValue="light" buttonStyle="solid">
                <Radio.Button value="light">白天</Radio.Button>
                <Radio.Button value="dark">夜间</Radio.Button>
                <Radio.Button value="system">跟随系统</Radio.Button>
              </Radio.Group>
            </Space>
          </div>
          <Divider />
          <Title level={5} id="exception-cookies">
            Cookies
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <Select
                defaultValue="zh-CN"
                style={{ width: 120 }}
                options={[
                  { value: "zh-CN", label: "简体中文" },
                  { value: "en-US", label: "English" },
                ]}
              />
            </Space>
          </div>
          <Divider />
          <Title level={5} id="exception-request">
            Request
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <Space>
                <Button type="primary">立即备份</Button>
                <Text style={{ color: "#000000a6" }}>
                  最近备份：2023.08.05 12:23:11
                </Text>
              </Space>
            </Space>
          </div>
          <Divider />
          <Title level={5} id="exception-response">
            Response
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <TextArea
                rows={4}
                cols={60}
                showCount
                style={{ height: 120, resize: "none" }}
                placeholder="请写下您的宝贵意见......"
              />
              <Button type="primary" disabled>
                告诉我们
              </Button>
            </Space>
          </div>
          <Divider />
          <Title level={5} id="exception-routing">
            Routing
          </Title>
          <div style={{ marginTop: 15 }}>
            <Space direction="vertical" size={15}>
              <Text style={{ color: "#000000a6" }}>
                一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。
              </Text>
            </Space>
          </div>
          <Divider />
        </Content>
      </Container>
    </Panel>
  );
}
