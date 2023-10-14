import styled from "styled-components";

const ContentMain = styled.div<{ $width?: number; $backgroundColor?: string }>`
  width: 100%;
  max-width: ${(props) => props.$width || 1660}px;
  margin: 0 auto;
  box-sizing: border-box;
  padding: 0 40px;

  background-color: ${(props) => props.$backgroundColor || undefined};

  @media only screen and (max-width: 640px) {
    padding: 0 25px;
  }
`;

export default ContentMain;
