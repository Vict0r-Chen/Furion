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
  type?: "default" | "tag";
}

const Default = styled(TextBox)`
  display: inline-block;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.8);
  font-size: 14px;
  white-space: nowrap;
`;

const Tag = styled.span<{ $bgColor?: string }>`
  display: inline-block;
  background-color: ${(props) => props.$bgColor};
  color: #ffffff;
  font-size: 13px;
  width: 70px;
  border-radius: 3px;
  text-align: center;
  height: 20px;
  line-height: 20px;
  font-weight: 600;
`;

const HttpMethod: React.FC<HttpMethodProps> = ({
  value,
  width,
  type = "default",
}) => {
  const style: React.CSSProperties = {
    width,
  };

  if (type === "tag") {
    return <Tag $bgColor={getColor(value)}>{value}</Tag>;
  }

  return (
    <Default $color={getColor(value)} style={style}>
      {value}
    </Default>
  );
};

export default HttpMethod;
