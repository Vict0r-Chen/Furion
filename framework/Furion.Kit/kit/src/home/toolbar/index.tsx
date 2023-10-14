import { Space } from "antd";
import React from "react";
import styled from "styled-components";
import Flexbox from "../../components/flexbox";
import { FlushDivider } from "../../components/flush-divider";
import ToolbarIcon from "./components/toolbar-icon";

const Container = styled(Flexbox)`
  width: 30px;
  min-width: 30px;
  text-align: center;
  align-items: center;
  padding: 15px 0;
`;

const Toolbar: React.FC = () => {
  return (
    <>
      <FlushDivider type="vertical" $heightBlock />
      <Container $spaceBetween direction="column">
        <div>
          <Space direction="vertical" size={20}>
            <ToolbarIcon
              type="icon-documentation"
              title="使用手册"
              link="https://furion.net"
            />
            <ToolbarIcon type="icon-language" title="中文 / English" />
          </Space>
        </div>
        <div></div>
      </Container>
    </>
  );
};

export default Toolbar;
