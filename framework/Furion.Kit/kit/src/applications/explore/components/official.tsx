import { Button, Space, message } from "antd";
import React, { useContext } from "react";
import { styled } from "styled-components";
import banner from "../../../assets/banner.png";
import banner1 from "../../../assets/banner1.jpeg";
import banner2 from "../../../assets/banner2.jpeg";
import banner3 from "../../../assets/banner3.png";
import CategoryList from "../../../components/category-list";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import ExploreContext from "../context";
import AppCard from "./app-card";
import Banner from "./banner";
import Main from "./main";

const Container = styled(Flexbox)`
  margin-top: 25px;
  margin-left: 10px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const Operation = styled.div`
  margin-top: 25px;
  text-align: center;
`;

const Official: React.FC = () => {
  const { showDrawer, listType } = useContext(ExploreContext);

  return (
    <Main>
      <Space direction="vertical" size={20}>
        <CategoryList
          title="分类"
          items={[
            { key: "1", label: "全部" },
            { key: "2", label: "电商" },
            { key: "3", label: "后台" },
            { key: "4", label: "视频" },
            { key: "5", label: "智能" },
            { key: "6", label: "工具" },
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
          type={listType}
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-panel" $size={26} />}
          banner={<Banner src={banner} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="输出"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-console" $size={26} />}
          banner={<Banner src={banner1} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="诊断"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-diagnosis" $size={26} />}
          banner={<Banner src={banner2} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="开放"
          description="在这里可以查看系统概况"
          classify="文档"
          logo={<IconFont type="icon-openapi" $size={26} />}
          banner={<Banner src={banner3} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="输出"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-console" $size={26} />}
          banner={<Banner src={banner1} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="诊断"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-diagnosis" $size={26} />}
          banner={<Banner src={banner2} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
        />
        <AppCard
          type={listType}
          title="开放"
          description="在这里可以查看系统概况"
          classify="文档"
          logo={<IconFont type="icon-openapi" $size={26} />}
          banner={<Banner src={banner3} alt="" />}
          onClick={() => showDrawer("panel")}
          installClick={() => message.success("安装成功")}
          install
        />
        <AppCard type={listType} skeleton />
        <AppCard type={listType} skeleton />
      </Container>
      <Operation>
        <Button>加载更多</Button>
      </Operation>
    </Main>
  );
};

export default Official;
