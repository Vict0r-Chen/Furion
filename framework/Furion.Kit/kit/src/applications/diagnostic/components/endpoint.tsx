import { Alert } from "antd";
import React, { useEffect } from "react";
import { styled } from "styled-components";
import EndpointItem from "./endpoint-item";

const Container = styled.div``;

const ItemContainer = styled.div`
  padding: 15px 0;
`;

const Endpoint: React.FC = () => {
  useEffect(() => {
    var eventSource = new EventSource(
      "https://localhost:7115/furion/endpoint-sse"
    );

    eventSource.onopen = (event) => {
      console.log(event);
    };

    eventSource.onmessage = (event) => {
      console.log(event);
      console.log(JSON.parse(event.data));
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
      <ItemContainer>
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="GET"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="POST"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="DELETE"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="PUT"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="HEAD"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="PATCH"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="OPTIONS"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="TRACE"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="CONNECT"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="GET"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="POST"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="DELETE"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="PUT"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="HEAD"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="PATCH"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="OPTIONS"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="TRACE"
        />
        <EndpointItem
          link="https://localhost:7115/furion/http-sse"
          httpMethod="CONNECT"
        />
      </ItemContainer>
    </Container>
  );
};

export default Endpoint;
