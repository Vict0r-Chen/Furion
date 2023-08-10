import { Badge } from "antd";
import React from "react";
import { styled } from "styled-components";
import Header from "..";

const Container = styled.div``;

const NotificationBox: React.FC = () => {
  return (
    <Container>
      <Badge count={5} size="small">
        <Header.Icon type="icon-notification" />
      </Badge>
    </Container>
  );
};

export default NotificationBox;
