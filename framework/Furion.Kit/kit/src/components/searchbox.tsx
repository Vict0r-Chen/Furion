import { Input, InputProps, InputRef, Space } from "antd";
import React, { useState } from "react";
import styled from "styled-components";
import IconFont from "./iconfont";

const Container = styled.div`
  box-sizing: border-box;
  display: inline-block;
`;

type SearchBoxProps = InputProps &
  React.RefAttributes<InputRef> & {
    defaultWidth?: number | string;
    maxWidth?: number | string;
  };

const SearchBox: React.FC<SearchBoxProps> = ({
  defaultWidth = 180,
  maxWidth = 220,
  ...props
}) => {
  const [width, setWidth] = useState(defaultWidth);

  return (
    <Container>
      <Space align="center" size={0}>
        <Input
          placeholder="输入关键字搜索..."
          bordered={width !== defaultWidth}
          allowClear
          style={{ width }}
          onFocus={() => setWidth(maxWidth)}
          onBlur={() => setWidth(defaultWidth)}
          suffix={<IconFont type="icon-search" $size={20} $color="#8c8c8c" />}
          {...props}
        />
      </Space>
    </Container>
  );
};

export default SearchBox;
