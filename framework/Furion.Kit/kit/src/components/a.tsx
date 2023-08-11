import { css, styled } from "styled-components";

const A = styled.a<{ $hoverDecoration?: boolean }>`
  color: unset !important;
  text-decoration: none;

  ${(props) =>
    props.$hoverDecoration === true &&
    css`
      &:hover {
        text-decoration: underline !important;
      }
    `}

  &:hover,
  &:active {
    color: unset;
  }
`;

export default A;
