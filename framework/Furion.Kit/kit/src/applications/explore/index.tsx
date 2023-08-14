import { Drawer, message } from "antd";
import React, { useState } from "react";
import { Outlet, useNavigate, useParams } from "react-router-dom";
import Fullscreen from "../../components/fullscreen";
import IconFont from "../../components/iconfont";
import Content from "../../home/content";
import Page from "./components/page";
import ExploreContext from "./context";
export { default as ExploreDetail } from "./detail";

const Explore: React.FC = () => {
  const navigate = useNavigate();
  const { name } = useParams();

  const [, contextHolder] = message.useMessage();

  const [open, setOpen] = useState(name ? true : false);
  const [fullscreen, setFullscreen] = useState(false);

  const showDrawer = (name: string) => {
    setOpen(true);
    navigate(`detail/${name}`);
  };

  const onClose = () => {
    setOpen(false);
    setTimeout(() => {
      navigate("/explore");
    }, 300);
  };

  return (
    <ExploreContext.Provider value={{ showDrawer }}>
      {contextHolder}
      <Drawer
        title="面板"
        placement="right"
        onClose={onClose}
        open={open}
        autoFocus={false}
        size="large"
        width={fullscreen ? "100%" : undefined}
        extra={
          <Fullscreen
            fullscreen={fullscreen}
            onClick={() => setFullscreen((f) => !f)}
          />
        }
      >
        <Outlet />
      </Drawer>
      <Content.Main>
        <Content.Title
          description="工具、文档、管理、娱乐，更多应用等你发掘。"
          extra={
            <Content.Menu
              menu={{
                items: [
                  {
                    key: 1,
                    label: "应用配置",
                    icon: <IconFont type="icon-configuration" $size={16} />,
                  },
                ],
              }}
            />
          }
        >
          探索
        </Content.Title>
        <Page />
      </Content.Main>
    </ExploreContext.Provider>
  );
};

export default Explore;
