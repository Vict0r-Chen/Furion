import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import Version from "./version";

const Container = styled.div`
  height: 35px;
  min-height: 35px;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
`;

const Header: React.FC = () => {
  return (
    <>
      <Container>
        <div></div>
        <Version number="Furion 5.0.0.preview.1" />
      </Container>
      <FlushDivider type="horizontal" $widthBlock />
    </>
  );
};

export default Header;
