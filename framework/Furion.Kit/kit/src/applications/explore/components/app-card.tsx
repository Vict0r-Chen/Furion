import { Button, Skeleton, Tag } from "antd";
import React, { MouseEventHandler } from "react";
import { css, styled } from "styled-components";
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

const Main = styled(Upward)<{ $showInstall?: boolean; $skeleton?: boolean }>`
  width: 100%;
  position: relative;
  overflow: hidden;
  height: ${(props) => (props.$showInstall === true ? 235 : 192)}px;
  border: 1px solid rgb(240, 240, 240);
  background-color: #ffffff;
  border-radius: 8px;

  ${(props) =>
    props.$skeleton !== true &&
    css`
      cursor: pointer;
    `}

  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  padding: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15);

  ${(props) =>
    props.$skeleton !== true
      ? css`
          transition: transform 0.3s;

          &:hover {
            transform: scale(1.03);
            z-index: 2;
            border: 1px solid #69b1ff;
            box-shadow: 0px 2px 8px #69b1ff;
          }
        `
      : css`
          transform: translateY(0) !important;
        `}
`;

const Tip = styled.div`
  display: inline-block;
  position: absolute;
  z-index: 1;
  top: 15px;
  right: 15px;
  background-color: rgba(0, 0, 0, 0.4);
  font-size: 12px;
  color: rgba(255, 255, 255, 0.8);
  padding: 0 5px;
  border-radius: 5px;
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
  min-width: 50px;
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
  overflow: hidden;
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
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  margin-top: 5px;
`;

const SkeletonImage = styled(Skeleton.Image)`
  width: 100% !important;
  height: 100% !important;
`;

const SkeletonDescription = styled(Skeleton)`
  & h3 {
    margin-top: 2px;
  }

  & ul {
    margin-top: 0 !important;
  }
`;

export type AppCardType = "card" | "list";

interface AppCardProps {
  title?: string;
  description?: React.ReactNode;
  classify?: string;
  logo?: React.ReactNode;
  banner?: React.ReactNode;
  install?: boolean;
  installClick?: MouseEventHandler<HTMLElement>;
  onClick?: MouseEventHandler<HTMLDivElement>;
  skeleton?: boolean;
  tip?: React.ReactNode;
  showInstall?: boolean;
  type?: AppCardType;
}

const AppCardSkeleton: React.FC<AppCardProps> = ({ showInstall = true }) => {
  return (
    <Container>
      <Main $showInstall={showInstall} $skeleton>
        <Panel>
          <Banner>
            <SkeletonImage active />
          </Banner>
          <Content>
            <Logo>
              <Skeleton.Avatar active shape="square" size={50} />
            </Logo>
            <Introduction>
              <SkeletonDescription
                active
                paragraph={{ rows: 1, width: "100%" }}
              />
            </Introduction>
          </Content>
        </Panel>
        {showInstall && <Skeleton.Input active block size="default" />}
      </Main>
    </Container>
  );
};

const AppCard: React.FC<AppCardProps> = ({
  title,
  description,
  classify,
  logo,
  banner,
  install = false,
  installClick,
  onClick,
  skeleton,
  tip,
  showInstall = true,
  type = "card",
}) => {
  return skeleton ? (
    <AppCardSkeleton showInstall={showInstall} />
  ) : (
    <Container onClick={onClick}>
      <Main $showInstall={showInstall}>
        {tip && <Tip>{tip}</Tip>}
        <Panel>
          <Banner>{banner}</Banner>
          <Content>
            <Logo>{logo}</Logo>
            <Introduction>
              <Classify bordered={false} color="cyan">
                {classify}
              </Classify>
              <Title>{title}</Title>
              <Description>{description}</Description>
            </Introduction>
          </Content>
        </Panel>
        {showInstall && (
          <Button
            type="primary"
            disabled={install}
            onClick={(ev) => {
              ev.stopPropagation();
              installClick && installClick(ev);
            }}
          >
            {!install ? "安装" : "已安装"}
          </Button>
        )}
      </Main>
    </Container>
  );
};

export default AppCard;
