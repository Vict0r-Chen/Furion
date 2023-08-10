import { Space } from "antd";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import Function from "../function";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  text-align: center;
  align-items: center;
  width: 60px;
  min-width: 60px;
  height: 100%;
  background-color: #f7f8fb;
`;

const Sider: React.FC = () => {
  return (
    <>
      <Container>
        <Space direction="vertical" size={15}>
          <Function
            content={<Function.Icon type="icon-panel" />}
            link="/"
            title="面板"
          />
          <Function
            content={<Function.Icon type="icon-console" />}
            link="/console"
            title="输出"
          />
        </Space>
      </Container>
      <FlushDivider type="vertical" $heightBlock />
    </>
  );
};

export default Sider;
