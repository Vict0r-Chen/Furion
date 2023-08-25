import { Button } from "antd";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import SearchBox from "../../../components/searchbox";
import SiderSticky from "../../../components/sider-sticky";
import RouteCategory from "./route-category";
import RouteItem from "./route-item";

const Directory = styled(SiderSticky)`
  height: 100%;
  overflow-y: auto;
`;

const PathList: React.FC = () => {
  return (
    <Directory $width={300}>
      <div
        style={{
          marginBottom: 15,
          position: "sticky",
          top: 0,
          backgroundColor: "#ffffff",
          zIndex: 2,
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
        }}
      >
        <SearchBox bordered defaultWidth={250} maxWidth={250} />
        <Button
          type="primary"
          size="small"
          icon={<IconFont type="icon-arrow-left" />}
        />
      </div>
      <RouteCategory title="Hello" description="测试接口">
        <RouteItem httpMethod="GET" path="/Furion/OpenApi" />
        <RouteItem httpMethod="POST" path="/Hello/Get/{id}" active />
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
  );
};

export default PathList;
