import { Layout } from "antd";
import { styled } from "styled-components";

const { Sider } = Layout;

const Container = styled(Sider)`
  background-color: rgb(247, 248, 251)!important;
  display: flex;
  flex-direction: column;
  text-align: center;
`;

const Logo = styled.div`
  margin: 15px 0 0 0;
`;

const Line = styled.div`
  padding: 0 15px;
`;

export { Container, Line, Logo };

