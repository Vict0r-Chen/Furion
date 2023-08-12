import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import banner from "../../assets/banner.png";
import CategoryList from "../../components/category-list";
import Flexbox from "../../components/flexbox";
import IconFont from "../../components/iconfont";
import AppCard from "./components/app-card";
import Main from "./components/main";

const Container = styled(Flexbox)`
  margin-top: 25px;
  margin-left: 10px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const Banner = styled.img`
  height: 100%;
  width: 100%;
  display: block;
`;

const Local: React.FC = () => {
  return (
    <Main>
      <Space direction="vertical" size={20}>
        <CategoryList
          title="分类"
          items={[
            { key: "1", label: "全部" },
            { key: "2", label: "工具" },
            { key: "3", label: "文档" },
          ]}
        />
      </Space>
      <Container>
        <AppCard
          title="面板"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-panel" $size={26} />}
          banner={<Banner src={banner} alt="" />}
        />
      </Container>
    </Main>
  );
};

export default Local;
