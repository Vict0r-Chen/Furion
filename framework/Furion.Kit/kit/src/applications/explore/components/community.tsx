import { Empty } from "antd";
import React from "react";
import { styled } from "styled-components";
import Main from "./main";

const Container = styled.div``;

const Community: React.FC = () => {
  return (
    <Main>
      <Container>
        <Empty
          image={Empty.PRESENTED_IMAGE_SIMPLE}
          description="更多社区应用集成中..."
        />
      </Container>
    </Main>
  );
};

export default Community;
