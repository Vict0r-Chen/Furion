import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";

const Container = styled.div`
  height: 30px;
  min-height: 30px;
`;

const Footer: React.FC = () => {
  return (
    <>
      <FlushDivider type="horizontal" $widthBlock />
      <Container>尾部</Container>
    </>
  );
};

export default Footer;
