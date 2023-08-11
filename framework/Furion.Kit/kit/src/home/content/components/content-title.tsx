import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";

const Container = styled(Flexbox)`
  align-items: center;
  position: sticky;
  top: 0;
  background-color: #fff;
`;

const Title = styled.h3`
  color: rgba(0, 0, 0, 0.88);
  font-weight: 600;
  font-size: 24px;
  margin-right: 15px;
`;

const ExtraContainer = styled.div`
  position: relative;
  top: 2px;
`;

interface ContentTitleProps {
  children?: React.ReactNode;
  extra?: React.ReactNode;
}

const ContentTitle: React.FC<ContentTitleProps> = ({ children, extra }) => {
  return (
    <Container>
      <Title>{children}</Title>
      <ExtraContainer>{extra}</ExtraContainer>
    </Container>
  );
};

export default ContentTitle;
