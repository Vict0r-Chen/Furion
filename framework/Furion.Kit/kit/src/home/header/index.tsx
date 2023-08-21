import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../components/flexbox";
import { FlushDivider } from "../../components/flush-divider";
import SearchBox from "../../components/searchbox";
import HeaderIcon from "./components/header-icon";
import Noble from "./components/noble";
import NotificationBox from "./components/notification";
import Version from "./components/version";

const Container = styled(Flexbox)`
  height: 35px;
  min-height: 35px;
  align-items: center;
  padding-left: 15px;
`;

const HomeIcon = styled(HeaderIcon)`
  margin-right: 10px;
`;

const Header: React.FC & { Icon: typeof HeaderIcon } = () => {
  return (
    <>
      <Container $spaceBetween>
        <div>
          <Space size={15} align="center">
            <Noble />
          </Space>
        </div>
        <div>
          <Space size={15}>
            <SearchBox />
            <a href="/" target="_blank">
              <HomeIcon type="icon-home" />
            </a>
            <NotificationBox />
            <Version number="Furion 5.0.0" />
          </Space>
        </div>
      </Container>
      <FlushDivider type="horizontal" $widthBlock />
    </>
  );
};

Header.Icon = HeaderIcon;

export default Header;
