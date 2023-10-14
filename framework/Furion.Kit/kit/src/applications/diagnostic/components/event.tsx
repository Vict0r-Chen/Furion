import { Alert } from "antd";
import React from "react";
import styled from "styled-components";
import Page from "./page";

const Container = styled(Page)``;

const Event: React.FC = () => {
  return (
    <Container>
      <Alert
        message="诊断器连接失败，请确保服务器已正常启动。"
        type="warning"
        showIcon
        closable
      />
      <br />
      诊断事件
    </Container>
  );
};

export default Event;
