import styled from "styled-components";

const SiderSticky = styled.div<{ $top?: number; $width?: number }>`
  position: sticky;
  top: ${(props) => props.$top || 0}px;
  width: ${(props) => props.$width || 200}px;
  margin-right: 15px;

  @media only screen and (max-width: 640px) {
    display: none;
  }
`;

export default SiderSticky;
