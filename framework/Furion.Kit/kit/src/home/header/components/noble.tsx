import { Space, Tag } from "antd";
import React from "react";
import { Link } from "react-router-dom";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";

const Container = styled.div`
  cursor: pointer;
`;

const Icon = styled(IconFont)`
  font-size: 16px;
  vertical-align: text-top;
`;

const Category = styled(TextBox)`
  font-weight: 500;
  display: inline-block;
  user-select: none;
`;

const Noble: React.FC = () => {
  return (
    <Container>
      <Link to="/equity">
        <Tag color="gold">
          <Space align="center" size={5}>
            <Icon type="icon-noble" />
            <Category>旗舰版</Category>
          </Space>
        </Tag>
      </Link>
    </Container>
  );
};

export default Noble;
