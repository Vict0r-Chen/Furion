import { Space } from "antd";
import React from "react";
import { css, styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import HttpMethod from "../../../components/http-method";
import TextBox from "../../../components/textbox";

const Container = styled(Flexbox)<{ $active?: boolean }>`
  align-items: center;
  cursor: pointer;
  border-radius: 3px;
  padding: 3px 10px 3px 22px;
  margin-bottom: 10px;
  color: #000000e0;
  letter-spacing: 0.5px;
  user-select: none;

  &:last-child {
    margin-bottom: 0;
  }

  &:hover {
    background-color: #e6f4ff;
  }

  ${(props) =>
    props.$active === true &&
    css`
      background-color: #e6f4ff;
      color: #1677ff;
    `}
`;

const Path = styled(TextBox)`
  &:hover {
    text-decoration: underline;
  }
`;

interface RouteItemProps {
  httpMethod: string;
  path: string;
  active?: boolean;
}

const RouteItem: React.FC<RouteItemProps> = ({
  httpMethod,
  path,
  active = false,
}) => {
  return (
    <Container $spaceBetween $active={active}>
      <Space align="center" size={15}>
        <HttpMethod value={httpMethod} />
        <Path
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
