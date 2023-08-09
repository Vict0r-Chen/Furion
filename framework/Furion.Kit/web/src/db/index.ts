import Dexie, { Table } from "dexie";

export interface HttpDiagnost {
  traceIdentifier: string;
  requestPath: string;
  requestMethod: string;
  exception?: string;
  statusCode?: number;
  endpoint?: EndpointModel;
  startTimestamp?: Date;
  endTimestamp?: Date;
  displayName?: string;
}

export interface EndpointModel {
  displayName?: string;
  routePattern?: string;
  order: number;
  httpMethods?: string;
}

export class MySubClassedDexie extends Dexie {
  httpDiagnost!: Table<HttpDiagnost>;

  constructor() {
    super("furion-kit");
    this.version(1).stores({
      httpDiagnost:
        "++traceIdentifier,  requestPath, requestMethod, exception, statusCode, endpoint, startTimestamp, endTimestamp, displayName",
    });
  }
}

export const db = new MySubClassedDexie();
