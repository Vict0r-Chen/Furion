import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";

const Container = styled.div`
  width: 30px;
  min-width: 30px;
`;

const Toolbar: React.FC = () => {
  return (
    <>
      <FlushDivider type="vertical" $heightBlock />
      <Container></Container>
    </>
  );
};

export default Toolbar;
