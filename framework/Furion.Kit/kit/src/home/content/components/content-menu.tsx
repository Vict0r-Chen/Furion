import React, { MouseEventHandler } from "react";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";

const Container = styled.div``;

const Icon = styled(IconFont)`
  font-size: 24px;
  color: #8c8c8c;
  cursor: pointer;

  &:hover {
    color: #1677ff;
  }
`;

interface ContentMenuProps {
  onClick?: MouseEventHandler<HTMLDivElement>;
}

const ContentMenu: React.FC<ContentMenuProps> = ({ onClick }) => {
  return (
    <Container onClick={onClick}>
      <Icon type="icon-more-menu" />
    </Container>
  );
};

export default ContentMenu;
