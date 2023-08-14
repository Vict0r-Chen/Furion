import { Divider } from "antd";
import { css, styled } from "styled-components";

const FlushDivider = styled(Divider)<{
  $heightBlock?: boolean;
  $widthBlock?: boolean;
  $size?: number;
}>`
  margin: 0;

  ${(props) =>
    props.$heightBlock &&
    css`
      height: 100%;
    `}

  ${(props) =>
    props.$widthBlock &&
    css`
      width: 100%;
    `}

  ${(props) =>
    props.$size &&
    css`
      margin: ${props.$size}px 0;
    `}
`;

export { FlushDivider };

