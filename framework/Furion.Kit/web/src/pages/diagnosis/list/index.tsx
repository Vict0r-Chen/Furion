import { CloseCircleOutlined, WarningOutlined } from "@ant-design/icons";
import {
  Button,
  Card,
  Empty,
  Popover,
  Skeleton,
  Space,
  Spin,
  Tag,
  Typography,
} from "antd";
import { useLiveQuery } from "dexie-react-hooks";
import React, { useEffect, useState } from "react";
import IconFont from "../../../components/iconfont";
import { db } from "../../../db";
import { useServerStore } from "../../../stores/server.store";
import { HighlightText } from "./style";

const { Text } = Typography;

const pageSize = 20;

export default function RequestList() {
  const online = useServerStore((state) => state.online);
  const [time, setTime] = useState(new Date());
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);

  const count = useLiveQuery(async () =>
    db.httpDiagnost.filter((f) => f.requestPath !== "/furion/http-sse").count()
  );

  const httpDiagnost = useLiveQuery(async () => {
    var data = await db.httpDiagnost
      .filter((f) => f.requestPath !== "/furion/http-sse")
      .reverse()
      .limit(pageSize * page)
      .toArray();

    setLoading(false);
    return data;
  }, [page]);

  useEffect(() => {
    const timer = setInterval(() => {
      setTime(new Date());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  if (!httpDiagnost) {
    return <Skeleton active />;
  }

  return (
    <>
      <Card hoverable style={{ width: "100%", margin: "15px 0" }}>
        {httpDiagnost.length === 0 ? (
          <Empty description="暂无数据" />
        ) : (
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
                        (time.getTime() - item.startTimestamp.getTime()) /
                          1000 >
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
                      {item.endTimestamp.getTime() -
                        item.startTimestamp.getTime()}
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

                  {item.displayName && (
                    <Space>
                      <IconFont type="icon-tag" style={{ color: "#8c8c8c" }} />
                      <Text italic style={{ color: "#434343" }}>
                        {item.displayName}
                      </Text>
                    </Space>
                  )}

                  {item.exception && (
                    <Tag icon={<CloseCircleOutlined />} color="#f50" />
                  )}
                </Space>
              </div>
            ))}
          </Space>
        )}
      </Card>
      <div
        style={{ textAlign: "center" }}
        onClick={() => {
          setLoading(true);
          setPage((p) => p + 1);
        }}
      >
        {count && count > httpDiagnost.length ? (
          <>
            {loading && <Skeleton active />}
            <Button>加载更多</Button>
          </>
        ) : count ? (
          <Text type="secondary">没有更多数据了</Text>
        ) : (
          <></>
        )}
      </div>
    </>
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
