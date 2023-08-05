import { Layout, Typography } from "antd";
import { styled } from "styled-components";

const { Text } = Typography;
const { Footer } = Layout;

const Container = styled(Footer)`
  height: 30px;
  background-color: #ffffff;
  padding: 0 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Powerby = styled(Text)`
  color: #000000a6;
  font-size: 12px;
`;

export { Container, Powerby };

