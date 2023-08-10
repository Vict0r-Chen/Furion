import { create } from "zustand";

interface State {
  float: boolean;
  floatX: number;
  floatY: number;
  switchFloat: () => void;
  setPosition: (x: number, y: number) => void;
}

const useSiderStore = create<State>((set) => {
  return {
    float: false,
    floatX: 24,
    floatY: 45,
    switchFloat: () => set((draft) => ({ ...draft, float: !draft.float })),
    setPosition: (x, y) => set((draft) => ({ ...draft, floatX: x, floatY: y })),
  };
});

export default useSiderStore;
