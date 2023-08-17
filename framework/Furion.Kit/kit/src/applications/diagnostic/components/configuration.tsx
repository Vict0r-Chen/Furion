import axios from "axios";
import React, { useEffect, useState } from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";

const Container = styled.div`
  word-break: break-all;
`;

const Configuration: React.FC = () => {
  const [data, setData] = useState<object>({});

  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await axios.get(
          "https://localhost:7115/furion/configuration"
        );
        setData(response.data);
      } catch (error) {}
    };

    loadData();
  }, []);

  return (
    <Container>
      <ReactJson src={data} name={false} sortKeys />
    </Container>
  );
};

export default Configuration;
