import { Divider, Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "./flexbox";

const Container = styled(Flexbox)`
  align-items: center;
`;

const Title = styled.h5`
  color: rgba(0, 0, 0, 0.88);
  font-weight: 600;
  font-size: 16px;
  margin-right: 15px;
  user-select: none;
`;

const ExtraContainer = styled.div`
  position: relative;
`;

const Main = styled.div`
  font-size: 14px;
`;

interface CategoryProps {
  title?: React.ReactNode;
  children?: React.ReactNode;
  extra?: React.ReactNode;
  id?: string;
  icon?: React.ReactNode;
}

const Category: React.FC<CategoryProps> = ({
  title,
  children,
  extra,
  id,
  icon,
}) => {
  return (
    <>
      <Container id={id}>
        <Space>
          {icon}
          <Title>{title}</Title>
        </Space>
        <ExtraContainer>{extra}</ExtraContainer>
      </Container>
      <Main>{children}</Main>
      <Divider />
    </>
  );
};

export default Category;
