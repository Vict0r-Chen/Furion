import { Watermark } from "antd";
import { styled } from "styled-components";
import Sider from "./sider";

const Container = styled.div`
  display: flex;
  flex-direction: row;
  width: 100vw;
  height: 100vh;
`;

const Home: React.FC = () => {
  return (
    <Watermark content="Furion">
      <Container>
        <Sider />
      </Container>
    </Watermark>
  );
};

export default Home;
