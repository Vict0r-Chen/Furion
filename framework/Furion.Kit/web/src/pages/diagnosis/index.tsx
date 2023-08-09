import { Alert, Typography } from "antd";
import { useEffect, useState } from "react";
import { HttpDiagnost, db } from "../../db";
import RequestList from "./list";
import { Panel } from "./style";

const { Title } = Typography;

export default function Diagnosis() {
  const [monitor, setMonitor] = useState(true);
  useEffect(() => {
    var eventSource = new EventSource("https://localhost:7115/furion/http-sse");

    const addData = async (data: HttpDiagnost) => {
      try {
        await db.httpDiagnost.put(data);
      } catch (error) {
        console.log(error);
      }
    };

    eventSource.onmessage = function (event) {
      console.log("Received SSE data:", event.data);
      var data = JSON.parse(event.data);
      if (data.startTimestamp) {
        data.startTimestamp = new Date(data.startTimestamp);
      }
      if (data.endTimestamp) {
        data.endTimestamp = new Date(data.endTimestamp);
      }

      addData(data as HttpDiagnost);
    };

    eventSource.onerror = function (event) {
      setMonitor(false);
      console.log("SSE error:", event);
    };

    eventSource.onopen = function () {
      setMonitor(true);
      console.log("SSE connection opened");
    };

    return () => {
      eventSource.close();
    };
  }, []);

  return (
    <Panel>
      <Title level={3}>诊断</Title>
      {!monitor && (
        <Alert
          message="诊断器连接失败，请确保服务器已正常启动。"
          type="warning"
          showIcon
          closable
        />
      )}
      <div>
        <RequestList />
      </div>
    </Panel>
  );
}
