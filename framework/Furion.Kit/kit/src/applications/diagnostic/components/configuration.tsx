import axios from "axios";
import React, { useEffect, useState } from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";

const Container = styled.div``;

const Configuration: React.FC = () => {
  const [data, setData] = useState<object>({});

  useEffect(() => {
    const loadData = async () => {
      const response = await axios.get(
        "https://localhost:7115/furion/configuration"
      );
      setData(response.data);
    };

    loadData();
  }, []);

  return (
    <Container>
      <ReactJson
        collapsed
        theme="apathy:inverted"
        sortKeys
        src={data}
        style={{ padding: 10 }}
      />
    </Container>
  );
};

export default Configuration;
