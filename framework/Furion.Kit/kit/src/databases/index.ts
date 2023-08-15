import Dexie from "dexie";

export class FurionKitDexie extends Dexie {
  constructor() {
    super("furion-kit");
  }
}

export const database = new FurionKitDexie();
