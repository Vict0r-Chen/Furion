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
  controllerType?: string;
  methodName?: string;
  query?: KeyValueModel[];
  cookies?: KeyValueModel[];
  headers: KeyValueModel[];
}

export interface EndpointModel {
  displayName?: string;
  routePattern?: string;
  order: number;
  httpMethods?: string;
}

export interface KeyValueModel {
  key: string;
  value?: string;
}

export class MySubClassedDexie extends Dexie {
  httpDiagnost!: Table<HttpDiagnost>;

  constructor() {
    super("furion-kit");
    this.version(1).stores({
      httpDiagnost:
        "++traceIdentifier,  requestPath, requestMethod, exception, statusCode, endpoint, startTimestamp, endTimestamp, displayName, controllerType, methodName, query, cookies, headers",
    });
  }
}

export const db = new MySubClassedDexie();
