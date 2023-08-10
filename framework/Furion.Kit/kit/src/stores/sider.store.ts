import { create } from "zustand";

interface State {
  float: boolean;
  floatX: number;
  floatY: number;
  switchFloat: () => void;
  setPosition: (x: number, y: number) => void;
}

const useSiderStore = create<State>((set) => {
  var floatLocal = window.localStorage.getItem("float");
  var float = floatLocal ? Boolean(Number(floatLocal)) : false;

  return {
    float,
    floatX: 24,
    floatY: 45,
    switchFloat: () =>
      set((draft) => {
        var f = !draft.float;
        window.localStorage.setItem("float", f ? "1" : "0");

        return { ...draft, float: f };
      }),
    setPosition: (x, y) => set((draft) => ({ ...draft, floatX: x, floatY: y })),
  };
});

export default useSiderStore;
