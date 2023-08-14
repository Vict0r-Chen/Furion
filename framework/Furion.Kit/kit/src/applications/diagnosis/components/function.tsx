import React, { MouseEventHandler } from "react";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import Upward from "../../../components/upward";

const Container = styled(Upward)`
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  width: calc(100% / 3);
  align-items: center;
  justify-content: center;
  padding: 10px;
  cursor: pointer;
  border-radius: 8px;
  color: #000000a6;

  &:hover {
    background-color: rgb(245, 245, 245);
    color: rgb(22, 119, 255);
  }
`;

const Title = styled(TextBox)`
  margin-top: 5px;
  font-size: 14px;
`;

const Icon = styled(IconFont)`
  font-size: 30px;
`;

interface FunctionProps {
  title?: string;
  icon?: React.ReactNode;
  onClick?: MouseEventHandler<HTMLDivElement>;
}

const Function: React.FC<FunctionProps> & {
  Icon: typeof Icon;
} = ({ title, icon, onClick }) => {
  return (
    <Container onClick={onClick}>
      {icon}
      <Title>{title}</Title>
    </Container>
  );
};

Function.Icon = Icon;

export default Function;
