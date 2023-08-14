import { Button, Popover, Segmented } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../components/flexbox";
import { FlushDivider } from "../../components/flush-divider";
import IconFont from "../../components/iconfont";
import Content from "../../home/content";
import Function from "./components/function";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const Container = styled.div``;

const Classify = styled.div`
  text-align: center;
`;

const NewContainer = styled(Flexbox)`
  width: 270px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const NewList: React.FC = () => {
  return (
    <NewContainer>
      <Function title="请求侦听" icon={<Function.Icon type="icon-http" />} />
      <Function
        title="终点路由"
        icon={<Function.Icon type="icon-endpoint" />}
      />
      <Function title="控制台" icon={<Function.Icon type="icon-console" />} />
      <FlushDivider $size={10} />
      <Function title="自定义" icon={<Function.Icon type="icon-customize" />} />
    </NewContainer>
  );
};

const Diagnosis: React.FC = () => {
  return (
    <Content.Main>
      <Content.Title
        description="添加请求、控制台、配置，等诊断信息。"
        more={
          <Popover
            placement="bottomRight"
            content={<NewList />}
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
      <Container>
        <Classify>
          <Segmented options={["请求", "路由", "控制台"]} />
        </Classify>
      </Container>
    </Content.Main>
  );
};

export default Diagnosis;
