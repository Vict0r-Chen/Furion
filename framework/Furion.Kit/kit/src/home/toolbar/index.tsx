import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import Flexbox from "../../components/flexbox";

const Container = styled(Flexbox)`
  width: 30px;
  min-width: 30px;
  text-align: center;
  align-items: center;
  padding: 15px 0;
`;

const Toolbar: React.FC = () => {
  return (
    <>
      <FlushDivider type="vertical" $heightBlock />
      <Container $spaceBetween direction="column"></Container>
    </>
  );
};

export default Toolbar;
