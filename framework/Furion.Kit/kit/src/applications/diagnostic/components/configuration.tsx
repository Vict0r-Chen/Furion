import { SyncOutlined } from "@ant-design/icons";
import { Alert, Empty, FloatButton, Space, Spin, Tabs, TabsProps } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import projectConfig from "../../../project.config";
import ConfigurationProvider from "./configuration-provider";
import Page from "./page";

const Container = styled(Page)``;

const AlertBox = styled(Alert)`
  margin-bottom: 15px;
`;

interface Metadata {
  id: number;
  provider: string;
  metadata: object;
  isFileConfiguration: boolean;
}

const providerTipColor = "#52c41a";

const getAppsettingsColor = (provider: string) => {
  return provider.indexOf("appsettings.") > -1 ? providerTipColor : undefined;
};

const Configuration: React.FC = () => {
  const [items, setItems] = useState<TabsProps["items"]>();
  const [environmentName, setEnvironmentName] = useState<string>("Unknown");
  const [loading, setLoading] = useState(false);

  const loadConfiguration = async () => {
    setLoading(true);

    const tabItems: TabsProps["items"] = [];

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/configuration-diagnostic`
      );
      tabItems.push({
        key: "Global Configuration",
        label: (
          <Space size={0}>
            <IconFont
              type="icon-configuration"
              $size={16}
              $color={providerTipColor}
            />
            <TextBox $disableSelect $color={providerTipColor}>
              Global Configuration
            </TextBox>
          </Space>
        ),
        children: <ConfigurationProvider data={response.data} />,
      });

      setEnvironmentName(response.headers["environment-name"]);
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
              <IconFont
                type="icon-file-configuration"
                $color={getAppsettingsColor(item.provider)}
                $size={16}
              />
              <TextBox
                $disableSelect
                $color={getAppsettingsColor(item.provider)}
              >
                {item.provider}
              </TextBox>
            </Space>
          ),
          children: <ConfigurationProvider data={item.metadata} />,
        })
      );
    } catch (error) {}

    setItems(tabItems);
    setLoading(false);
  };

  useEffect(() => {
    loadConfiguration();
  }, []);

  if (!items || items.length === 0) {
    return (
      <Spin spinning={loading}>
        <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="暂无数据" />
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadConfiguration()}
          style={{ bottom: 100 }}
        />
      </Spin>
    );
  }

  return (
    <Spin spinning={loading}>
      <Container>
        <AlertBox message={"当前配置环境：" + environmentName} type="warning" />
        <Tabs tabPosition="right" items={items} tabBarGutter={0} />
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadConfiguration()}
          style={{ bottom: 100 }}
        />
      </Container>
    </Spin>
  );
};

export default Configuration;
