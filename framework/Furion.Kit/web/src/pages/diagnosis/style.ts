import { Layout } from "antd";
import { styled } from "styled-components";

const Panel = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const Container = styled(Layout)`
  overflow: hidden;
  flex: 1;
  background-color: #ffffff;

  &:hover {
    overflow: auto;
  }

  &::-webkit-scrollbar {
    width: 2px;
    height: 2px;
    background-color: rgba(0, 0, 0, 0);
  }

  &::-webkit-scrollbar-thumb {
    background-color: rgba(0, 0, 0, 0);
  }

  &::-webkit-scrollbar-track {
    background-color: rgba(0, 0, 0, 0);
  }
`;

export { Container, Panel };

