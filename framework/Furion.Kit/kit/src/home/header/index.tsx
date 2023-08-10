import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import IconFont from "../../components/iconfont";
import SpaceBetween from "../../components/space-between";
import useSiderStore from "../../stores/sider.store";
import Noble from "./noble";
import NotificationBox from "./notification";
import SearchBox from "./searchbox";
import Version from "./version";

const Container = styled(SpaceBetween)`
  height: 35px;
  min-height: 35px;
  flex-direction: row;
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

const ClearFloatIcon = styled(HeaderIcon)`
  color: #001d66;
`;

const HeaderDefault: React.FC = () => {
  const [float, switchFloat] = useSiderStore((state) => [
    state.float,
    state.switchFloat,
  ]);

  return (
    <>
      <Container>
        <div>
          <Space size={15} align="center">
            <div onClick={switchFloat}>
              {float ? (
                <ClearFloatIcon type="icon-clear-float" />
              ) : (
                <HeaderIcon type="icon-float" />
              )}
            </div>

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
