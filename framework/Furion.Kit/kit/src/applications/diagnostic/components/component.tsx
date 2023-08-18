import React from "react";
import { styled } from "styled-components";
import Page from "./page";

const Container = styled(Page)``;

const Component: React.FC = () => {
  return <Container>组件依赖</Container>;
};

export default Component;
