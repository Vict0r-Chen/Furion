import JsonView, { JsonViewProps } from "@uiw/react-json-view";
import React from "react";
import { styled } from "styled-components";

import { TriangleSolidArrow } from "@uiw/react-json-view/triangle-solid-arrow";

const Container = styled.div`
  word-break: break-all;
`;

const customTheme = {
  "--w-rjv-color": "rgb(0, 43, 54)",
};

const JsonViewr: React.FC<
  Omit<JsonViewProps<object>, "ref"> & React.RefAttributes<HTMLDivElement>
> = (props) => {
  return (
    <Container>
      <JsonView
        objectSortKeys
        collapsed={false}
        style={{
          lineHeight: "26.4px",
          letterSpacing: "0.5px",
          opacity: 0.85,
          ...customTheme,
        }}
        components={{
          arrow: <TriangleSolidArrow />,
        }}
        {...props}
      />
    </Container>
  );
};

export default JsonViewr;
