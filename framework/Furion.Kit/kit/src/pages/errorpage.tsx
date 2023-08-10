import { Button, Result } from "antd";
import React from "react";
import { useNavigate, useRouteError } from "react-router-dom";

const ErrorPage: React.FC = () => {
  const error: any = useRouteError();
  console.log(error);
  const navigate = useNavigate();

  return (
    <Result
      status={error.status}
      title={error.status}
      subTitle={error.statusText || error.message}
      extra={
        <Button type="primary" onClick={() => navigate("/")}>
          回到首页
        </Button>
      }
    />
  );
};

export default ErrorPage;
