import { Typography } from "antd";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";

const { Text } = Typography;

const Container = styled.div`
  height: 35px;
  background-color: #e6f4ff;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0 15px;
  position: relative;
  cursor: pointer;

  &::before {
    content: "";
    position: absolute;
    top: 0;
    left: -20px;
    width: 100%;
    height: 100%;
    transform: skew(45deg);
    background-color: #e6f4ff;
  }
`;

const Version = styled(Text)`
  z-index: 1;
  position: relative;
  font-weight: 500;
  color: #001d66;
`;

const Icon = styled(IconFont)`
  z-index: 1;
  position: relative;
  font-size: 12px;
  color: #001d66;
`;

const BranchIcon = styled(Icon)`
  font-size: 14px;
`;

export { BranchIcon, Container, Icon, Version };

