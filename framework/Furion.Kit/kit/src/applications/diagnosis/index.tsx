import {
  Alert,
  Button,
  Dropdown,
  Form,
  Input,
  Modal,
  Popover,
  Space,
  Tabs,
  TabsProps,
} from "antd";
import React, { useContext, useState } from "react";
import ReactJson from "react-json-view";
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

const ModalContainer = styled.div`
  padding: 15px 0 0 0;
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
        title="诊断事件"
        icon={<Function.Icon type="icon-eventlog" />}
        onClick={clickHandle}
      />
      <Function
        title="终点路由"
        icon={<Function.Icon type="icon-endpoint" />}
        onClick={clickHandle}
      />
      <Function
        title="运行日志"
        icon={<Function.Icon type="icon-logging" />}
        onClick={clickHandle}
      />
      <Function
        title="项目配置"
        icon={<Function.Icon type="icon-configuration" />}
        onClick={clickHandle}
      />
      <FlushDivider $size={10} />
      <Function
        badge="Beta"
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
        <Icon type="icon-eventlog" $size={15} />
        <TextBox $disableSelect>诊断事件</TextBox>
      </Space>
    ),
    children: (
      <div>
        <Alert
          message="诊断器连接失败，请确保服务器已正常启动。"
          type="warning"
          showIcon
          closable
        />
        <br />
        内容
      </div>
    ),
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-endpoint" $size={15} />
        <TextBox $disableSelect>终点路由</TextBox>
      </Space>
    ),
    children: <div>内容2</div>,
  },
  {
    key: "3",
    label: (
      <Space>
        <Icon type="icon-logging" $size={15} />
        <TextBox $disableSelect>运行日志</TextBox>
      </Space>
    ),
    children: <div>内容3</div>,
  },
  {
    key: "4",
    label: (
      <Space>
        <Icon type="icon-configuration" $size={15} />
        <TextBox $disableSelect>项目配置</TextBox>
      </Space>
    ),
    children: (
      <div>
        <ReactJson
          // enableClipboard={false}
          theme="apathy:inverted"
          src={{
            name: "Furion",
            age: 31,
            version: "5.0.0",
          }}
        />
      </div>
    ),
  },
];

type FieldType = {
  title?: string;
  address?: string;
  category?: string;
};

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
        title="配置 - 请求侦听"
        open={isModalOpen}
        onCancel={handleCancel}
        maskClosable={false}
        footer={null}
      >
        <ModalContainer>
          <Form
            name="basic"
            labelCol={{ span: 6 }}
            wrapperCol={{ span: 18 }}
            initialValues={{
              title: "请求侦听",
              address: "https://localhost:7115/",
              category: "Microsoft.AspNetCore",
            }}
            autoComplete="off"
          >
            <Form.Item<FieldType> label="标题" name="title">
              <Input />
            </Form.Item>
            <Form.Item<FieldType> label="服务器地址" name="address">
              <Input />
            </Form.Item>
            <Form.Item<FieldType> label="诊断类别" name="category">
              <Input />
            </Form.Item>
            <Form.Item wrapperCol={{ offset: 6, span: 18 }}>
              <Space align="center">
                <Button type="primary" htmlType="submit" onClick={handleOk}>
                  确定
                </Button>
                <Button htmlType="button" onClick={handleCancel}>
                  取消
                </Button>
              </Space>
            </Form.Item>
          </Form>
        </ModalContainer>
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
