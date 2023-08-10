import { Divider, Tooltip } from "antd";
import React from "react";
import { NavLink } from "react-router-dom";
import { css, styled } from "styled-components";
import IconFont from "../../components/iconfont";
import Upward from "../../components/upward";

const iconActiveStyle = css`
  color: #001d66;
`;

const actionStyle = css`
  background-color: #fff;
  border: 1px solid #69b1ff;
  box-shadow: 0px 0px 3px #69b1ff;
`;

const Container = styled.div<{ $active?: boolean }>`
  width: 40px;
  height: 40px;
  display: flex;
  position: relative;
  box-sizing: border-box;
  justify-content: center;
  align-items: center;
  text-align: center;
  border-radius: 8px;
  border: 1px solid transparent;
  background-color: #e6f4ff;

  ${(props) => props.$active === true && actionStyle}

  &:hover {
    ${actionStyle}
  }

  &:hover svg {
    ${iconActiveStyle}
  }
`;

const DividerContainer = styled.div`
  padding: 0 8px;

  & > div {
    margin: 20px 0 5px 0;
  }
`;

export interface FunctionProps {
  render: React.ReactNode | ((isActive: boolean) => React.ReactNode);
  title?: string;
  link: string;
  style?: React.CSSProperties;
  divider?: boolean;
  position?: "top" | "bottom";
}

const FunctionDefault: React.FC<FunctionProps> = ({
  render,
  title,
  link,
  style,
  divider = false,
}) => {
  return (
    <>
      <NavLink to={link}>
        {({ isActive, isPending }) => (
          <Tooltip placement="right" title={title}>
            <Upward>
              <Container style={style} $active={isActive}>
                {typeof render === "function" ? render(isActive) : render}
              </Container>
            </Upward>
          </Tooltip>
        )}
      </NavLink>
      {divider && (
        <DividerContainer>
          <Divider />
        </DividerContainer>
      )}
    </>
  );
};

const FunctionIcon = styled(IconFont)<{ $active?: boolean }>`
  font-size: 24px;
  color: #000000e0;

  ${(props) => props.$active === true && iconActiveStyle}
`;

type FunctionComponent = typeof FunctionDefault & {
  Icon: typeof FunctionIcon;
};

const Function = FunctionDefault as FunctionComponent;
Function.Icon = FunctionIcon;

export default Function;
