import Dexie, { Table } from "dexie";
import { RoutingDiagnosis } from "./types/routingDiagnosis";

export class FurionKitDexie extends Dexie {
  constructor() {
    super("furion-kit");
    this.version(1).stores({});
  }

  routingDiagnosis!: Table<RoutingDiagnosis>;
}

export const database = new FurionKitDexie();
