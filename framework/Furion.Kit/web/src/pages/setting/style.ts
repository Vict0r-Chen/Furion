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
`;

export { Container, Panel };

