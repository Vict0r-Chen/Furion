import { Typography } from "antd";
import { css, styled } from "styled-components";

const { Text } = Typography;

const TextBox = styled(Text)<{
  $color?: string;
  $pointer?: boolean;
  $disableSelect?: boolean;
}>`
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

  ${(props) =>
    props.$disableSelect === true &&
    css`
      user-select: none;
    `}
`;

export default TextBox;
