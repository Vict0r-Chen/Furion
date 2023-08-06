import { Alert, Typography } from "antd";

const { Title } = Typography;

export default function Panel() {
  return (
    <div>
      <Title level={3}>面板</Title>
      <Alert
        message="Furion v5.0.0.Preview.1 版本发布啦！"
        type="success"
        showIcon
      />
    </div>
  );
}
