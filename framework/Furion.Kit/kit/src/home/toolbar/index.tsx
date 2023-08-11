import { Space, Tooltip } from "antd";
import React from "react";
import { styled } from "styled-components";
import A from "../../components/a";
import { FlushDivider } from "../../components/divider";
import Flexbox from "../../components/flexbox";
import IconFont from "../../components/iconfont";

const Container = styled(Flexbox)`
  width: 30px;
  min-width: 30px;
  text-align: center;
  align-items: center;
  padding: 15px 0;
`;

const Icon = styled(IconFont)`
  cursor: pointer;
  font-size: 18px;
  color: #8c8c8c;
`;

interface ToolbarIconProps {
  type: string;
  title?: string;
  link?: string;
}

const ToolbarIcon: React.FC<ToolbarIconProps> = ({ type, title, link }) => {
  const Element = (
    <Tooltip title={title} placement="left">
      <Icon type={type} />
    </Tooltip>
  );
  return link ? (
    <A href={link} target="_blank" rel="noreferrer">
      {Element}
    </A>
  ) : (
    Element
  );
};

const Toolbar: React.FC = () => {
  return (
    <>
      <FlushDivider type="vertical" $heightBlock />
      <Container $spaceBetween direction="column">
        <div>
          <Space direction="vertical" size={20}>
            <ToolbarIcon
              type="icon-documentation"
              title="使用手册"
              link="https://furion.net"
            />
            <ToolbarIcon type="icon-language" title="中文 / English" />
          </Space>
        </div>
        <div></div>
      </Container>
    </>
  );
};

export default Toolbar;
