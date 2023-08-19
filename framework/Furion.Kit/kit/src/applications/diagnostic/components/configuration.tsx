import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import JsonViewer from "../../../components/json-viewer";
import projectConfig from "../../../project.config";
import Page from "./page";

const Container = styled(Page)``;

const Configuration: React.FC = () => {
  const [data, setData] = useState<object>({});

  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await axios.get(
          `${projectConfig.serverAddress}/configuration-diagnostic`
        );
        setData(response.data);
      } catch (error) {}
    };

    loadData();
  }, []);

  return (
    <Container>
      <JsonViewer value={data} collapsed />
    </Container>
  );
};

export default Configuration;
