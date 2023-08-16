import { Button } from "antd";
import { styled } from "styled-components";
import IconFont from "../../components/iconfont";
import Content from "../../home/content";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const OpenAPI: React.FC = () => {
  return (
    <Content.Main>
      <Content.Title
        description="API 文档、在线调试、接口自动测试，等功能。"
        more={
          <Button type="primary" icon={<AddIcon type="icon-add" $size={16} />}>
            导入
          </Button>
        }
      >
        开放
      </Content.Title>
    </Content.Main>
  );
};

export default OpenAPI;
