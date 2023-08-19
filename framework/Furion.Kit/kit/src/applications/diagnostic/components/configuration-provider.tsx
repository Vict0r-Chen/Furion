import React from "react";
import JsonViewer from "../../../components/json-viewer";
import Page from "./page";

const ConfigurationProvider: React.FC<{ data?: object }> = ({ data }) => {
  return (
    <Page>
      <JsonViewer value={data} collapsed objectSortKeys={false} />
    </Page>
  );
};

export default ConfigurationProvider;
