import { Tooltip } from "antd";
import { styled } from "styled-components";
import A from "../../../components/a";
import IconFont from "../../../components/iconfont";

const Icon = styled(IconFont)`
  cursor: pointer;
  font-size: 18px;
  color: #8c8c8c;

  &:hover {
    color: #1677ff;
  }
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

export default ToolbarIcon;
