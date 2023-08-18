import { FloatButton } from "antd";
import React, { useId } from "react";
import { styled } from "styled-components";

const Container = styled.div`
  overflow-y: auto;
  height: calc(100vh - 245px);
  box-sizing: border-box;
  position: relative;
`;

const Page: React.FC<React.HtmlHTMLAttributes<HTMLDivElement>> = (props) => {
  const id = useId();

  return (
    <Container {...props} id={id}>
      {props.children}
      <FloatButton.BackTop target={() => document.getElementById(id)!} />
    </Container>
  );
};

export default Page;
