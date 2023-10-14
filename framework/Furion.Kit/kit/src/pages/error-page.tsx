import { Button, Result } from "antd";
import React from "react";
import { useNavigate, useRouteError } from "react-router-dom";
import styled from "styled-components";
import Layout from "../home/layout";

const Container = styled.div`
  flex: 1;
`;

const ErrorPage: React.FC = () => {
  const error: any = useRouteError();
  const navigate = useNavigate();

  return (
    <Layout>
      <Container>
        <Result
          status={error.status || "warning"}
          title={error.status || "页面出现未知异常"}
          subTitle={error.statusText || error.message}
          extra={
            <Button type="primary" onClick={() => navigate("/")}>
              回到首页
            </Button>
          }
        />
      </Container>
    </Layout>
  );
};

export default ErrorPage;
