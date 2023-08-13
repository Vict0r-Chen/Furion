import React from "react";
import { styled } from "styled-components";
import Mdx from "../../mdxs/index.mdx";

const Container = styled.div``;

const Detail: React.FC = () => {
  return (
    <Container>
      <Mdx />
    </Container>
  );
};

export default Detail;
