import React from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";

const Container = styled.div``;

const Configuration: React.FC = () => {
  return (
    <Container>
      <ReactJson
        // enableClipboard={false}
        theme="apathy:inverted"
        src={{
          name: "Furion",
          age: 31,
          version: "5.0.0",
        }}
        style={{ padding: 10 }}
      />
    </Container>
  );
};

export default Configuration;
