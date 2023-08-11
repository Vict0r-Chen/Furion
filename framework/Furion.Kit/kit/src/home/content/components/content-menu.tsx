import { Dropdown, DropdownProps } from "antd";
import React from "react";
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

const ContentMenu: React.FC<DropdownProps> = (props) => {
  return (
    <Dropdown trigger={["click"]} placement="bottomRight" {...props}>
      <Container>
        <Icon type="icon-more-menu" />
      </Container>
    </Dropdown>
  );
};

export default ContentMenu;
