import { Alert } from "antd";
import React, { useEffect } from "react";
import { styled } from "styled-components";
import RoutingItem from "./routing-item";

const Container = styled.div``;

const Routing: React.FC = () => {
  useEffect(() => {
    var eventSource = new EventSource("https://localhost:7115/furion/http-sse");

    eventSource.onopen = (event) => {
      console.log(event);
    };

    eventSource.onmessage = (event) => {
      console.log(event);
    };

    eventSource.onerror = (event) => {
      console.log(event);
    };

    return () => {
      console.log("eventsouce.onclose");
      eventSource.close();
    };
  }, []);

  return (
    <Container>
      <Alert
        message="诊断器连接失败，请确保服务器已正常启动。"
        type="warning"
        showIcon
        closable
      />
      <br />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="GET"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="POST"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="DELETE"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="PUT"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="HEAD"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="PATCH"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="OPTIONS"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="TRACE"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="CONNECT"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="GET"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="POST"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="DELETE"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="PUT"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="HEAD"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="PATCH"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="OPTIONS"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="TRACE"
      />
      <RoutingItem
        link="https://localhost:7115/furion/http-sse"
        httpMethod="CONNECT"
      />
    </Container>
  );
};

export default Routing;
