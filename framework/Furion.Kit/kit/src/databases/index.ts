import Dexie, { Table } from "dexie";
import { EndpointDiagnosticModel } from "./types/endpoint.diagnostic";

export class FurionKitDexie extends Dexie {
  constructor() {
    super("furion-kit");

    this.version(1).stores({
      endpointDiagnostic: "++traceIdentifier",
    });
  }

  endpointDiagnostic!: Table<EndpointDiagnosticModel>;
}

export const database = new FurionKitDexie();
