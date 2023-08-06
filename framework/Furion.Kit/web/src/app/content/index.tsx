import { Outlet } from "react-router-dom";
import { Container } from "./style";

export default function ContentPanel() {
  return (
    <Container>
      <Outlet />
    </Container>
  );
}