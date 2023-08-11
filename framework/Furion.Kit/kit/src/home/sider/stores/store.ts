import { create } from "zustand";

interface SiderState {
  float: boolean;
  floatX: number;
  floatY: number;
  switchFloat: () => void;
  setPosition: (x: number, y: number) => void;
}

const useSiderStore = create<SiderState>((set) => {
  return {
    float: false,
    floatX: 15,
    floatY: 100,
    switchFloat: () => set((draft) => ({ ...draft, float: !draft.float })),
    setPosition: (x, y) => set((draft) => ({ ...draft, floatX: x, floatY: y })),
  };
});

export default useSiderStore;
