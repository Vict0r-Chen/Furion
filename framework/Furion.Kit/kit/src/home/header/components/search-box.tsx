import { Input, Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import Header from "..";

const Container = styled.div`
  box-sizing: border-box;
`;

const InputBox = styled(Input)`
  width: 180px;
`;

const SearchBox: React.FC = () => {
  return (
    <Container>
      <Space align="center" size={0}>
        <InputBox
          placeholder="输入关键字搜索..."
          bordered={false}
          allowClear
          suffix={<Header.Icon type="icon-search" />}
        />
      </Space>
    </Container>
  );
};

export default SearchBox;
