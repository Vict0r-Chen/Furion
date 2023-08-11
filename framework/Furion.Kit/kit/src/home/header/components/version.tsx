import { Space } from "antd";
import { styled } from "styled-components";
import A from "../../../components/a";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";

const Container = styled.div`
  background-color: #e6f4ff;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0 15px;
  position: relative;
  cursor: pointer;
  height: 35px;
  min-height: 35px;
  margin-left: 35px;
  color: #4096ff;
  font-size: 14px;

  &::before {
    content: "";
    position: absolute;
    z-index: -1;
    top: 0;
    left: -20px;
    width: 100%;
    height: 100%;
    transform: skew(45deg);
    background-color: #e6f4ff;
  }

  &:hover,
  &:hover::before {
    color: #1677ff;
    background-color: #bae0ff;
  }
`;

const Number = styled(TextBox)`
  font-weight: 500;
`;

const Icon = styled(IconFont)`
  font-size: 14px;
`;

const BranchIcon = styled(Icon)`
  font-size: 16px;
`;

interface VersionProps {
  link?: string;
  number?: string;
}

const Version: React.FC<VersionProps> = ({
  link = "https://furion.net",
  number,
}) => {
  return (
    <Container>
      <A href={link} target="_blank" rel="noreferrer">
        <Space>
          <BranchIcon type="icon-branch" />
          <Number>{number}</Number>
          <Icon type="icon-arrow-right" />
        </Space>
      </A>
    </Container>
  );
};

export default Version;
