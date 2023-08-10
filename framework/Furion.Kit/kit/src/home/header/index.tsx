import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";

const Container = styled.div`
  height: 35px;
  min-height: 35px;
`;

const Header: React.FC = () => {
  return (
    <>
      <Container>头部</Container>
      <FlushDivider type="horizontal" $widthBlock />
    </>
  );
};

export default Header;
