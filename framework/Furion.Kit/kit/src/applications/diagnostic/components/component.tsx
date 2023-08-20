import { RadialTreeGraph, RadialTreeGraphConfig } from "@ant-design/graphs";
import React from "react";
import { styled } from "styled-components";
import Page from "./page";

const Container = styled(Page)``;

const Component: React.FC = () => {
  const config: RadialTreeGraphConfig = {
    data: {
      id: "root",
      value: {
        title: "Furion.Tests",
      },
      children: [
        {
          id: "无聊",
          value: {
            title: "其他啊",
          },
          children: [
            {
              id: "哈哈哈",
              value: {
                title: "是全球",
                items: [
                  {
                    text: "内容",
                  },
                ],
              },
            },
          ],
        },
        {
          id: "无聊2",
          value: {
            title: "其他啊",
          },
          children: [
            {
              id: "哈哈哈2",
              value: {
                title: "是全球",
                items: [
                  {
                    text: "内容",
                  },
                ],
              },
            },
          ],
        },
        {
          id: "无聊3",
          value: {
            title: "其他啊",
          },
          children: [
            {
              id: "哈哈哈23",
              value: {
                title: "是全球",
                items: [
                  {
                    text: "内容",
                  },
                ],
              },
            },
          ],
        },
      ],
    },
    nodeCfg: {
      type: "diamond",
    },
    behaviors: ["drag-canvas", "zoom-canvas", "drag-node"],
  };

  return (
    <Container>
      <RadialTreeGraph {...config} />
    </Container>
  );
};

export default Component;
