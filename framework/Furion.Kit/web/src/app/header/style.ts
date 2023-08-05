import { Layout } from "antd";
import { styled } from "styled-components";

const { Header } = Layout;

const Container = styled(Header)`
  height: 35px;
  background-color: #ffffff;
  line-height: 22px;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: space-between;
`;

export { Container };

