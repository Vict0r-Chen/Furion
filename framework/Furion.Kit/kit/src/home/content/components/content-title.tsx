import { Typography } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";

const { Title } = Typography;

const Container = styled(Flexbox)`
  align-items: center;
  padding: 0 40px;
`;

interface ContentTitleProps {
  children?: React.ReactNode;
  extra?: React.ReactNode;
}

const ContentTitle: React.FC<ContentTitleProps> = ({ children, extra }) => {
  return (
    <Container $spaceBetween>
      <Title level={3}>{children}</Title>
      {extra}
    </Container>
  );
};

export default ContentTitle;
