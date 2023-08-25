import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import HttpMethod from "../../../components/http-method";
import TextBox from "../../../components/textbox";

const Container = styled(Flexbox)`
  align-items: center;
  cursor: pointer;
  border-radius: 3px;
  padding: 2px 10px 2px 20px;
  margin-bottom: 10px;

  &:last-child {
    margin-bottom: 0;
  }

  &:hover {
    background-color: #e6f4ff;
  }
`;

const Path = styled(TextBox)`
  font-size: 14px;
  cursor: pointer;
  user-select: none;

  &:hover {
    color: #1677ff;
  }
`;

interface RouteItemProps {
  httpMethod: string;
  path: string;
}

const RouteItem: React.FC<RouteItemProps> = ({ httpMethod, path }) => {
  return (
    <Container $spaceBetween>
      <Space align="center">
        <HttpMethod value={httpMethod} width={70} />
        <Path
          $color="#000000e0"
          copyable={{
            tooltips: ["复制", "复制成功"],
          }}
        >
          {path}
        </Path>
      </Space>
    </Container>
  );
};

export default RouteItem;
