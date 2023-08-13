import {
  Button,
  Drawer,
  Dropdown,
  Space,
  Tabs,
  TabsProps,
  message,
} from "antd";
import React, { useState } from "react";
import { Outlet, useNavigate, useParams } from "react-router-dom";
import { styled } from "styled-components";
import Fullscreen from "../../components/fullscreen";
import IconFont from "../../components/iconfont";
import SearchBox from "../../components/searchbox";
import TextBox from "../../components/textbox";
import Content from "../../home/content";
import Community from "./components/community";
import Local from "./components/local";
import Official from "./components/official";
import ExploreContext from "./context";
export { default as ExploreDetail } from "./detail";

const onChange = (key: string) => {
  console.log(key);
};

const Icon = styled(IconFont)`
  margin-right: 0 !important;
`;

const items: TabsProps["items"] = [
  {
    key: "1",
    label: (
      <Space>
        <Icon type="icon-local" $size={16} />
        <TextBox $disableSelect>本地应用</TextBox>
      </Space>
    ),
    children: <Local />,
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-curated" $size={16} />
        <TextBox $disableSelect>官方精选</TextBox>
      </Space>
    ),
    children: <Official />,
  },
  {
    key: "3",
    label: (
      <Space>
        <Icon type="icon-community" $size={16} />
        <TextBox $disableSelect>社区推荐</TextBox>
      </Space>
    ),
    children: <Community />,
  },
];

const Explore: React.FC = () => {
  const navigate = useNavigate();
  const { name } = useParams();

  const [, contextHolder] = message.useMessage();

  const [open, setOpen] = useState(name ? true : false);
  const [fullscreen, setFullscreen] = useState(false);

  const showDrawer = (name: string) => {
    setOpen(true);
    navigate(`detail/${name}`);
  };

  const onClose = () => {
    setOpen(false);
    setTimeout(() => {
      navigate("/explore");
    }, 300);
  };

  return (
    <ExploreContext.Provider value={{ showDrawer }}>
      {contextHolder}
      <Drawer
        title="面板"
        placement="right"
        onClose={onClose}
        open={open}
        autoFocus={false}
        size="large"
        width={fullscreen ? "100%" : undefined}
        extra={
          <Fullscreen
            fullscreen={fullscreen}
            onClick={() => setFullscreen((f) => !f)}
          />
        }
      >
        <Outlet />
      </Drawer>
      <Content.Main>
        <Content.Title
          description="工具、文档、管理、娱乐，更多应用等你发掘。"
          extra={
            <Content.Menu
              menu={{
                items: [
                  {
                    key: 1,
                    label: "应用配置",
                    icon: <IconFont type="icon-configuration" $size={16} />,
                  },
                ],
              }}
            />
          }
        >
          探索
        </Content.Title>
        <Tabs
          tabBarExtraContent={{
            right: (
              <Space>
                <SearchBox placeholder="ChatGPT 电商" />
                <Dropdown
                  placement="bottomRight"
                  menu={{
                    items: [
                      {
                        key: 1,
                        label: "选择应用包",
                        icon: <IconFont type="icon-upload" $size={16} />,
                      },
                    ],
                  }}
                >
                  <Button type="primary">上传</Button>
                </Dropdown>
              </Space>
            ),
          }}
          defaultActiveKey="1"
          items={items}
          onChange={onChange}
        />
      </Content.Main>
    </ExploreContext.Provider>
  );
};

export default Explore;
