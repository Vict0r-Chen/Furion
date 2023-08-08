import Dexie, { Table } from "dexie";

export interface HttpDiagnost {
  id?: number;
  traceIdentifier: string;
  requestPath: string;
  exception?: string;
}

export class MySubClassedDexie extends Dexie {
  httpDiagnost!: Table<HttpDiagnost>;

  constructor() {
    super("furion-kit");
    this.version(1).stores({
      httpDiagnost: "++id, traceIdentifier, requestPath, exception",
    });
  }
}

export const db = new MySubClassedDexie();
