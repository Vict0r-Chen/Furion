import { Button, Space, Tag } from "antd";
import React from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import Upward from "../../../components/upward";

const Container = styled.div`
  box-sizing: border-box;
  padding: 0 15px 15px 0;
  @media only screen and (max-width: 480px) {
    width: 100%;
  }

  @media only screen and (min-width: 481px) and (max-width: 880px) {
    width: 50%;
  }

  @media only screen and (min-width: 881px) and (max-width: 1200px) {
    width: 33.33%;
  }

  @media (min-width: 1201px) and (max-width: 1590px) {
    width: 25%;
  }
  @media (min-width: 1591px) {
    width: 16.66%;
  }
`;

const Main = styled(Upward)`
  width: 100%;
  position: relative;
  overflow: hidden;
  height: 250px;
  border: 1px solid rgb(240, 240, 240);
  background-color: #ffffff;
  border-radius: 8px;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  padding: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15);

  transition: transform 0.3s;

  &:hover {
    transform: scale(1.03);
    z-index: 2;
    border: 1px solid #69b1ff;
    box-shadow: 0px 2px 8px #69b1ff;
  }
`;

const Panel = styled.div`
  flex: 1;
`;

const Banner = styled.div`
  background-color: rgb(240, 240, 240);
  height: 110px;
  border-radius: 8px;
  margin-bottom: 10px;
  overflow: hidden;
  user-select: none;
`;

const Content = styled(Flexbox)``;

const Logo = styled(Flexbox)`
  height: 50px;
  width: 50px;
  border-radius: 8px;
  background-color: rgb(240, 240, 240);
  margin-right: 10px;
  align-items: center;
  justify-content: center;
  text-align: center;
  overflow: hidden;
`;

const Introduction = styled.div`
  flex: 1;
  position: relative;
`;

const Classify = styled(Tag)`
  position: absolute;
  right: 0;
  top: 0;
  user-select: none;
`;

const Title = styled.h4`
  margin: 0;
  user-select: none;
`;

const Description = styled.div`
  font-size: 12px;
  color: #000000a6;
  user-select: none;
`;

interface AppCardProps {
  title?: string;
  description?: React.ReactNode;
  classify?: string;
  logo?: React.ReactNode;
  banner?: React.ReactNode;
  install?: boolean;
}

const AppCard: React.FC<AppCardProps> = ({
  title,
  description,
  classify,
  logo,
  banner,
  install = false,
}) => {
  return (
    <Container>
      <Main>
        <Panel>
          <Banner>{banner}</Banner>
          <Content>
            <Logo>{logo}</Logo>
            <Introduction>
              <Classify bordered={false} color="cyan">
                {classify}
              </Classify>
              <Space direction="vertical" size={5}>
                <Title>{title}</Title>
                <Description>{description}</Description>
              </Space>
            </Introduction>
          </Content>
        </Panel>
        <Button type="primary" disabled={install}>
          {!install ? "安装" : "已安装"}
        </Button>
      </Main>
    </Container>
  );
};

export default AppCard;
