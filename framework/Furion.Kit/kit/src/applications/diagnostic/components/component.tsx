import {
  CardItem,
  CommonConfig,
  DendrogramLayout,
  NodeData,
  RadialTreeGraph,
  TreeGraphData,
} from "@ant-design/graphs";
import { SyncOutlined } from "@ant-design/icons";
import { Empty, FloatButton, Space, Spin } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import TextBox from "../../../components/textbox";
import projectConfig from "../../../project.config";
import Page from "./page";

const Container = styled(Page)``;

const Dot = styled.span<{ $bgColor: string }>`
  display: inline-block;
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background-color: ${(props) => props.$bgColor};
`;

interface ComponentDiagnosticModel {
  components: ComponentModel[];
}

interface ComponentModel {
  unique?: string;
  name?: string;
  fullName?: string;
  assemblyName?: string;
  assemblyDescription?: string;
  assemblyVersion?: string;
  dependencies?: ComponentModel[];
}

const ItemContainer = styled.div`
  font-size: 13px;
  color: #595959;
`;

const ItemLabel = styled(TextBox)`
  color: #000000e0;
  font-weight: 600;
`;

const ItemText = styled(TextBox)`
  margin-left: 5px;
`;

const ComponentItem: React.FC<ComponentModel> = (model) => {
  return (
    <ItemContainer>
      {model.unique ? (
        <Space direction="vertical" size={5}>
          <Space direction="vertical" size={5}>
            <ItemLabel>类型</ItemLabel>
            <ItemText>{model.fullName}</ItemText>
          </Space>
          <Space direction="vertical" size={5}>
            <ItemLabel>程序集</ItemLabel>
            <ItemText>
              {model.assemblyName} {model.assemblyVersion}
            </ItemText>
          </Space>
          {model.assemblyDescription && (
            <Space direction="vertical" size={5}>
              <ItemLabel>说明</ItemLabel>
              <ItemText>{model.assemblyDescription}</ItemText>
            </Space>
          )}
        </Space>
      ) : (
        <Space direction="vertical" size={5}>
          <Space direction="vertical" size={5}>
            <ItemLabel>启动项目</ItemLabel>
            <ItemText>{(model as any).value}</ItemText>
          </Space>
        </Space>
      )}
    </ItemContainer>
  );
};

const getColor = (node: any) => {
  return node.id === "root"
    ? "#52c41a"
    : node.isEntry === true
    ? "#faad14"
    : themeColor;
};

const themeColor = "#91caff";
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
      fill: getColor(node),
      stroke: "#0E1155",
      lineWidth: 1,
      strokeOpacity: 0.45,
      shadowColor: getColor(node),
      shadowBlur: 10,
    }),
    nodeStateStyles: {
      hover: {
        stroke: themeColor,
        lineWidth: 1,
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
        lineWidth: 1,
      },
    },
  },
  behaviors: ["drag-canvas", "zoom-canvas", "drag-node"],
};

const convertJson = (componentDiagnosticModel: ComponentDiagnosticModel) => {
  var list: NodeData<CardItem>[] = [];

  for (const cmp of componentDiagnosticModel.components) {
    var model = createModel(cmp) as any;
    model.isEntry = true;

    list.push(model);
  }

  return list;
};

const createModel = (model: ComponentModel) => {
  const item: any = {
    ...model,
    id: model.unique,
    value: {
      title: model.fullName,
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

  if (!data) {
    return (
      <>
        <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="暂无数据" />
        <FloatButton
          icon={<SyncOutlined />}
          onClick={() => loadData()}
          style={{ bottom: 100 }}
        />
      </>
    );
  }

  return (
    <Spin spinning={loading}>
      <Container>
        <div style={{ textAlign: "right" }}>
          <Space>
            <Space>
              <Dot $bgColor="#52c41a" />
              <TextBox $color="#000000a6">启动项目</TextBox>
            </Space>
            <Space>
              <Dot $bgColor="#faad14" />
              <TextBox $color="#000000a6">入口组件</TextBox>
            </Space>
            <Space>
              <Dot $bgColor={themeColor} />
              <TextBox $color="#000000a6">依赖组件</TextBox>
            </Space>
          </Space>
        </div>
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
