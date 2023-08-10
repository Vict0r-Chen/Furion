import { Link } from "react-router-dom";
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
    <Link to="/">
      <Img src={logo} alt="Furion Kit Log" />
    </Link>
  </Container>
);

export default Logo;
