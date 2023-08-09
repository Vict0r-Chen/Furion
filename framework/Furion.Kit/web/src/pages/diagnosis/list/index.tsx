import { CloseCircleOutlined } from "@ant-design/icons";
import { Card, Popover, Space, Spin, Tag, Typography } from "antd";
import { useLiveQuery } from "dexie-react-hooks";
import React from "react";
import { db } from "../../../db";

const { Text } = Typography;

export default function RequestList() {
  const httpDiagnost = useLiveQuery(async () => {
    return await db.httpDiagnost.reverse().limit(50).toArray();
  });

  if (!httpDiagnost) {
    return <Spin />;
  }

  return (
    <Card hoverable title="请求" style={{ width: "100%", marginTop: 15 }}>
      <Space direction="vertical">
        {httpDiagnost?.map((item) => (
          <div key={item.traceIdentifier}>
            <Popover
              title={
                <Space size={5}>
                  <Text>连接标识：</Text>
                  <Text style={{ color: "#595959" }}>
                    {item.traceIdentifier}
                  </Text>
                </Space>
              }
            >
              <Space>
                <HttpMethod value={item.requestHttpMethod} />
                <Text type="success">{item.responseStatusCode}</Text>
                <Text
                  underline
                  copyable
                  ellipsis
                  italic
                  type={item.exception ? "danger" : undefined}
                  style={{ fontWeight: item.exception ? 500 : undefined }}
                >
                  {decodeURIComponent(item.requestPath)}
                </Text>
                {item.exception && (
                  <Tag icon={<CloseCircleOutlined />} color="#f50" />
                )}
              </Space>
            </Popover>
          </div>
        ))}
      </Space>
    </Card>
  );
}
const getColor = (value: string) => {
  switch (value) {
    case "GET":
      return "#00FF00";
    case "POST":
      return "#0000FF";
    case "DELETE":
      return "#FF0000";
    case "PUT":
      return "#FFFF00";
    case "HEAD":
      return "#808080";
    case "PATCH":
      return "#FFA500";
    case "OPTIONS":
      return "#800080";
    case "TRACE":
      return "#ADD8E6";
    case "CONNECT":
      return "#00FA9A";
  }
};

const HttpMethod: React.FC<{ value: string }> = ({ value }) => {
  return (
    <Text
      style={{
        fontWeight: 500,
        color: getColor(value),
        width: 70,
        display: "inline-block",
      }}
    >
      {value}
    </Text>
  );
};
