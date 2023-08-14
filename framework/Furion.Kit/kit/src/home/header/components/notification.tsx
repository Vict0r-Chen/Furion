import { Badge, Button, Empty, Popover, Tabs, TabsProps } from "antd";
import React from "react";
import { styled } from "styled-components";
import Header from "..";
import IconFont from "../../../components/iconfont";

const Container = styled.div``;

const Main = styled.div`
  width: 350px;
  min-height: 200px;
  padding: 0 5px;
  box-sizing: border-box;
`;

const SettingIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const items: TabsProps["items"] = [
  {
    key: "1",
    label: `未读通知`,
    children: <Empty description="暂无通知" />,
  },
  {
    key: "2",
    label: `全部通知`,
    children: <Empty description="暂无通知" />,
  },
];

const Content: React.FC = () => {
  return (
    <Main>
      <Tabs
        defaultActiveKey="1"
        items={items}
        tabBarExtraContent={{
          right: (
            <Button
              type="text"
              icon={<SettingIcon type="icon-setting" $size={16} />}
            />
          ),
        }}
      />
    </Main>
  );
};

const NotificationBox: React.FC = () => {
  return (
    <Container>
      <Popover placement="bottom" content={<Content />} trigger="click">
        <Badge count={5} size="small">
          <Header.Icon type="icon-notification" />
        </Badge>
      </Popover>
    </Container>
  );
};

export default NotificationBox;
