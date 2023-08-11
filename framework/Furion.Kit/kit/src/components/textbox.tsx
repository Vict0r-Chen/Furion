import { Typography } from "antd";
import { css, styled } from "styled-components";

const { Text } = Typography;

const TextBox = styled(Text)<{ $color?: string; $pointer?: boolean }>`
  font-size: unset;
  color: unset;

  ${(props) =>
    props.$color &&
    css`
      color: ${props.$color};
    `}

  ${(props) =>
    props.$pointer === true &&
    css`
      cursor: pointer;
    `}
`;

export default TextBox;
