import { Tooltip } from "antd";
import React from "react";
import { NavLink } from "react-router-dom";
import { styled } from "styled-components";
import IconFont from "../../components/iconfont";

const Container = styled.div`
  width: 40px;
  height: 40px;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  border-radius: 8px;
  background-color: #e6f4ff;
`;

export interface FunctionProps {
  content: React.ReactNode;
  title?: string;
  link: string;
  style?: React.CSSProperties;
}

const FunctionDefault: React.FC<FunctionProps> = ({
  content,
  title,
  link,
  style,
}) => {
  return (
    <NavLink to={link}>
      {({ isActive, isPending }) => (
        <Container style={style}>
          <Tooltip placement="right" title={title}>
            {content}
          </Tooltip>
        </Container>
      )}
    </NavLink>
  );
};

const FunctionIcon = styled(IconFont)`
  font-size: 24px;
  color: #000000e0;
`;

type FunctionComponent = typeof FunctionDefault & {
  Icon: typeof FunctionIcon;
};

const Function = FunctionDefault as FunctionComponent;
Function.Icon = FunctionIcon;

export default Function;
