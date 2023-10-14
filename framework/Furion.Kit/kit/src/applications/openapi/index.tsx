import { Button, Space, Tabs, TabsProps } from "antd";
import axios from "axios";
import { useEffect, useMemo, useState } from "react";
import styled from "styled-components";
import IconFont from "../../components/iconfont";
import TextBox from "../../components/textbox";
import { OpenApiModel } from "../../databases/types/openapi";
import Content from "../../home/content";
import projectConfig from "../../project.config";
import Page from "./components/page";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const Container = styled.div``;

const Icon = styled(IconFont)`
  margin-right: 0 !important;
  position: relative;
  top: 1px;
`;

const OpenAPI: React.FC = () => {
  const [data, setData] = useState<OpenApiModel>();
  const [loading, setLoading] = useState(false);

  const items = useMemo(() => {
    if (!data) {
      return undefined;
    }

    const _items: TabsProps["items"] = [];
    for (const group of data.groups!) {
      _items.push({
        key: group.name!,
        label: (
          <Space>
            <Icon type="icon-api" $size={16} />
            <TextBox $disableSelect>{group.name}</TextBox>
          </Space>
        ),
        children: <Page {...group} />,
      });
    }

    return _items;
  }, [data]);

  const loadData = async () => {
    setLoading(true);

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/openapi`
      );

      setData(response.data);
    } catch (error) {}

    setLoading(false);
  };

  useEffect(() => {
    loadData();
  }, []);

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
