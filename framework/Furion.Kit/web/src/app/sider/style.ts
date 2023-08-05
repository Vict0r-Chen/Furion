import { Layout } from "antd";
import { styled } from "styled-components";

const { Sider } = Layout;

const Container = styled(Sider)`
  background-color: rgb(247, 248, 251) !important;
  text-align: center;
`;

const Logo = styled.div`
  margin: 15px 0 0 0;
`;

const Line = styled.div`
  padding: 0 15px;
`;

const Main = styled.div`
  height: 100vh;
  display: flex;
  flex-direction: column;
`;

const Menus = styled.div`
  flex: 1;
`;

const Explore = styled.div`
  margin-bottom: 15px;
`;

export { Container, Explore, Line, Logo, Main, Menus };

