import { Watermark } from "antd";
import { styled } from "styled-components";
import Content from "./content";
import Footer from "./footer";
import Header from "./header";
import Sider from "./sider";
import Toolbar from "./toolbar";

const Container = styled.div`
  display: flex;
  flex-direction: row;
  width: 100vw;
  height: 100vh;
`;

const Layout = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
`;

const ContentLayout = styled.div`
  display: flex;
  flex: 1;
  flex-direction: row;
`;

const Home: React.FC = () => {
  return (
    <Watermark content="Furion">
      <Container>
        <Sider />
        <Layout>
          <Header />
          <ContentLayout>
            <Content />
            <Toolbar />
          </ContentLayout>
          <Footer />
        </Layout>
      </Container>
    </Watermark>
  );
};

export default Home;
