import { Space } from "antd";
import { useRef } from "react";
import { styled } from "styled-components";
import { FlushDivider } from "../../components/divider";
import useResize from "../../hooks/useResize";
import Function, { FunctionProps } from "../function";
import Logo from "../logo";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  text-align: center;
  align-items: center;
  width: 60px;
  min-width: 60px;
  height: 100%;
  background-color: #f7f8fb;
  justify-content: space-between;
  padding: 15px 0;
  box-sizing: border-box;
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

  useResize(containerRef, (rect) => {
    console.log(rect);
  });

  return (
    <>
      <Container ref={containerRef}>
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
        <div>
          <Space direction="vertical" size={15}>
            {functions
              .filter((fn) => fn.position === "bottom")
              .map((fn) => (
                <Function key={fn.link} {...fn} />
              ))}
          </Space>
        </div>
      </Container>
      <FlushDivider type="vertical" $heightBlock />
    </>
  );
};

export default Sider;
