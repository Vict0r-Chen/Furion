import { Layout } from "antd";
import { styled } from "styled-components";

const { Sider } = Layout;

const Container = styled(Sider)`
  background-color: #ffffff !important;
  text-align: center;
`;

const Main = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
  box-sizing: border-box;
  padding: 15px 0;
`;

const Menus = styled.div`
  flex: 1;
`;

export { Container, Main, Menus };

