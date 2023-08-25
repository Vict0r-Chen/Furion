import { createFromIconfontCN } from "@ant-design/icons";
import { css, styled } from "styled-components";

const AntdIconFont = createFromIconfontCN({
  scriptUrl: "//at.alicdn.com/t/c/font_4199075_vk6w65q4gza.js",
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
