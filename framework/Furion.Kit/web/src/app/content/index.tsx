import { Alert } from "antd";
import { Container } from "./style";

export default function ContentPanel() {
  return (
    <Container>
      <Alert
        message="Furion v5.0.0.Preview.1 版本发布啦！"
        type="success"
        showIcon
      />
    </Container>
  );
}
