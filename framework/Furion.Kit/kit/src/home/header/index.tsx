import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import Flexbox from "../../components/flexbox";
import IconFont from "../../components/iconfont";
import Noble from "./noble";
import NotificationBox from "./notification";
import SearchBox from "./searchbox";
import Version from "./version";

const Container = styled(Flexbox)`
  height: 35px;
  min-height: 35px;
  align-items: center;
  padding-left: 15px;
`;

const HeaderIcon = styled(IconFont)`
  font-size: 20px;
  color: #8c8c8c;
  cursor: pointer;

  &:hover {
    color: #69b1ff;
  }
`;

const HeaderDefault: React.FC = () => {
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
            <NotificationBox />
            <Version number="Furion 5.0.0.preview.1" />
          </Space>
        </div>
      </Container>
      <FlushDivider type="horizontal" $widthBlock />
    </>
  );
};

type HeaderComponent = typeof HeaderDefault & {
  Icon: typeof HeaderIcon;
};

const Header = HeaderDefault as HeaderComponent;
Header.Icon = HeaderIcon;

export default Header;
