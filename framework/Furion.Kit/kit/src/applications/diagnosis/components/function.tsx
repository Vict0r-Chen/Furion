import { Badge } from "antd";
import React, { MouseEventHandler } from "react";
import { css, styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import Upward from "../../../components/upward";

const Container = styled(Upward)<{ $disabled?: boolean }>`
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  width: calc(100% / 3);
  align-items: center;
  justify-content: center;
  padding: 10px;

  border-radius: 8px;

  ${(props) =>
    props.$disabled !== true
      ? css`
          cursor: pointer;
          color: #000000a6;

          &:hover {
            background-color: rgb(245, 245, 245);
            color: rgb(22, 119, 255);
          }
        `
      : css`
          cursor: not-allowed;
          color: rgba(0, 0, 0, 0.25);
          transform: translateY(0) !important;
        `}
`;

const Title = styled(TextBox)`
  margin-top: 5px;
  font-size: 14px;
  user-select: none;
`;

const Icon = styled(IconFont)<{ $disabled?: boolean }>`
  font-size: 30px;
  height: 32px;

  ${(props) =>
    props.$disabled === true &&
    css`
      color: rgba(0, 0, 0, 0.25);
    `}
`;

interface FunctionProps {
  title?: string;
  icon?: React.ReactNode;
  onClick?: MouseEventHandler<HTMLDivElement>;
  badge?: React.ReactNode;
  disabled?: boolean;
}

const Function: React.FC<FunctionProps> & {
  Icon: typeof Icon;
} = ({ title, icon, onClick, badge, disabled }) => {
  const functionIcon = React.isValidElement(icon)
    ? React.cloneElement<any>(icon, {
        $disabled: disabled,
      })
    : icon;

  return (
    <Container
      onClick={disabled === true ? undefined : onClick}
      $disabled={disabled}
    >
      <Badge count={badge} color="#d9d9d9" offset={[5, 3]}>
        {functionIcon}
      </Badge>
      <Title>{title}</Title>
    </Container>
  );
};

Function.Icon = Icon;

export default Function;
