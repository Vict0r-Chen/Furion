import { Space } from "antd";
import React, { useState } from "react";
import styled from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";

const Container = styled.div`
  margin-bottom: 10px;

  &:last-child {
    margin-bottom: 0;
  }
`;

const Title = styled(Flexbox)`
  font-size: 15px;
  font-weight: 600;
  cursor: pointer;
  user-select: none;
  align-items: center;
`;

const Description = styled(TextBox)`
  font-weight: normal;
  font-size: 14px;
`;

const Main = styled.div`
  padding: 10px 0 0 0;
`;

interface RouteCategoryProps {
  title: React.ReactNode;
  description?: React.ReactNode;
  children?: React.ReactNode;
}

const RouteCategory: React.FC<RouteCategoryProps> = ({
  title,
  description,
  children,
}) => {
  const [expand, setExpand] = useState(true);

  return (
    <Container>
      <Title onClick={() => setExpand((d) => !d)} $spaceBetween>
        <Space size={6}>
          <IconFont
            type={expand ? "icon-arrow-open" : "icon-arrow-close"}
            $color="#002b36"
            $size={14}
          />
          <IconFont
            $size={16}
            type={expand ? "icon-folder-open" : "icon-folder-close"}
            $color="#4096ff"
          />
          <TextBox $color="#000000E0">{title}</TextBox>
          <Description $color="#00000073">{description}</Description>
        </Space>
      </Title>
      {expand && <Main>{children}</Main>}
    </Container>
  );
};

export default RouteCategory;
