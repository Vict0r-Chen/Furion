import { Input, Space, Tabs, TabsProps } from "antd";
import { styled } from "styled-components";
import CategoryList from "../../components/category-list";
import IconFont from "../../components/iconfont";
import TextBox from "../../components/textbox";
import Content from "../../home/content";
import Main from "./components/main";

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
    children: (
      <Main>
        <div>
          <Space direction="vertical" size={20}>
            <CategoryList
              title="分类"
              items={[
                { key: "1", label: "全部" },
                { key: "2", label: "工具" },
                { key: "3", label: "文档" },
              ]}
            />
          </Space>
        </div>
        <br />
        本地应用
      </Main>
    ),
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-curated" $size={15} />
        <TextBox $disableSelect>官方精选</TextBox>
      </Space>
    ),
    children: (
      <Main>
        <div>
          <Space direction="vertical" size={20}>
            <CategoryList
              title="分类"
              items={[
                { key: "1", label: "全部" },
                { key: "2", label: "视频" },
                { key: "3", label: "智能" },
                { key: "4", label: "工具" },
              ]}
            />
            <CategoryList
              title="综合"
              items={[
                { key: "1", label: "全部" },
                { key: "2", label: "最热" },
                { key: "3", label: "最新" },
              ]}
            />
          </Space>
        </div>
        <br />
        官方精选
      </Main>
    ),
  },
  {
    key: "3",
    label: (
      <Space>
        <Icon type="icon-community" $size={15} />
        <TextBox $disableSelect>社区维护</TextBox>
      </Space>
    ),
    children: (
      <Main>
        <div>
          <Space direction="vertical" size={20}>
            <CategoryList
              title="分类"
              items={[
                { key: "1", label: "全部" },
                { key: "2", label: "视频" },
                { key: "3", label: "智能" },
                { key: "4", label: "工具" },
              ]}
            />
            <CategoryList
              title="综合"
              items={[
                { key: "1", label: "全部" },
                { key: "2", label: "最热" },
                { key: "3", label: "最新" },
              ]}
            />
          </Space>
        </div>
        <br />
        社区维护
      </Main>
    ),
  },
];

const Explore: React.FC = () => {
  return (
    <Content.Main>
      <Content.Title description="工具、文档、管理、娱乐，更多应用等你发掘。">
        探索
      </Content.Title>
      <Tabs
        tabBarExtraContent={{
          right: <Search placeholder="ChatGPT 后台" enterButton />,
        }}
        defaultActiveKey="1"
        items={items}
        onChange={onChange}
      />
    </Content.Main>
  );
};

export default Explore;
