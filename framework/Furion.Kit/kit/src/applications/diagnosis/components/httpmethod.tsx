import React from "react";
import { styled } from "styled-components";
import TextBox from "../../../components/textbox";

export const getColor = (value: string) => {
  switch (value) {
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
}

const Value = styled(TextBox)`
  font-weight: 600;
  display: inline-block;
`;

const HttpMethod: React.FC<HttpMethodProps> = ({ value }) => {
  return <Value $color={getColor(value)}>{value}</Value>;
};

export default HttpMethod;
