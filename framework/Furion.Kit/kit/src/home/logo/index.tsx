import { Link } from "react-router-dom";
import styled from "styled-components";
import logo from "../../assets/logo.png";

const Container = styled.div`
  position: absolute;
  z-index: 1;
`;

const Img = styled.img`
  width: 36px;
  display: block;
  margin: 0 auto;
  user-select: none;
`;

const Logo: React.FC = () => (
  <Container>
    <Link to="/">
      <Img src={logo} alt="Furion Kit Log" />
    </Link>
  </Container>
);

export default Logo;
