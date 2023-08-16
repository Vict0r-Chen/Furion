import Dexie, { Table } from "dexie";
import EndpointDiagnostic from "./types/endpoint.diagnostic";

export class FurionKitDexie extends Dexie {
  constructor() {
    super("furion-kit");
    this.version(1).stores({});
  }

  endpointDiagnostic!: Table<EndpointDiagnostic>;
}

export const database = new FurionKitDexie();
