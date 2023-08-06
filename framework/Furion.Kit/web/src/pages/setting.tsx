import {
  Button,
  Divider,
  Input,
  Layout,
  Radio,
  Select,
  Space,
  Typography,
} from "antd";
import IconFont from "../components/iconfont";

const { TextArea } = Input;
const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Setting() {
  return (
    <div>
      <Title level={3}>设置</Title>
      <Layout>
        <Sider theme="light">
          <div style={{ marginTop: 20 }}>
            <Space direction="vertical" size={20}>
              <Space>
                <IconFont
                  type="icon-account"
                  style={{ fontSize: 15, color: "#1677ff" }}
                />
                <Text style={{ color: "#1677ff" }}>账户设置</Text>
              </Space>
              <Space>
                <IconFont type="icon-appearance" style={{ fontSize: 15 }} />
                <Text>外观</Text>
              </Space>
              <Space>
                <IconFont type="icon-backup" style={{ fontSize: 15 }} />
                <Text>备份</Text>
              </Space>
              <Space>
                <IconFont type="icon-advice" style={{ fontSize: 15 }} />
                <Text>意见反馈</Text>
              </Space>
              <Space>
                <IconFont type="icon-aboutus" style={{ fontSize: 15 }} />
                <Text>关于我们</Text>
              </Space>
            </Space>
          </div>
        </Sider>
        <Content style={{ backgroundColor: "#ffffff" }}>
          <Title level={5} style={{ marginTop: 0 }}>
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
          <Title level={5}>外观</Title>
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
          <Title level={5}>语言</Title>
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
          <Title level={5}>备份</Title>
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
          <Title level={5}>意见反馈</Title>
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
          <Title level={5}>关于我们</Title>
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
