import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import CategoryList from "../../components/category-list";
import Main from "./components/main";

const Container = styled.div``;

const Official: React.FC = () => {
  return (
    <Main>
      <Container>
        <Space direction="vertical" size={20}>
          <CategoryList
            title="分类"
            items={[
              { key: "1", label: "全部" },
              { key: "2", label: "视频" },
              { key: "3", label: "智能" },
              { key: "4", label: "工具" },
            ]}
          />
          <CategoryList
            title="综合"
            items={[
              { key: "1", label: "全部" },
              { key: "2", label: "最热" },
              { key: "3", label: "最新" },
            ]}
          />
        </Space>
      </Container>
    </Main>
  );
};

export default Official;
