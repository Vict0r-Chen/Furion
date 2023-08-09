import { Typography } from "antd";
import { styled } from "styled-components";

const { Text } = Typography;

const HighlightText = styled(Text)<{ $color?: string }>`
  position: relative;
  transition: color 0.3s;

  &::before {
    content: "";
    position: absolute;
    left: 4px;
    right: 4px;
    bottom: -3px;
    height: 2px;
    background-color: ${(props) => props.$color || "red"};
    transform: scaleX(0);
    transform-origin: left center;
    transition: transform 0.3s;
  }

  &:hover {
    color: ${(props) => props.$color || "red"};
  }

  &:hover::before {
    transform: scaleX(1);
  }
`;

export { HighlightText };

