import { css, styled } from "styled-components";

interface FlexboxProps {
  direction?: "column" | "column-reverse" | "row" | "row-reverse";
}

const Flexbox = styled.div<
  FlexboxProps & {
    $spaceBetween?: boolean;
    $flex?: number;
    $fullscreen?: boolean;
  }
>`
  display: flex;
  box-sizing: border-box;

  flex-direction: ${(props) => props.direction || "row"};
  ${(props) =>
    props.$flex &&
    css`
      flex: ${props.$flex};
    `}

  ${(props) =>
    props.$spaceBetween === true &&
    css`
      justify-content: space-between;
    `}

    ${(props) =>
    props.$fullscreen &&
    css`
      width: 100vw;
      height: 100vh;
    `}
`;

export default Flexbox;
