import { Button, Result } from "antd";
import { useNavigate, useRouteError } from "react-router-dom";

export default function ErrorPage() {
  const error: any = useRouteError();
  const navigate = useNavigate();

  return (
    <Result
      status="404"
      title="404"
      subTitle={
        "对不起，您访问的页面不存在。" + (error.statusText || error.message)
      }
      extra={
        <Button type="primary" onClick={() => navigate("/")}>
          回到首页
        </Button>
      }
    />
  );
}
