import { Space } from "antd";
import React, { useRef } from "react";
import Draggable from "react-draggable";
import { css, styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import SpaceBetween from "../../components/space-between";
import useResize from "../../hooks/useResize";
import useSiderStore from "../../stores/sider.store";
import Function, { FunctionProps } from "../function";
import Logo from "../logo";

const Container = styled(SpaceBetween)<{ $float?: boolean }>`
  flex-direction: column;
  text-align: center;
  align-items: center;
  width: 60px;
  min-width: 60px;
  height: 100%;
  background-color: #f7f8fb;
  padding: 15px 0;
  box-sizing: border-box;

  ${(props) =>
    props.$float &&
    css`
      position: fixed;
      z-index: 100;
      left: 24px;
      height: auto;
      /* top: 50%;
      transform: translateY(-50%); */
      border-radius: 8px;
      border: 1px solid #bae0ff;
      box-shadow: 0px 0px 3px #bae0ff;
    `}
`;

const FooterContainer = styled.div`
  margin-top: 15px;
`;

const functions: FunctionProps[] = [
  {
    render: (isActive) => (
      <Function.Icon type="icon-panel" $active={isActive} />
    ),
    link: "/",
    title: "面板",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-console" $active={isActive} />
    ),
    link: "/console",
    title: "输出",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-diagnosis" $active={isActive} />
    ),
    link: "/diagnosis",
    title: "诊断",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-openapi" $active={isActive} />
    ),
    link: "/openapi",
    title: "开放",
    divider: true,
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-system-info" $active={isActive} />
    ),
    link: "/systeminfo",
    title: "系统",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-component" $active={isActive} />
    ),
    link: "/component",
    title: "组件",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-configuration" $active={isActive} />
    ),
    link: "/configuration",
    title: "配置",
    divider: true,
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-starter" $active={isActive} />
    ),
    link: "/starter",
    title: "启动",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-code-generate" $active={isActive} />
    ),
    link: "/generate",
    title: "代码",
    divider: true,
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-explore" $active={isActive} />
    ),
    link: "/explore",
    title: "探索",
    position: "bottom",
  },
  {
    render: (isActive) => (
      <Function.Icon type="icon-setting" $active={isActive} />
    ),
    link: "/setting",
    title: "设置",
    position: "bottom",
  },
];

const Sider: React.FC = () => {
  const containerRef = useRef<HTMLDivElement>(null);
  const [float, position, setPosition] = useSiderStore((state) => [
    state.float,
    { x: state.floatX, y: state.floatY },
    state.setPosition,
  ]);

  useResize(containerRef, (rect) => {
    console.log(rect);
  });

  const Element = (
    <Container ref={containerRef} $float={float}>
      <div>
        <Logo />
        <Space direction="vertical" size={15}>
          {functions
            .filter((fn) => fn.position !== "bottom")
            .map((fn) => (
              <Function key={fn.link} {...fn} />
            ))}
        </Space>
      </div>
      <FooterContainer>
        <Space direction="vertical" size={15}>
          {functions
            .filter((fn) => fn.position === "bottom")
            .map((fn) => (
              <Function key={fn.link} {...fn} />
            ))}
        </Space>
      </FooterContainer>
    </Container>
  );

  return (
    <>
      {float ? (
        <Draggable
          bounds="parent"
          nodeRef={containerRef}
          defaultPosition={position}
          onDrag={(ev, data) => {
            setPosition(data.x, data.y);
          }}
        >
          {Element}
        </Draggable>
      ) : (
        Element
      )}
      <FlushDivider type="vertical" $heightBlock />
    </>
  );
};

export default Sider;
