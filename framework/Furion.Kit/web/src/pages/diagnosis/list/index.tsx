import { CloseCircleOutlined, WarningOutlined } from "@ant-design/icons";
import { Card, Popover, Space, Spin, Tag, Typography } from "antd";
import { useLiveQuery } from "dexie-react-hooks";
import React, { useEffect, useState } from "react";
import { db } from "../../../db";
import { useServerStore } from "../../../stores/server.store";
import { HighlightText } from "./style";

const { Text } = Typography;

export default function RequestList() {
  const online = useServerStore((state) => state.online);
  const [time, setTime] = useState(new Date());

  const httpDiagnost = useLiveQuery(async () => {
    return await db.httpDiagnost
      .where("requestPath")
      .notEqual("/furion/http-sse")
      .reverse()
      .limit(50)
      .sortBy("startTimestamp");
  });

  useEffect(() => {
    const timer = setInterval(() => {
      setTime(new Date());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  if (!httpDiagnost) {
    return <Spin />;
  }

  return (
    <Card hoverable style={{ width: "100%", margin: "15px 0" }}>
      <Space direction="vertical">
        {httpDiagnost?.map((item) => (
          <div key={item.traceIdentifier}>
            <Space size={15}>
              <HttpMethod value={item.requestMethod} />
              {item.statusCode ? (
                <Text
                  type={item.exception ? "danger" : "success"}
                  style={{ fontWeight: 500 }}
                >
                  {item.statusCode}
                </Text>
              ) : (
                <>
                  {!online ||
                  (item.startTimestamp &&
                    (time.getTime() - item.startTimestamp.getTime()) / 1000 >
                      10) ? (
                    <WarningOutlined style={{ color: "#faad14" }} />
                  ) : (
                    <Spin size="small" />
                  )}
                </>
              )}

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
                <Text
                  underline
                  copyable
                  ellipsis
                  italic
                  type={item.exception ? "danger" : undefined}
                  style={{ fontWeight: 500 }}
                >
                  {decodeURIComponent(item.requestPath)}
                </Text>
              </Popover>
              {item.startTimestamp && item.endTimestamp && (
                <Text type="secondary">
                  {item.endTimestamp.getTime() - item.startTimestamp.getTime()}
                  ms
                </Text>
              )}
              <HighlightText
                type="secondary"
                italic
                keyboard
                $color="#000000A6"
              >
                {item.endpoint?.displayName}
              </HighlightText>

              {item.exception && (
                <Tag icon={<CloseCircleOutlined />} color="#f50" />
              )}
            </Space>
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
