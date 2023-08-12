import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import banner from "../../assets/banner.png";
import banner1 from "../../assets/banner1.jpeg";
import banner2 from "../../assets/banner2.jpeg";
import banner3 from "../../assets/banner3.png";
import CategoryList from "../../components/category-list";
import Flexbox from "../../components/flexbox";
import IconFont from "../../components/iconfont";
import AppCard from "./components/app-card";
import Banner from "./components/banner";
import Main from "./components/main";

const Container = styled(Flexbox)`
  margin-top: 25px;
  margin-left: 10px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const Official: React.FC = () => {
  return (
    <Main>
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
      <Container>
        <AppCard
          title="面板"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-panel" $size={26} />}
          banner={<Banner src={banner} alt="" />}
        />
        <AppCard
          title="输出"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-console" $size={26} />}
          banner={<Banner src={banner1} alt="" />}
        />
        <AppCard
          title="诊断"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-diagnosis" $size={26} />}
          banner={<Banner src={banner2} alt="" />}
        />
        <AppCard
          title="开放"
          description="在这里可以查看系统概况"
          classify="文档"
          logo={<IconFont type="icon-openapi" $size={26} />}
          banner={<Banner src={banner3} alt="" />}
        />
        <AppCard
          title="输出"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-console" $size={26} />}
          banner={<Banner src={banner1} alt="" />}
        />
        <AppCard
          title="诊断"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-diagnosis" $size={26} />}
          banner={<Banner src={banner2} alt="" />}
        />
        <AppCard
          title="开放"
          description="在这里可以查看系统概况"
          classify="文档"
          logo={<IconFont type="icon-openapi" $size={26} />}
          banner={<Banner src={banner3} alt="" />}
        />
        <AppCard
          title="面板"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-panel" $size={26} />}
          banner={<Banner src={banner} alt="" />}
          install
        />
      </Container>
    </Main>
  );
};

export default Official;
