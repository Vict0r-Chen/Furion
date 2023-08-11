import { createFromIconfontCN } from "@ant-design/icons";
import { css, styled } from "styled-components";

const IconFontDefault = createFromIconfontCN({
  scriptUrl: "//at.alicdn.com/t/c/font_4199075_6crgssopmcs.js",
});

const IconFont = styled(IconFontDefault)<{ $size?: number; $color?: string }>`
  ${(props) =>
    props.$size &&
    css`
      font-size: ${props.$size}px!important;
    `}

  ${(props) =>
    props.$color &&
    css`
      color: ${props.$color}!important;
    `}
`;

export default IconFont;
