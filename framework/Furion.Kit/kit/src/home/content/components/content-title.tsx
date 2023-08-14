import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";

const Container = styled.div`
  position: sticky;
  top: 0;
  background-color: #ffffff;
  z-index: 10;
`;

const Main = styled(Flexbox)`
  align-items: center;
`;

const Title = styled.h3`
  color: rgba(0, 0, 0, 0.88);
  font-weight: 600;
  font-size: 24px;
  margin-right: 15px;
  user-select: none;
`;

const ExtraContainer = styled.div`
  position: relative;
  top: 2px;
`;

const Description = styled.div`
  font-size: 14px;
  color: #000000a6;
  position: relative;
  top: -14px;
  z-index: 1;
  user-select: none;
`;

interface ContentTitleProps {
  children?: React.ReactNode;
  extra?: React.ReactNode;
  description?: React.ReactNode;
  spaceBetween?: boolean;
}

const ContentTitle: React.FC<ContentTitleProps> = ({
  children,
  extra,
  description,
  spaceBetween,
}) => {
  return (
    <Container>
      <Main $spaceBetween={spaceBetween}>
        <Title>{children}</Title>
        <ExtraContainer>{extra}</ExtraContainer>
      </Main>
      {description && <Description>{description}</Description>}
    </Container>
  );
};

export default ContentTitle;
