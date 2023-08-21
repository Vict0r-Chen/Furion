import { Button, Carousel, Dropdown, Space, Tabs, TabsProps } from "antd";
import React from "react";
import { styled } from "styled-components";
import Category from "../../../components/category";
import IconFont from "../../../components/iconfont";
import SearchBox from "../../../components/searchbox";
import TextBox from "../../../components/textbox";
import Community from "./community";
import My from "./my";
import Official from "./official";

const Container = styled.div`
  margin-top: 25px;
`;

const Icon = styled(IconFont)`
  margin-right: 0 !important;
`;

const UploadIcon = styled(IconFont)`
  position: relative;
  top: 1px;
`;

const items: TabsProps["items"] = [
  {
    key: "1",
    label: (
      <Space>
        <Icon type="icon-curated" $size={16} />
        <TextBox $disableSelect>官方精选</TextBox>
      </Space>
    ),
    children: <Official />,
  },
  {
    key: "2",
    label: (
      <Space>
        <Icon type="icon-community" $size={16} />
        <TextBox $disableSelect>社区推荐</TextBox>
      </Space>
    ),
    children: <Community />,
  },
];

const onChange = (key: string) => {
  console.log(key);
};

const BannerImg = styled.img`
  height: 180px;
  width: 100%;
  /* object-fit: contain; */
  background-color: #f0f0f0;
  border-radius: 8px;
`;

const Page: React.FC = () => {
  return (
    <>
      <Carousel autoplay autoplaySpeed={10000}>
        <div>
          <BannerImg src="http://furion.baiqian.ltd/img/crmeb.jpg" alt="" />
        </div>
        <div>
          <BannerImg src="http://furion.baiqian.ltd/img/tpflow.png" alt="" />
        </div>
        <div>
          <BannerImg src="http://furion.baiqian.ltd/img/lk.jpg" alt="" />
        </div>
        <div>
          <BannerImg src="http://furion.baiqian.ltd/img/weishen.jpg" alt="" />
        </div>
      </Carousel>
      <Container>
        <Category
          title="我的应用"
          icon={<IconFont type="icon-myapp" $size={16} />}
        >
          <My />
        </Category>
        <Category
          title="应用市场"
          icon={<IconFont type="icon-appmarket" $size={16} />}
        >
          <Tabs
            tabBarExtraContent={{
              right: (
                <Space>
                  <SearchBox placeholder="ChatGPT 电商" />
                  <Dropdown
                    placement="bottomRight"
                    menu={{
                      items: [
                        {
                          key: 1,
                          label: "选择应用包",
                          icon: (
                            <IconFont
                              type="icon-package"
                              $size={16}
                              color="#000000A6"
                            />
                          ),
                        },
                      ],
                    }}
                  >
                    <Button
                      type="primary"
                      icon={<UploadIcon type="icon-upload" $size={16} />}
                    >
                      上传
                    </Button>
                  </Dropdown>
                </Space>
              ),
            }}
            defaultActiveKey="1"
            items={items}
            onChange={onChange}
          />
        </Category>
      </Container>
    </>
  );
};

export default Page;
