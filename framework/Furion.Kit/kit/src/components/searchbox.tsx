import { Input, InputProps, InputRef, Space } from "antd";
import React, { useState } from "react";
import { styled } from "styled-components";
import IconFont from "./iconfont";

const Container = styled.div`
  box-sizing: border-box;
  display: inline-block;
`;

const defaultWidth = 180;

type SearchBoxProps = InputProps & React.RefAttributes<InputRef>;

const SearchBox: React.FC<SearchBoxProps> = (props) => {
  const [width, setWidth] = useState(defaultWidth);

  return (
    <Container>
      <Space align="center" size={0}>
        <Input
          placeholder="输入关键字搜索..."
          bordered={width !== defaultWidth}
          allowClear
          style={{ width }}
          onFocus={() => setWidth(250)}
          onBlur={() => setWidth(defaultWidth)}
          suffix={<IconFont type="icon-search" $size={20} $color="#8c8c8c" />}
          {...props}
        />
      </Space>
    </Container>
  );
};

export default SearchBox;
