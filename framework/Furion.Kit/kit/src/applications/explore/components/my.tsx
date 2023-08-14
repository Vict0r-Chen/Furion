import React, { useContext } from "react";
import { styled } from "styled-components";
import banner from "../../../assets/banner.png";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import ExploreContext from "../context";
import AppCard from "./app-card";
import Banner from "./banner";
import Main from "./main";

const Container = styled(Flexbox)`
  margin-left: 10px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const My: React.FC = () => {
  const { showDrawer } = useContext(ExploreContext);

  return (
    <Main>
      <Container>
        <AppCard
          title="面板"
          description="在这里可以查看系统概况"
          classify="工具"
          logo={<IconFont type="icon-panel" $size={26} />}
          banner={<Banner src={banner} alt="" />}
          onClick={() => showDrawer("panel")}
          showInstall={false}
        />
        <AppCard skeleton showInstall={false} />
      </Container>
    </Main>
  );
};

export default My;
