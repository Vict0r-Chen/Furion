import { Tooltip } from "antd";
import React from "react";
import styled from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import useSiderStore from "../stores/store";

const FloatContainer = styled(Flexbox)`
  height: 31px;
  min-height: 31px;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
  border-top: 1px solid rgb(240, 240, 240);
  width: 58px;
  background-color: #e6f4ff;
`;

const FloatIcon = styled(IconFont)`
  font-size: 18px;
  color: #8c8c8c;
  cursor: pointer;
  height: 22px;
  width: 22px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;

  &:hover {
    color: #69b1ff;
  }
`;

const ClearFloatIcon = styled(FloatIcon)`
  color: #4096ff;
  background-color: #bae0ff;

  &:hover {
    color: #1677ff;
  }
`;

const FloatButton: React.FC = () => {
  const [float, switchFloat] = useSiderStore((state) => [
    state.float,
    state.switchFloat,
  ]);

  return (
    <FloatContainer direction="column">
      <Tooltip title={float ? "吸附" : "浮动"}>
        {float ? (
          <ClearFloatIcon type="icon-clear-float" onClick={switchFloat} />
        ) : (
          <FloatIcon type="icon-float" onClick={switchFloat} />
        )}
      </Tooltip>
    </FloatContainer>
  );
};

export default FloatButton;
