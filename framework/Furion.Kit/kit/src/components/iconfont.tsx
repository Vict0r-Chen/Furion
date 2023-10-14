import { createFromIconfontCN } from "@ant-design/icons";
import styled, { css } from "styled-components";

const AntdIconFont = createFromIconfontCN({
  scriptUrl: "//at.alicdn.com/t/c/font_4199075_q6tsf4d0yqe.js",
});

const IconFont = styled(AntdIconFont)<{ $size?: number; $color?: string }>`
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
