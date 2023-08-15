import { Alert, Space } from "antd";
import React, { useEffect } from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import HttpMethod from "./httpmethod";

const Container = styled.div``;

const ItemContainer = styled.div``;

const Item: React.FC = () => {
  return (
    <ItemContainer>
      <Flexbox
        style={{
          backgroundColor: "rgb(247, 248, 251)",
          alignItems: "center",
          justifyContent: "space-between",
          padding: "2px 5px",
        }}
      >
        <Space>
          <IconFont type="icon-arrow-right" />
          <HttpMethod value="GET" />
          <TextBox underline>https://localhost:7115/furion/http-sse</TextBox>
          <TextBox $color="red">200 OK</TextBox>
          <TextBox>HTTP SSE 请求</TextBox>
        </Space>
        <Space>100ms</Space>
      </Flexbox>
      <Flexbox style={{ padding: "0 15px" }}>
        <ReactJson
          collapsed
          src={{
            name: "Furion",
            age: 31,
            version: "0.0.1",
          }}
        />
      </Flexbox>
    </ItemContainer>
  );
};

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
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
      <Item />
    </Container>
  );
};

export default Routing;
