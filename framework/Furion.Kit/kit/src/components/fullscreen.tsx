import { FullscreenExitOutlined, FullscreenOutlined } from "@ant-design/icons";
import { Tooltip } from "antd";
import React, { MouseEventHandler } from "react";
import styled from "styled-components";

const Container = styled.div`
  display: inline-block;
  cursor: pointer;
`;

const FullscreenExit = styled(FullscreenExitOutlined)`
  color: #1677ff;
`;

interface FullscreenProps {
  fullscreen?: boolean;
  onClick?: MouseEventHandler<HTMLDivElement>;
}

const Fullscreen: React.FC<FullscreenProps> = ({
  fullscreen = false,
  onClick,
}) => {
  return (
    <Container onClick={onClick}>
      <Tooltip title={fullscreen ? "退出全屏" : "全屏"} placement="left">
        {fullscreen ? <FullscreenExit /> : <FullscreenOutlined />}
      </Tooltip>
    </Container>
  );
};

export default Fullscreen;
