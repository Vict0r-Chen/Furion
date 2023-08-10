import { styled } from "styled-components";
import logo from "../../assets/logo.png";

const Container = styled.div`
  margin-bottom: 40px;
`;

const Img = styled.img`
  width: 36px;
  display: block;
  margin: 0 auto;
`;

const Logo: React.FC = () => (
  <Container>
    <a href="/">
      <Img src={logo} alt="Furion Kit Log" />
    </a>
  </Container>
);

export default Logo;
