import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";

const Container = styled(Flexbox)`
  align-items: center;
  padding: 0 40px;
`;

const Title = styled.h3`
  color: rgba(0, 0, 0, 0.88);
  font-weight: 600;
  font-size: 24px;
`;

interface ContentTitleProps {
  children?: React.ReactNode;
  extra?: React.ReactNode;
}

const ContentTitle: React.FC<ContentTitleProps> = ({ children, extra }) => {
  return (
    <Container $spaceBetween>
      <Title>{children}</Title>
      {extra}
    </Container>
  );
};

export default ContentTitle;
