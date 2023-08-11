import { styled } from "styled-components";

const ContentMain = styled.div<{ $width?: number }>`
  width: 100%;
  max-width: ${(props) => props.$width || 1660}px;
  margin: 0 auto;
  box-sizing: border-box;
  padding: 0 40px;
`;

export default ContentMain;