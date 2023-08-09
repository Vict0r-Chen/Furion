import { create } from "zustand";

interface ServerState {
  online: boolean;
  setState: (state: boolean) => void;
}

const useServerStore = create<ServerState>((set) => ({
  online: true,
  setState: (state) => set((draft) => ({ online: state })),
}));

export { useServerStore };

