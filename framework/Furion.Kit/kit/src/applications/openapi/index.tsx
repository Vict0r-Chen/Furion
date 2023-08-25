import { Button, Tabs, TabsProps } from "antd";
import { styled } from "styled-components";
import IconFont from "../../components/iconfont";
import TextBox from "../../components/textbox";
import Content from "../../home/content";
import Page from "./components/page";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const Container = styled.div``;

const items: TabsProps["items"] = [
  {
    key: "1",
    label: <TextBox $disableSelect>Web 端</TextBox>,
    children: <Page />,
  },
  {
    key: "2",
    label: <TextBox $disableSelect>小程序端</TextBox>,
    children: <Page />,
  },
];

const OpenAPI: React.FC = () => {
  return (
    <Content.Main>
      <Content.Title
        description="API 文档、在线调试、接口自动测试，等功能。"
        more={
          <Button type="primary" icon={<AddIcon type="icon-add" $size={16} />}>
            导入
          </Button>
        }
      >
        开放
      </Content.Title>
      <Container>
        <Tabs defaultActiveKey="1" items={items} />
      </Container>
    </Content.Main>
  );
};

export default OpenAPI;
