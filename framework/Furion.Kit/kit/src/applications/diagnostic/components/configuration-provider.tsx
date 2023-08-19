import React from "react";
import { styled } from "styled-components";
import JsonViewer from "../../../components/json-viewer";

const ScrollContainer = styled.div`
  overflow-y: auto;
  height: calc(100vh - 297px);
  box-sizing: border-box;
  position: relative;
`;

const ConfigurationProvider: React.FC<{ data?: object }> = ({ data }) => {
  return (
    <ScrollContainer>
      <JsonViewer value={data} collapsed objectSortKeys={false} />
    </ScrollContainer>
  );
};

export default ConfigurationProvider;
