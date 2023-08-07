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
  overflow-y: auto;
  padding: 3px 0;
`;

const Explore = styled.div`
  margin-top: 15px;
  margin-bottom: 15px;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
`;

export { Container, Explore, Line, Logo, Main, Menus };

