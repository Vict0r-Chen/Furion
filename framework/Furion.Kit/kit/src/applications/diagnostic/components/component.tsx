import {
  CardItem,
  CommonConfig,
  DendrogramLayout,
  NodeData,
  RadialTreeGraph,
  TreeGraphData,
} from "@ant-design/graphs";
import { SyncOutlined } from "@ant-design/icons";
import { FloatButton, Space, Spin } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import TextBox from "../../../components/textbox";
import projectConfig from "../../../project.config";
import Page from "./page";

const Container = styled(Page)``;

interface ComponentDiagnosticModel {
  components: ComponentModel[];
}

interface ComponentModel {
  guid?: string;
  name?: string;
  assemblyName?: string;
  assemblyDescription?: string;
  assemblyVersion?: string;
  dependencies?: ComponentModel[];
}

const ComponentItem: React.FC<ComponentModel> = (model) => {
  return model.guid ? (
    <Space direction="vertical">
      <TextBox>{model.name}</TextBox>
      <TextBox>{model.assemblyName}</TextBox>
      <TextBox>{model.assemblyVersion}</TextBox>
      {model.assemblyDescription && (
        <TextBox>{model.assemblyDescription}</TextBox>
      )}
    </Space>
  ) : (
    <TextBox>启动项目</TextBox>
  );
};

const themeColor = "#73B3D1";
const config: Omit<CommonConfig<DendrogramLayout>, "data"> = {
  menuCfg: {
    show: false,
  },
  tooltipCfg: {
    show: true,
    customContent: (node) => <ComponentItem {...(node as ComponentModel)} />,
  },
  nodeCfg: {
    size: 30,
    type: "circle",
    label: {
      style: {
        fill: "#fff",
      },
    },
    style: (node: any) => ({
      fill: node.id === "root" ? "#52c41a" : themeColor,
      stroke: "#0E1155",
      lineWidth: 2,
      strokeOpacity: 0.45,
      shadowColor: themeColor,
      shadowBlur: 25,
    }),
    nodeStateStyles: {
      hover: {
        stroke: themeColor,
        lineWidth: 2,
        strokeOpacity: 1,
      },
    },
  },
  edgeCfg: {
    style: {
      stroke: themeColor,
      shadowColor: themeColor,
      shadowBlur: 20,
    },
    endArrow: {
      type: "triangle",
      fill: themeColor,
      d: 15,
      size: 8,
    },
    edgeStateStyles: {
      hover: {
        stroke: themeColor,
        lineWidth: 2,
      },
    },
  },
  behaviors: ["drag-canvas", "zoom-canvas", "drag-node"],
};

const convertJson = (componentDiagnosticModel: ComponentDiagnosticModel) => {
  var list: NodeData<CardItem>[] = [];

  for (const cmp of componentDiagnosticModel.components) {
    list.push(createModel(cmp));
  }

  return list;
};

const createModel = (model: ComponentModel) => {
  const item: any = {
    ...model,
    id: model.guid,
    value: {
      title: model.name,
    },
    children: model.dependencies?.map((sub) => createModel(sub)),
  };

  return item as NodeData<CardItem>;
};

const Component: React.FC = () => {
  const [data, setData] = useState<TreeGraphData>();
  const [loading, setLoading] = useState(false);

  const loadData = async () => {
    setLoading(true);

    try {
      const response = await axios.get(
        `${projectConfig.serverAddress}/component-diagnostic`
      );

      const diagnosticModel = response.data as ComponentDiagnosticModel;
      const projectName = response.headers["project-name"];

      const graphData: TreeGraphData = {
        id: "root",
        value: projectName,
        children: convertJson(diagnosticModel),
      };

      setData(graphData);
    } catch (error) {}

    setLoading(false);
  };

  useEffect(() => {
    loadData();
  }, []);

  return (
    <Spin spinning={loading || !data}>
      <Container>
        {data && <RadialTreeGraph data={data} {...config} />}
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadData()}
          style={{ bottom: 100 }}
        />
      </Container>
    </Spin>
  );
};

export default Component;
