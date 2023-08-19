import { SyncOutlined } from "@ant-design/icons";
import { FloatButton, Tabs, TabsProps, message } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import projectConfig from "../../../project.config";
import ConfigurationProvider from "./configuration-provider";
import Page from "./page";

const Container = styled(Page)``;

interface Metadata {
  id: number;
  provider: string;
  metadata: object;
}

const Configuration: React.FC = () => {
  const [items, setItems] = useState<TabsProps["items"]>();
  const [messageApi, contextHolder] = message.useMessage();

  const loadConfiguration = async () => {
    messageApi.open({
      type: "loading",
      content: "加载中...",
      duration: 0,
    });

    const tabItems: TabsProps["items"] = [];

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/configuration-diagnostic`
      );
      tabItems.push({
        key: "Global Configuration",
        label: "Global Configuration",
        children: <ConfigurationProvider data={response.data} />,
      });
    } catch (error) {}

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/configuration-provider-diagnostic`
      );
      var metadata = response.data as Metadata[];
      metadata.forEach((item) =>
        tabItems.push({
          key: item.id.toString(),
          label: item.provider,
          children: <ConfigurationProvider data={item.metadata} />,
        })
      );
    } catch (error) {}

    messageApi.destroy();
    setItems(tabItems);
  };

  useEffect(() => {
    loadConfiguration();
  }, []);

  return (
    <>
      {contextHolder}
      <Container>
        <Tabs tabPosition="right" items={items} tabBarGutter={0} />
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadConfiguration()}
          style={{ bottom: 100 }}
        />
      </Container>
    </>
  );
};

export default Configuration;
