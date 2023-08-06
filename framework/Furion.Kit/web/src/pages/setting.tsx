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
import IconFont from "../components/iconfont";

const { TextArea } = Input;
const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Setting() {
  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100%" }}>
      <Title level={3}>设置</Title>
      <Layout
        id="setting-container"
        style={{
          overflow: "auto",
          flex: 1,
          backgroundColor: "#ffffff",
        }}
      >
        <Sider
          theme="light"
          style={{ position: "sticky", overflowY: "auto", top: 0 }}
        >
          <div style={{ marginTop: 20 }}>
            <Anchor
              onClick={(ev) => ev.preventDefault()}
              getContainer={() =>
                document.getElementById("setting-container") as AnchorContainer
              }
              affix={false}
              items={[
                {
                  key: "account",
                  href: "#setting-account",
                  title: (
                    <Space>
                      <IconFont type="icon-account" />
                      <Text>账户设置</Text>
                    </Space>
                  ),
                },
                {
                  key: "apperance",
                  href: "#setting-appearance",
                  title: (
                    <Space>
                      <IconFont
                        type="icon-appearance"
                        style={{ fontSize: 15 }}
                      />
                      <Text>外观</Text>
                    </Space>
                  ),
                },
                {
                  key: "language",
                  href: "#setting-language",
                  title: (
                    <Space>
                      <IconFont type="icon-language" style={{ fontSize: 15 }} />
                      <Text>语言</Text>
                    </Space>
                  ),
                },
                {
                  key: "backup",
                  href: "#setting-backup",
                  title: (
                    <Space>
                      <IconFont type="icon-backup" style={{ fontSize: 15 }} />
                      <Text>备份</Text>
                    </Space>
                  ),
                },
                {
                  key: "advice",
                  href: "#setting-advice",
                  title: (
                    <Space>
                      <IconFont type="icon-advice" style={{ fontSize: 15 }} />
                      <Text>意见反馈</Text>
                    </Space>
                  ),
                },
                {
                  key: "aboutus",
                  href: "#setting-aboutus",
                  title: (
                    <Space>
                      <IconFont type="icon-aboutus" style={{ fontSize: 15 }} />
                      <Text>关于我们</Text>
                    </Space>
                  ),
                },
              ]}
            />
          </div>
        </Sider>
        <Content style={{ marginLeft: 15 }}>
          <Title level={5} style={{ marginTop: 0 }} id="setting-account">
            账户设置
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
          <Title level={5} id="setting-appearance">
            外观
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
          <Title level={5} id="setting-language">
            语言
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
          <Title level={5} id="setting-backup">
            备份
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
          <Title level={5} id="setting-advice">
            意见反馈
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
          <Title level={5} id="setting-aboutus">
            关于我们
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
      </Layout>
    </div>
  );
}
