import { Divider, Image, Popover, Space } from "antd";
import { useEffect, useRef, useState } from "react";
import { Link, NavLink } from "react-router-dom";
import logo from "../../assets/logo.png";
import Function from "../sider/function";
import { Container, Explore, Line, Logo, Main, Menus } from "./style";

export default function SiderPanel() {
  const siderRef = useRef<HTMLDivElement>(null);
  const [height, setHeight] = useState(0);

  useEffect(() => {
    const siderElement = siderRef.current;
    const observer = new ResizeObserver((entries) => {
      for (let entry of entries) {
        const { height } = entry.contentRect;
        setHeight(height);
      }
    });

    if (siderElement) {
      observer.observe(siderElement);
    }

    return () => {
      if (siderElement && observer) {
        observer.unobserve(siderElement);
      }
    };
  }, []);

  const MoreFunctions = (
    <>
      <Space direction="vertical" align="center" size={18}>
        <NavLink to="/systeminfo">
          {({ isActive, isPending }) => (
            <Function type="icon-system-info" active={isActive} title="系统" />
          )}
        </NavLink>

        <NavLink to="/component">
          {({ isActive, isPending }) => (
            <Function type="icon-component" active={isActive} title="组件" />
          )}
        </NavLink>

        <NavLink to="/configuration">
          {({ isActive, isPending }) => (
            <Function
              type="icon-configuration"
              active={isActive}
              title="配置"
            />
          )}
        </NavLink>
      </Space>
      <Line>
        <Divider style={{ margin: "20px 0" }} />
      </Line>
      <Space direction="vertical" align="center" size={18}>
        <NavLink to="/starter">
          {({ isActive, isPending }) => (
            <Function type="icon-starter" active={isActive} title="启动" />
          )}
        </NavLink>

        <NavLink to="/generate">
          {({ isActive, isPending }) => (
            <Function
              type="icon-code-generate"
              active={isActive}
              title="代码"
            />
          )}
        </NavLink>
      </Space>
    </>
  );

  return (
    <Container width={60} ref={siderRef}>
      <Main>
        <Logo>
          <Link to="/">
            <Image src={logo} width={36} preview={false} />
          </Link>
        </Logo>
        <Line>
          <Divider />
        </Line>
        <Menus>
          <Space direction="vertical" align="center" size={18}>
            <NavLink to="/">
              {({ isActive, isPending }) => (
                <Function type="icon-panel" active={isActive} title="面板" />
              )}
            </NavLink>

            <NavLink to="/console">
              {({ isActive, isPending }) => (
                <Function type="icon-console" active={isActive} title="输出" />
              )}
            </NavLink>

            <NavLink to="/diagnosis">
              {({ isActive, isPending }) => (
                <Function
                  active={isActive}
                  type="icon-diagnosis"
                  title="诊断"
                  badge={5}
                  badgeStatus="error"
                />
              )}
            </NavLink>

            <NavLink to="/openapi">
              {({ isActive, isPending }) => (
                <Function type="icon-openapi" active={isActive} title="开放" />
              )}
            </NavLink>
          </Space>
          <Line>
            <Divider style={{ margin: "20px 0" }} />
          </Line>
          {height >= 800 ? (
            MoreFunctions
          ) : (
            <Popover content={MoreFunctions} placement="right" trigger="click">
              <Space direction="vertical" align="center" size={18}>
                <Function type="icon-more" />
              </Space>
            </Popover>
          )}
        </Menus>
        <Explore>
          <Space direction="vertical" align="center" size={18}>
            <NavLink to="/explore">
              {({ isActive, isPending }) => (
                <Function
                  type="icon-explore"
                  active={isActive}
                  title="探索"
                  style={{ color: "#52c41a" }}
                />
              )}
            </NavLink>
            <NavLink to="/setting">
              {({ isActive, isPending }) => (
                <Function type="icon-setting" active={isActive} title="设置" />
              )}
            </NavLink>
          </Space>
        </Explore>
      </Main>
    </Container>
  );
}