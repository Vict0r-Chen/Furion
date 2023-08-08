import { useLiveQuery } from "dexie-react-hooks";
import { db } from "../../../db";

export default function RequestList() {
  const httpDiagnost = useLiveQuery(async () => {
    return await db.httpDiagnost.reverse().toArray();
  });

  if (!httpDiagnost) {
    return <div>加载中...</div>;
  }

  return (
    <div>
      {httpDiagnost?.map((item) => (
        <div key={item.traceIdentifier}>
          {item.traceIdentifier} {item.requestPath}
        </div>
      ))}
    </div>
  );
}