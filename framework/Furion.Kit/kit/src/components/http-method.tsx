import React from "react";
import { styled } from "styled-components";
import TextBox from "./textbox";

export const getColor = (value: string) => {
  switch (value.toUpperCase()) {
    case "GET":
      return "#00FF00";
    case "POST":
      return "#0000FF";
    case "DELETE":
      return "#FF0000";
    case "PUT":
      return "#FFFF00";
    case "HEAD":
      return "#808080";
    case "PATCH":
      return "#FFA500";
    case "OPTIONS":
      return "#800080";
    case "TRACE":
      return "#ADD8E6";
    case "CONNECT":
      return "#00FA9A";
  }
};

interface HttpMethodProps {
  value: string;
  width?: number | string;
  fontSize?: number | string;
}

const Default = styled(TextBox)`
  display: inline-block;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.8);
  font-size: 14px;
  white-space: nowrap;
  user-select: none;
  letter-spacing: 0.5px;
`;

const HttpMethod: React.FC<HttpMethodProps> = ({ value, width, fontSize }) => {
  const style: React.CSSProperties = {
    width,
    textAlign: width ? "left" : undefined,
    fontSize,
  };

  return (
    <Default $color={getColor(value)} style={style}>
      {value}
    </Default>
  );
};

export default HttpMethod;
