import { DownOutlined } from "@ant-design/icons";
import { FloatButton, Space, Tree } from "antd";
import type { DataNode } from "antd/es/tree";
import React, { useId } from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import { FlushDivider } from "../../../components/flush-divider";
import HttpMethod from "../../../components/http-method";
import SiderSticky from "../../../components/sider-sticky";
import TextBox from "../../../components/textbox";

const Container = styled(Flexbox)`
  overflow-y: auto;
  height: calc(100vh - 242px);
  box-sizing: border-box;
  position: relative;
  align-items: flex-start;
`;

const Main = styled.div`
  flex: 1;
  box-sizing: border-box;
  padding: 0 15px;
`;

const httpMethodWidth = 70;

const treeData: DataNode[] = [
  {
    title: "Hello-Controller",
    key: "0-0",
    children: [
      {
        title: (
          <Space>
            <HttpMethod value="GET" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/get</TextBox>
          </Space>
        ),
        key: "0-0-0",
      },
      {
        title: (
          <Space>
            <HttpMethod value="POST" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/post</TextBox>
          </Space>
        ),
        key: "0-0-1",
      },
      {
        title: (
          <Space>
            <HttpMethod value="DELETE" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/delete</TextBox>
          </Space>
        ),
        key: "0-0-2",
      },
      {
        title: (
          <Space>
            <HttpMethod value="PUT" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/put</TextBox>
          </Space>
        ),
        key: "0-0-3",
      },
      {
        title: (
          <Space>
            <HttpMethod value="HEAD" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/head</TextBox>
          </Space>
        ),
        key: "0-0-4",
      },
      {
        title: (
          <Space>
            <HttpMethod value="PATCH" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/patch</TextBox>
          </Space>
        ),
        key: "0-0-5",
      },
      {
        title: (
          <Space>
            <HttpMethod value="OPTIONS" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/options</TextBox>
          </Space>
        ),
        key: "0-0-6",
      },
      {
        title: (
          <Space>
            <HttpMethod value="TRACE" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/trace</TextBox>
          </Space>
        ),
        key: "0-0-7",
      },
      {
        title: (
          <Space>
            <HttpMethod value="CONNECT" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/hello/connect</TextBox>
          </Space>
        ),
        key: "0-0-8",
      },
    ],
  },
  {
    title: "Home-Controller",
    key: "0-1",
    children: [
      {
        title: (
          <Space>
            <HttpMethod value="GET" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/get</TextBox>
          </Space>
        ),
        key: "0-1-0",
      },
      {
        title: (
          <Space>
            <HttpMethod value="POST" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/post</TextBox>
          </Space>
        ),
        key: "0-1-1",
      },
      {
        title: (
          <Space>
            <HttpMethod value="DELETE" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/delete</TextBox>
          </Space>
        ),
        key: "0-1-2",
      },
      {
        title: (
          <Space>
            <HttpMethod value="PUT" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/put</TextBox>
          </Space>
        ),
        key: "0-1-3",
      },
      {
        title: (
          <Space>
            <HttpMethod value="HEAD" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/head</TextBox>
          </Space>
        ),
        key: "0-1-4",
      },
      {
        title: (
          <Space>
            <HttpMethod value="PATCH" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/patch</TextBox>
          </Space>
        ),
        key: "0-1-5",
      },
      {
        title: (
          <Space>
            <HttpMethod value="OPTIONS" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/options</TextBox>
          </Space>
        ),
        key: "0-1-6",
      },
      {
        title: (
          <Space>
            <HttpMethod value="TRACE" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/trace</TextBox>
          </Space>
        ),
        key: "0-1-7",
      },
      {
        title: (
          <Space>
            <HttpMethod value="CONNECT" width={httpMethodWidth} type="tag" />
            <TextBox>/v1/home/connect</TextBox>
          </Space>
        ),
        key: "0-1-8",
      },
    ],
  },
];

const Page: React.FC = () => {
  const id = useId();

  return (
    <Container id={id}>
      <SiderSticky $width={280}>
        <Tree
          showLine
          switcherIcon={<DownOutlined />}
          defaultExpandAll
          treeData={treeData}
        />
      </SiderSticky>
      <FlushDivider type="vertical" $heightBlock />
      <Main>
        <div>
          <Space>
            <HttpMethod value="GET" />
            <TextBox>/v1/hello/get</TextBox>
          </Space>
        </div>
      </Main>
      <FloatButton.BackTop target={() => document.getElementById(id)!} />
    </Container>
  );
};

export default Page;
