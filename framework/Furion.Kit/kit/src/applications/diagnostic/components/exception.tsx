import React from "react";
import { styled } from "styled-components";
import Page from "./page";

const Container = styled(Page)``;

const Exception: React.FC = () => {
  return <Container>系统异常</Container>;
};

export default Exception;
