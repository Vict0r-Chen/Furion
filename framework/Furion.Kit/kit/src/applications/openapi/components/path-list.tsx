import { Button } from "antd";
import { useContext } from "react";
import styled from "styled-components";
import IconFont from "../../../components/iconfont";
import SearchBox from "../../../components/searchbox";
import SiderSticky from "../../../components/sider-sticky";
import { OpenApiGroup } from "../../../databases/types/openapi";
import OpenApiGroupContext from "../contexts";
import RouteCategory from "./route-category";
import RouteItem from "./route-item";

const Directory = styled(SiderSticky)`
  height: 100%;
  overflow-y: auto;
`;

const PathList: React.FC<OpenApiGroup> = (group) => {
  const { setApiDescription } = useContext(OpenApiGroupContext);

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
        <SearchBox bordered defaultWidth={260} maxWidth={260} />
        <Button
          type="primary"
          size="small"
          icon={<IconFont type="icon-arrow-left" />}
        />
      </div>
      {group.tags?.map((item) => (
        <RouteCategory key={item.name} title={item.name}>
          {item.descriptions?.map((desc) => (
            <RouteItem
              key={desc.id}
              httpMethod={desc.httpMethod!}
              path={desc.relativePath!}
              anonymous={desc.allowAnonymous}
              onClick={() => setApiDescription(desc)}
            />
          ))}
        </RouteCategory>
      ))}
    </Directory>
  );
};

export default PathList;
