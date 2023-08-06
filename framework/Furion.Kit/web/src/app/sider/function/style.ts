import { css, styled } from "styled-components";
import IconFont from "../../../components/iconfont";

const activeCss = css`
  background-color: #fff;
  border: 1px solid #69b1ff;
  box-shadow: 0px 0px 3px #69b1ff;
`;

const activeColorCss = css`
  color: #001d66;
`;

const IconButton = styled.div<{ $active?: boolean }>`
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  border-radius: 8px;
  cursor: pointer;
  transition: transform 0.2s;

  ${(props) =>
    props.$active === true
      ? activeCss
      : css`
          background-color: #e6f4ff;
        `}

  &:hover {
    transform: translateY(-2px);

    ${activeCss}
  }

  &:hover svg {
    ${activeColorCss}
  }
`;

const Icon = styled(IconFont)<{ $active?: boolean }>`
  font-size: 24px;
  color: #000000e0;

  ${(props) =>
    props.$active === true
      ? activeColorCss
      : css`
          color: #000000e0;
        `}
`;

export { Icon, IconButton };

