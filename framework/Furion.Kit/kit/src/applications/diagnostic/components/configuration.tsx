import { SyncOutlined } from "@ant-design/icons";
import { Empty, FloatButton, Space, Tabs, TabsProps, message } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import projectConfig from "../../../project.config";
import ConfigurationProvider from "./configuration-provider";
import Page from "./page";

const Container = styled(Page)``;

interface Metadata {
  id: number;
  provider: string;
  metadata: object;
  isFileConfiguration: boolean;
}

const Configuration: React.FC = () => {
  const [items, setItems] = useState<TabsProps["items"]>();
  const [messageApi, contextHolder] = message.useMessage();

  const loadConfiguration = async () => {
    messageApi.open({
      type: "loading",
      content: "同步配置中...",
      duration: 0,
    });

    const tabItems: TabsProps["items"] = [];

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/configuration-diagnostic`
      );
      tabItems.push({
        key: "Global Configuration",
        label: (
          <Space size={0}>
            <IconFont type="icon-configuration" $size={16} />
            <TextBox $disableSelect $color="#1677ff">
              Global Configuration
            </TextBox>
          </Space>
        ),
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
          label: !item.isFileConfiguration ? (
            <TextBox $disableSelect>{item.provider}</TextBox>
          ) : (
            <Space size={0}>
              <IconFont type="icon-file-configuration" $size={16} />
              <TextBox $disableSelect>{item.provider}</TextBox>
            </Space>
          ),
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

  if (!items || items.length === 0) {
    return (
      <>
        {contextHolder}
        <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="暂无数据" />
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadConfiguration()}
          style={{ bottom: 100 }}
        />
      </>
    );
  }

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
