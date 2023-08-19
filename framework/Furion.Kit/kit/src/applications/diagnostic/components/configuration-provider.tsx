import { FloatButton } from "antd";
import React, { useId } from "react";
import { styled } from "styled-components";
import JsonViewer from "../../../components/json-viewer";

const ScrollContainer = styled.div`
  overflow-y: auto;
  height: calc(100vh - 297px);
  box-sizing: border-box;
  position: relative;
`;

const ConfigurationProvider: React.FC<{ data?: object }> = ({ data }) => {
  const id = useId();

  return (
    <ScrollContainer id={id}>
      <JsonViewer value={data} collapsed objectSortKeys={false} />
      <FloatButton.BackTop target={() => document.getElementById(id)!} />
    </ScrollContainer>
  );
};

export default ConfigurationProvider;
