import { Button, Popover } from "antd";
import { styled } from "styled-components";
import IconFont from "../../components/iconfont";
import Content from "../../home/content";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const Diagnosis: React.FC = () => {
  return (
    <Content.Main>
      <Content.Title
        description="添加请求、日志、配置，等诊断信息。"
        more={
          <Popover
            placement="bottomRight"
            content={<div style={{ width: 300, height: 300 }}>内容</div>}
            trigger="click"
          >
            <Button
              type="primary"
              icon={<AddIcon type="icon-add" $size={16} />}
            >
              新建
            </Button>
          </Popover>
        }
      >
        诊断
      </Content.Title>
    </Content.Main>
  );
};

export default Diagnosis;
