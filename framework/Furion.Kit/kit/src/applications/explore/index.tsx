import { Button, Dropdown, Input, Space, Tabs, TabsProps } from "antd";
import { useState } from "react";
import { styled } from "styled-components";
import IconFont from "../../components/iconfont";
import SearchBox from "../../components/searchbox";
import TextBox from "../../components/textbox";
import Content from "../../home/content";
import Community from "./community";
import Local from "./local";
import Official from "./official";

const { Search } = Input;

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
        <Icon type="icon-local" $size={15} />
        <TextBox $disableSelect>本地应用</TextBox>
      </Space>
    ),
    children: <Local />,
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-curated" $size={15} />
        <TextBox $disableSelect>官方精选</TextBox>
      </Space>
    ),
    children: <Official />,
  },
  {
    key: "3",
    label: (
      <Space>
        <Icon type="icon-community" $size={15} />
        <TextBox $disableSelect>社区维护</TextBox>
      </Space>
    ),
    children: <Community />,
  },
];

const defaultWidth = 180;

const Explore: React.FC = () => {
  const [width, setWidth] = useState(defaultWidth);

  return (
    <Content.Main>
      <Content.Title
        description="工具、文档、管理、娱乐，更多应用等你发掘。"
        extra={
          <Content.Menu
            menu={{
              items: [
                {
                  key: 1,
                  label: "配置",
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
  );
};

export default Explore;
