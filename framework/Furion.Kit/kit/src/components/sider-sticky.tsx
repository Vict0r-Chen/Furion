import { styled } from "styled-components";

const SiderSticky = styled.div<{ $top?: number }>`
  position: sticky;
  top: ${(props) => props.$top || 0}px;
  width: 200px;
  margin-right: 15px;

  @media only screen and (max-width: 640px) {
    display: none;
  }
`;

export default SiderSticky;
