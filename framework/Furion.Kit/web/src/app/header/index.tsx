import { ThunderboltOutlined } from "@ant-design/icons";
import { Space, Tag } from "antd";
import Framework from "../header/framework";
import { Container } from "./style";

export default function HeaderPanel() {
  return (
    <Container>
      <div>
        <Space size={[0, 8]} wrap>
          <Tag icon={<ThunderboltOutlined />} color="#f50">
            旗舰版
          </Tag>
        </Space>
      </div>
      <Framework />
    </Container>
  );
}