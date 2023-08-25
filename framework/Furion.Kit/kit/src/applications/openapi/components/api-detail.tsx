import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import Category from "../../../components/category";
import Flexbox from "../../../components/flexbox";
import HttpMethod from "../../../components/http-method";
import TextBox from "../../../components/textbox";

const Container = styled.div``;

const TitleContainer = styled.div``;

const Path = styled(TextBox)`
  font-size: 18px;
  letter-spacing: 0.5px;
  font-weight: 600;
`;

const ApiDetail: React.FC = () => {
  return (
    <Container>
      <TitleContainer>
        <Space size={15}>
          <HttpMethod value="POST" fontSize={18} />
          <Path>{"/Hello/Get/{id}"}</Path>
        </Space>
      </TitleContainer>
      <br />
      <br />
      <Flexbox>
        <div style={{ flex: 1, height: 300 }}>
          <Category title="接口信息"></Category>
        </div>
        <div style={{ flex: 1 }}>
          <Category title="响应数据"></Category>
        </div>
      </Flexbox>
      <Category title="请求参数"></Category>
      <Category title="请求信息"></Category>
      <Category title="请求代码"></Category>
      <Category title="响应码"></Category>
      <Category title="响应主体"></Category>
      <Category title="响应头部"></Category>
    </Container>
  );
};

export default ApiDetail;
