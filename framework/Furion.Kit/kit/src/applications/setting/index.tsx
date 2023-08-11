import {
  Button,
  Checkbox,
  Input,
  Popconfirm,
  Radio,
  Select,
  Space,
  message,
} from "antd";
import { useState } from "react";
import { styled } from "styled-components";
import AnchorBox from "../../components/anchor-box";
import Category from "../../components/category";
import Flexbox from "../../components/flexbox";
import IconFont from "../../components/iconfont";
import SiderSticky from "../../components/sider-sticky";
import TextBox from "../../components/textbox";
import Content from "../../home/content";

const { TextArea } = Input;

const Container = styled(Flexbox)`
  align-items: flex-start;
`;

const Main = styled.div`
  flex: 1;
  margin-top: -30px;
`;

const textColor = "#000000a6";
const iconSize = 16;

const Setting: React.FC = () => {
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
      <Content.Main>
        <Content.Title>设置</Content.Title>
        <Container>
          <SiderSticky>
            <AnchorBox
              items={[
                {
                  key: "account",
                  href: "#setting-account",
                  title: (
                    <Space>
                      <IconFont type="icon-account" $size={iconSize} />
                      <TextBox>账户设置</TextBox>
                    </Space>
                  ),
                },
                {
                  key: "apperance",
                  href: "#setting-appearance",
                  title: (
                    <Space>
                      <IconFont type="icon-appearance" $size={iconSize} />
                      <TextBox>外观</TextBox>
                    </Space>
                  ),
                },
                {
                  key: "language",
                  href: "#setting-language",
                  title: (
                    <Space>
                      <IconFont type="icon-language" $size={iconSize} />
                      <TextBox>语言</TextBox>
                    </Space>
                  ),
                },
                {
                  key: "backup",
                  href: "#setting-backup",
                  title: (
                    <Space>
                      <IconFont type="icon-backup" $size={iconSize} />
                      <TextBox>备份</TextBox>
                    </Space>
                  ),
                },
                {
                  key: "advice",
                  href: "#setting-advice",
                  title: (
                    <Space>
                      <IconFont type="icon-advice" $size={iconSize} />
                      <TextBox>意见反馈</TextBox>
                    </Space>
                  ),
                },
                {
                  key: "aboutus",
                  href: "#setting-aboutus",
                  title: (
                    <Space>
                      <IconFont type="icon-aboutus" $size={iconSize} />
                      <TextBox>关于我们</TextBox>
                    </Space>
                  ),
                },
              ]}
            />
          </SiderSticky>
          <Main>
            <Category
              id="setting-account"
              title="账户设置"
              icon={<IconFont type="icon-account" $size={iconSize} />}
            >
              <Space direction="vertical">
                <Space>
                  <Button type="primary">登录</Button>
                  <TextBox $color={textColor}>还没有账号？</TextBox>
                  <TextBox $color={textColor} $pointer underline>
                    注册
                  </TextBox>
                </Space>
              </Space>
            </Category>
            <Category
              id="setting-appearance"
              title="外观"
              icon={<IconFont type="icon-appearance" $size={iconSize} />}
            >
              <Space direction="vertical">
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
            </Category>
            <Category
              id="setting-language"
              title="语言"
              icon={<IconFont type="icon-language" $size={iconSize} />}
            >
              <Space direction="vertical">
                <Select
                  defaultValue="zh-CN"
                  style={{ width: 120 }}
                  options={[
                    { value: "zh-CN", label: "简体中文" },
                    { value: "en-US", label: "English" },
                  ]}
                  onChange={() => success("切换成功")}
                />
              </Space>
            </Category>
            <Category
              id="setting-backup"
              title="备份"
              icon={<IconFont type="icon-backup" $size={iconSize} />}
            >
              <Space direction="vertical">
                <Space>
                  <Button type="primary" onClick={() => openBackupMessage()}>
                    立即备份
                  </Button>
                  <TextBox $color={textColor}>
                    最近备份：2023.08.05 12:23:11
                  </TextBox>
                </Space>
              </Space>
            </Category>
            <Category
              id="setting-advice"
              title="意见反馈"
              icon={<IconFont type="icon-advice" $size={iconSize} />}
            >
              <Space direction="vertical">
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
                  <TextBox $color={textColor}>哪些信息可以收集？</TextBox>

                  <Popconfirm
                    placement="topLeft"
                    title="我们可以收集您哪些信息？"
                    description={<Collect />}
                    onConfirm={() => {}}
                    okText="选好了"
                    cancelText="取消"
                  >
                    <TextBox $color={textColor} $pointer underline>
                      配置
                    </TextBox>
                  </Popconfirm>
                </Space>
              </Space>
            </Category>
            <Category
              id="setting-aboutus"
              title="关于我们"
              icon={<IconFont type="icon-aboutus" $size={iconSize} />}
            >
              <Space direction="vertical" size={15}>
                <TextBox $color={textColor}>
                  一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。
                </TextBox>
              </Space>
            </Category>
          </Main>
        </Container>
      </Content.Main>
    </>
  );
};

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

export default Setting;
