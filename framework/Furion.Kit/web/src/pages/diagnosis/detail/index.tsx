import { Spin } from "antd";
import { useLiveQuery } from "dexie-react-hooks";
import { useParams } from "react-router-dom";
import { db } from "../../../db";

export default function DiagnosisDetail() {
  let { id } = useParams();

  const diagnosis = useLiveQuery(
    async () => await db.httpDiagnost.get({ traceIdentifier: id }),
    [id]
  );

  if (!diagnosis) {
    return <Spin />;
  }

  return <div>{diagnosis.requestPath}.</div>;
}
