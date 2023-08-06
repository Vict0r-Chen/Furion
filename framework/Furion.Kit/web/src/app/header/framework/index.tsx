import { Space } from "antd";
import { BranchIcon, Container, Icon, Version } from "./style";

export default function Framework() {
  return (
    <Container>
      <a href="https://furion.net" target="_blank" rel="noreferrer">
        <Space>
          <BranchIcon type="icon-branch" />
          <Version>Furion 5.0.0.preview.1</Version>
          <Icon type="icon-arrow-right" />
        </Space>
      </a>
    </Container>
  );
}
