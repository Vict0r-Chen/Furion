import { FloatButton } from "antd";
import React, { useId } from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import { FlushDivider } from "../../../components/flush-divider";
import ApiDetail from "./api-detail";
import PathList from "./path-list";

const Container = styled(Flexbox)`
  overflow-y: auto;
  height: calc(100vh - 242px);
  align-items: flex-start;
`;

const Main = styled.div`
  flex: 1;
  box-sizing: border-box;
  padding: 0 15px;
`;

const Page: React.FC = () => {
  const id = useId();

  return (
    <Container id={id}>
      <PathList />
      <FlushDivider type="vertical" $heightBlock />
      <Main>
        <ApiDetail />
      </Main>
      <FloatButton.BackTop target={() => document.getElementById(id)!} />
    </Container>
  );
};

export default Page;
