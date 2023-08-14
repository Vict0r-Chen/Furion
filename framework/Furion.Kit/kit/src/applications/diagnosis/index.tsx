import { Button, Dropdown, Modal, Popover, Space, Tabs, TabsProps } from "antd";
import React, { useContext, useState } from "react";
import { styled } from "styled-components";
import Flexbox from "../../components/flexbox";
import { FlushDivider } from "../../components/flush-divider";
import IconFont from "../../components/iconfont";
import TextBox from "../../components/textbox";
import Content from "../../home/content";
import Function from "./components/function";
import DiagnosisContext from "./context";

const AddIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const Container = styled.div``;

const Icon = styled(IconFont)`
  margin-right: 0 !important;
`;

const SettingIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const NewContainer = styled(Flexbox)`
  width: 270px;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const NewList: React.FC = () => {
  const { showModal, hidePopover } = useContext(DiagnosisContext);

  const clickHandle = () => {
    hidePopover();
    showModal();
  };

  return (
    <NewContainer>
      <Function
        title="请求侦听"
        icon={<Function.Icon type="icon-http" />}
        onClick={clickHandle}
      />
      <Function
        title="终点路由"
        icon={<Function.Icon type="icon-endpoint" />}
        onClick={clickHandle}
      />
      <Function
        title="控制台"
        icon={<Function.Icon type="icon-console" />}
        onClick={clickHandle}
      />
      <FlushDivider $size={10} />
      <Function
        title="自定义"
        icon={<Function.Icon type="icon-customize" />}
        onClick={clickHandle}
      />
    </NewContainer>
  );
};

const items: TabsProps["items"] = [
  {
    key: "1",
    label: (
      <Space>
        <Icon type="icon-http" $size={14} />
        <TextBox $disableSelect>请求侦听</TextBox>
      </Space>
    ),
    children: <div>内容</div>,
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-endpoint" $size={14} />
        <TextBox $disableSelect>终点路由</TextBox>
      </Space>
    ),
    children: <div>内容2</div>,
  },
  {
    key: "3",
    label: (
      <Space>
        <Icon type="icon-console" $size={14} />
        <TextBox $disableSelect>控制台</TextBox>
      </Space>
    ),
    children: <div>内容3</div>,
  },
];

const Diagnosis: React.FC = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [open, setOpen] = useState(false);

  const hidePopover = () => {
    setOpen(false);
  };

  const handleOpenChange = (newOpen: boolean) => {
    setOpen(newOpen);
  };

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleOk = () => {
    setIsModalOpen(false);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  return (
    <DiagnosisContext.Provider value={{ showModal, hidePopover }}>
      <Modal
        title="诊断配置"
        open={isModalOpen}
        onOk={handleOk}
        onCancel={handleCancel}
        cancelText="关闭"
        okText="确定"
      >
        内容
      </Modal>
      <Content.Main>
        <Content.Title
          description="添加请求、路由、控制台，等诊断信息。"
          more={
            <Popover
              placement="bottomRight"
              content={<NewList />}
              trigger="click"
              open={open}
              onOpenChange={handleOpenChange}
            >
              <Button
                type="primary"
                icon={<AddIcon type="icon-add" $size={16} />}
              >
                新建
              </Button>
            </Popover>
          }
        >
          诊断
        </Content.Title>
        <Container>
          <Tabs
            defaultActiveKey="1"
            items={items}
            centered
            tabBarExtraContent={{
              right: (
                <Dropdown
                  placement="bottomRight"
                  menu={{
                    items: [
                      {
                        key: 1,
                        label: "移除此项",
                        icon: (
                          <IconFont
                            type="icon-uninstall"
                            $size={16}
                            color="#000000A6"
                          />
                        ),
                      },
                    ],
                  }}
                >
                  <Button
                    type="text"
                    icon={<SettingIcon type="icon-setting" $size={16} />}
                  >
                    配置
                  </Button>
                </Dropdown>
              ),
            }}
          />
        </Container>
      </Content.Main>
    </DiagnosisContext.Provider>
  );
};

export default Diagnosis;
