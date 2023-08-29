import { Alert, Button, Empty, Skeleton, Typography } from "antd";
import { useLiveQuery } from "dexie-react-hooks";
import React, { useEffect, useState } from "react";
import { styled } from "styled-components";
import { database } from "../../../databases";
import { EndpointDiagnosticModel } from "../../../databases/types/endpoint.diagnostic";
import projectConfig from "../../../project.config";
import EndpointItem from "./endpoint-item";
import Page from "./page";

const Container = styled(Page)``;

const ItemContainer = styled.div``;

const AlertBox = styled(Alert)`
  margin-bottom: 15px;
`;

const Operation = styled.div`
  text-align: center;
  margin: 15px 0;
`;

const pageSize = 15;

const excludeUrls = [
  "/furion/configuration-diagnostic",
  "/furion/endpoint-diagnostic-sse",
  "/furion/configuration-provider-diagnostic",
  "/furion/component-diagnostic",
  "/furion/openapi",
];

const Endpoint: React.FC = () => {
  const [online, setOnline] = useState(true);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);

  if (!database.isOpen()) {
    database.open();
  }

  const endpointDiagnostics = useLiveQuery(async () => {
    var data = await database.endpointDiagnostic
      .filter((u) => excludeUrls.indexOf(u.path!) === -1)
      .reverse()
      .limit(pageSize * page)
      .toArray();

    setLoading(false);
    return data;
  }, [page]);

  const count = useLiveQuery(async () =>
    database.endpointDiagnostic
      .filter((u) => excludeUrls.indexOf(u.path!) === -1)
      .count()
  );

  useEffect(() => {
    var eventSource = new EventSource(
      `${projectConfig.serverAddress}/endpoint-diagnostic-sse`
    );

    const addData = async (data: EndpointDiagnosticModel) => {
      try {
        await database.endpointDiagnostic.put(data);
      } catch (error) {
        console.log(error);
      }
    };

    eventSource.onopen = (event) => {
      setOnline(true);
    };

    eventSource.onmessage = (event) => {
      var data = JSON.parse(event.data);
      if (data.requestStartTime) {
        data.requestStartTime = new Date(data.requestStartTime);
      }
      if (data.requestEndTime) {
        data.requestEndTime = new Date(data.requestEndTime);
      }

      addData(data);
    };

    eventSource.onerror = (event) => {
      setOnline(false);
    };

    return () => {
      eventSource.close();
    };
  }, []);

  if (!endpointDiagnostics) {
    return (
      <>
        <Skeleton active />
        <Skeleton active />
      </>
    );
  }

  return (
    <Container>
      {!online && (
        <AlertBox
          message="诊断器连接失败，请确保服务器已正常启动。"
          type="warning"
          showIcon
          closable
        />
      )}
      <ItemContainer>
        {endpointDiagnostics.length === 0 ? (
          <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="暂无数据" />
        ) : (
          endpointDiagnostics &&
          endpointDiagnostics.map((item) => (
            <EndpointItem key={item.traceIdentifier} {...item} />
          ))
        )}
      </ItemContainer>
      <Operation
        onClick={() => {
          setLoading(true);
          setPage((p) => p + 1);
        }}
      >
        {count && count > endpointDiagnostics.length ? (
          <>
            {loading && <Skeleton active />}
            <Button>加载更多</Button>
          </>
        ) : count ? (
          <Typography.Text type="secondary">没有更多数据了</Typography.Text>
        ) : (
          <></>
        )}
      </Operation>
    </Container>
  );
};

export default Endpoint;
