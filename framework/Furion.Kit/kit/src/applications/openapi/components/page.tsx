import { FloatButton } from "antd";
import React, { useId, useState } from "react";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import { FlushDivider } from "../../../components/flush-divider";
import {
  OpenApiDescription,
  OpenApiGroup,
} from "../../../databases/types/openapi";
import OpenApiGroupContext from "../contexts";
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

const Page: React.FC<OpenApiGroup> = (group) => {
  const id = useId();
  const [apiDescription, setApiDescription] = useState<OpenApiDescription>();

  return (
    <OpenApiGroupContext.Provider
      value={{
        apiDescription,
        setApiDescription,
      }}
    >
      <Container id={id}>
        <PathList {...group} />
        <FlushDivider type="vertical" $heightBlock />
        <Main>
          <ApiDetail />
        </Main>
        <FloatButton.BackTop target={() => document.getElementById(id)!} />
      </Container>
    </OpenApiGroupContext.Provider>
  );
};

export default Page;
