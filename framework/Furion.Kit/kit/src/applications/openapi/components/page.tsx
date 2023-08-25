import { FloatButton, Space } from "antd";
import React, { useId } from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import { FlushDivider } from "../../../components/flush-divider";
import HttpMethod from "../../../components/http-method";
import SearchBox from "../../../components/searchbox";
import SiderSticky from "../../../components/sider-sticky";
import TextBox from "../../../components/textbox";
import RouteCategory from "./route-category";
import RouteItem from "./route-item";

const Container = styled(Flexbox)`
  overflow-y: auto;
  height: calc(100vh - 242px);
  align-items: flex-start;
`;

const Directory = styled(SiderSticky)`
  height: 100%;
  overflow-y: auto;
`;

const Main = styled.div`
  flex: 1;
  box-sizing: border-box;
  padding: 0 15px;
`;

const Page: React.FC = () => {
  const id = useId();

  return (
    <Container id={id}>
      <Directory $width={300}>
        <div
          style={{
            marginBottom: 15,
            position: "sticky",
            top: 0,
            backgroundColor: "#ffffff",
            zIndex: 2,
          }}
        >
          <SearchBox bordered defaultWidth={300} maxWidth={300} />
        </div>
        <RouteCategory title="Hello" description="测试接口">
          <RouteItem httpMethod="GET" path="/Furion/OpenApi" />
          <RouteItem httpMethod="POST" path="/Hello/Get" active />
          <RouteItem httpMethod="DELETE" path="/Hello/Post" />
          <RouteItem httpMethod="PUT" path="/Hello/TestRetryPolicy" />
          <RouteItem httpMethod="HEAD" path="/Hello/TestFallbackPolicy" />
          <RouteItem httpMethod="PATCH" path="/Hello/TestTimeoutPolicy" />
          <RouteItem httpMethod="OPTIONS" path="/Hello/TestThrow" />
          <RouteItem httpMethod="TRACE" path="/Hello/TestThrow" />
          <RouteItem httpMethod="CONNECT" path="/Hello/TestThrow" />
        </RouteCategory>
        <RouteCategory title="Furion.Tests" description="最小 API">
          <RouteItem httpMethod="GET" path="/furion/openapi" />
          <RouteItem httpMethod="POST" path="/Hello/Get" />
          <RouteItem httpMethod="DELETE" path="/Hello/Post" />
          <RouteItem httpMethod="PUT" path="/Hello/TestRetryPolicy" />
          <RouteItem httpMethod="HEAD" path="/Hello/TestFallbackPolicy" />
          <RouteItem httpMethod="PATCH" path="/Hello/TestTimeoutPolicy" />
          <RouteItem httpMethod="OPTIONS" path="/Hello/TestThrow" />
          <RouteItem httpMethod="TRACE" path="/Hello/TestThrow" />
          <RouteItem httpMethod="CONNECT" path="/Hello/TestThrow" />
        </RouteCategory>
        <RouteCategory title="User Management" description="用户管理">
          <RouteItem httpMethod="GET" path="/furion/openapi" />
          <RouteItem httpMethod="POST" path="/Hello/Get" />
          <RouteItem httpMethod="DELETE" path="/Hello/Post" />
          <RouteItem httpMethod="PUT" path="/Hello/TestRetryPolicy" />
          <RouteItem httpMethod="HEAD" path="/Hello/TestFallbackPolicy" />
          <RouteItem httpMethod="PATCH" path="/Hello/TestTimeoutPolicy" />
          <RouteItem httpMethod="OPTIONS" path="/Hello/TestThrow" />
          <RouteItem httpMethod="TRACE" path="/Hello/TestThrow" />
          <RouteItem httpMethod="CONNECT" path="/Hello/TestThrow" />
        </RouteCategory>
      </Directory>
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
