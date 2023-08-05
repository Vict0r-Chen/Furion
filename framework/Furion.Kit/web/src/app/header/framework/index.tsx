import { Space } from "antd";
import { BranchIcon, Container, Icon, Version } from "./style";

export default function Framework() {
  return (
    <Container>
      <Space>
        <BranchIcon type="icon-branch" />
        <Version>Furion 5.0.0.preview.1</Version>
        <Icon type="icon-arrow-right" />
      </Space>
    </Container>
  );
}
