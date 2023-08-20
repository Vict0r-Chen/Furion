import {
  CardItem,
  CommonConfig,
  DendrogramLayout,
  NodeData,
  RadialTreeGraph,
  TreeGraphData,
} from "@ant-design/graphs";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import projectConfig from "../../../project.config";
import Page from "./page";

const Container = styled(Page)``;

interface ComponentDiagnosticModel {
  components: ComponentModel[];
}

interface ComponentModel {
  name?: string;
  assemblyName?: string;
  assemblyDescription?: string;
  assemblyVersion?: string;
  dependencies?: ComponentModel[];
}

const themeColor = "#73B3D1";
const config: Omit<CommonConfig<DendrogramLayout>, "data"> = {
  nodeCfg: {
    size: 30,
    type: "circle",
    label: {
      style: {
        fill: "#fff",
      },
    },
    style: {
      fill: themeColor,
      stroke: "#0E1155",
      lineWidth: 2,
      strokeOpacity: 0.45,
      shadowColor: themeColor,
      shadowBlur: 25,
    },
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
  const item: NodeData<CardItem> = {
    id: model.name!,
    value: {
      title: model.name,
    },
    children: model.dependencies?.map((sub) => createModel(sub)),
  };

  return item;
};

const Component: React.FC = () => {
  const [data, setData] = useState<TreeGraphData>();

  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await axios.get(
          `${projectConfig.serverAddress}/component-diagnostic`
        );

        const diagnosticModel = response.data as ComponentDiagnosticModel;
        const projectName = response.headers["project-name"];

        const graphData: TreeGraphData = {
          id: projectName,
          value: projectName,
          children: convertJson(diagnosticModel),
        };

        setData(graphData);
      } catch (error) {}
    };

    loadData();
  }, []);

  if (!data) {
    return <div>加载...</div>;
  }

  return (
    <Container>
      <RadialTreeGraph data={data!} {...config} />
    </Container>
  );
};

export default Component;
