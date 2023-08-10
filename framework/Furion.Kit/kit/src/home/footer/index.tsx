import React from "react";
import { styled } from "styled-components";
import A from "../../components/a";
import { FlushDivider } from "../../components/divider";
import SpaceBetween from "../../components/space-between";
import TextBox from "../../components/textbox";

const Container = styled(SpaceBetween)`
  height: 30px;
  min-height: 30px;
  padding: 0 15px;
  color: #000000a6;
  font-size: 12px;
  align-items: center;
`;

const Footer: React.FC = () => {
  return (
    <>
      <FlushDivider type="horizontal" $widthBlock />
      <Container>
        <div>
          <TextBox>
            技术支持 © 2020-2023{" "}
            <A
              href="https://gitee.com/monksoul"
              target="_blank"
              rel="noreferrer"
              $hoverDecoration
            >
              百小僧
            </A>
            ，
            <A
              href="https://baiqian.ltd"
              target="_blank"
              rel="noreferrer"
              $hoverDecoration
            >
              百签科技（广东）有限公司
            </A>
          </TextBox>
        </div>
      </Container>
    </>
  );
};

export default Footer;
