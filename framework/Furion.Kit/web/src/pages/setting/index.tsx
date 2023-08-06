import {
  Anchor,
  Button,
  Checkbox,
  Divider,
  Input,
  Layout,
  Popconfirm,
  Radio,
  Select,
  Space,
  Typography,
  message,
} from "antd";
import { AnchorContainer } from "antd/es/anchor/Anchor";
import React, { useState } from "react";
import IconFont from "../../components/iconfont";
import { Container, Panel } from "./style";

const { TextArea } = Input;
const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Setting() {
  const [advice, setAdvice] = useState("");
  const [messageApi, contextHolder] = message.useMessage();

  const success = (message: string = "保存成功") => {
    messageApi.open({
      type: "success",
      content: message,
    });
  };

  const openBackupMessage = () => {
    messageApi.open({
      key: "backup",
      type: "loading",
      content: "备份中...",
    });
    setTimeout(() => {
      messageApi.open({
        key: "backup",
        type: "success",
        content: "备份成功",
      });
    }, 2000);
  };

  return (
    <>
      {contextHolder}
      <Panel>
        <Title level={3}>设置</Title>
        <Container id="setting-container">
          <Sider
            theme="light"
            style={{ position: "sticky", overflowY: "auto", top: 0 }}
          >
            <div style={{ marginTop: 20 }}>
              <Anchor
                onClick={(ev) => ev.preventDefault()}
                getContainer={() =>
                  document.getElementById(
                    "setting-container"
                  ) as AnchorContainer
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
                        <IconFont
                          type="icon-language"
                          style={{ fontSize: 15 }}
                        />
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
                        <IconFont
                          type="icon-aboutus"
                          style={{ fontSize: 15 }}
                        />
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
                  <Text
                    style={{ color: "#000000a6", cursor: "pointer" }}
                    underline
                  >
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
                <Radio.Group
                  defaultValue="白天"
                  buttonStyle="solid"
                  onChange={() => success("切换成功")}
                >
                  <Radio.Button value="白天">白天</Radio.Button>
                  <Radio.Button value="夜间">夜间</Radio.Button>
                  <Radio.Button value="跟随系统">跟随系统</Radio.Button>
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
                  onChange={() => success("切换成功")}
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
                  <Button type="primary" onClick={() => openBackupMessage()}>
                    立即备份
                  </Button>
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
                  allowClear
                  style={{ height: 120, resize: "none" }}
                  placeholder="请写下您的宝贵意见，10个字以上。"
                  value={advice}
                  onChange={(ev) => {
                    setAdvice(ev.target.value);
                  }}
                />
                <Space>
                  <Button
                    type="primary"
                    disabled={advice.length < 10}
                    onClick={() => {
                      setAdvice("");
                      success("已成功发送，我们将在 7 个工作日内给您答复。");
                    }}
                  >
                    通知我们
                  </Button>
                  <Text style={{ color: "#000000a6" }}>哪些信息可以收集？</Text>

                  <Popconfirm
                    placement="topLeft"
                    title="配置您许可的信息收集。"
                    description={<Collect />}
                    onConfirm={() => {}}
                    okText="选好了"
                    cancelText="取消"
                  >
                    <Text
                      style={{ color: "#000000a6", cursor: "pointer" }}
                      underline
                    >
                      配置
                    </Text>
                  </Popconfirm>
                </Space>
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
        </Container>
      </Panel>
    </>
  );
}

const Collect: React.FC = () => {
  return (
    <div style={{ padding: "10px 20px 10px 0" }}>
      <Space direction="vertical">
        <Checkbox defaultChecked>操作系统 (版本/MAC/IP)</Checkbox>
        <Checkbox defaultChecked>浏览器 (版本)</Checkbox>
        <Checkbox defaultChecked>项目 (名称/网站/端口)</Checkbox>
        <Checkbox defaultChecked disabled>
          框架 (.NET SDK/Furion)
        </Checkbox>
      </Space>
    </div>
  );
};
