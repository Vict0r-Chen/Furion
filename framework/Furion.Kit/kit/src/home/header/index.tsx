import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import IconFont from "../../components/iconfont";
import Noble from "./noble";
import NotificationBox from "./notification";
import SearchBox from "./searchbox";
import Version from "./version";

const Container = styled.div`
  height: 35px;
  min-height: 35px;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  padding-left: 15px;
`;

const HeaderDefault: React.FC = () => {
  return (
    <>
      <Container>
        <div>
          <Space size={15}>
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

const HeaderIcon = styled(IconFont)`
  font-size: 20px;
  color: #8c8c8c;
  cursor: pointer;

  &:hover {
    color: #000000e0;
  }
`;

type HeaderComponent = typeof HeaderDefault & {
  Icon: typeof HeaderIcon;
};

const Header = HeaderDefault as HeaderComponent;
Header.Icon = HeaderIcon;

export default Header;
