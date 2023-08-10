import { Space } from "antd";
import { styled } from "styled-components";
import IconFont from "../../../components/iconfont";
import Text from "../../../components/text";

const Container = styled.div`
  height: 100%;
  background-color: #e6f4ff;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0 15px;
  position: relative;
  cursor: pointer;

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
    background-color: #bae0ff;
  }
`;

const Number = styled(Text)`
  font-weight: 600;
  color: unset;
`;

const Icon = styled(IconFont)`
  font-size: 14px;
`;

const BranchIcon = styled(Icon)`
  font-size: 16px;
`;

const Link = styled.a`
  text-decoration: none;
  color: unset;
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
      <Link href={link} target="_blank" rel="noreferrer">
        <Space>
          <BranchIcon type="icon-branch" />
          <Number>{number}</Number>
          <Icon type="icon-arrow-right" />
        </Space>
      </Link>
    </Container>
  );
};

export default Version;
