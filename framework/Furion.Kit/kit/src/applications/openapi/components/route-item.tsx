import { Space, Tooltip } from "antd";
import React from "react";
import { css, styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import HttpMethod from "../../../components/http-method";
import IconFont from "../../../components/iconfont";
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

const AnonymousIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

interface RouteItemProps {
  httpMethod: string;
  path: string;
  active?: boolean;
  anonymous?: boolean;
}

const RouteItem: React.FC<RouteItemProps> = ({
  httpMethod,
  path,
  active = false,
  anonymous = false,
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
        {anonymous && (
          <Tooltip title="允许匿名访问">
            <AnonymousIcon $size={16} type="icon-anonymous" $color="#faad14" />
          </Tooltip>
        )}
      </Space>
    </Container>
  );
};

export default RouteItem;
