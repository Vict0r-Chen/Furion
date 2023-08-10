import { create } from "zustand";

interface State {
  float: boolean;
  switchFloat: () => void;
}

const useStore = create<State>((set) => {
  var floatLocal = window.localStorage.getItem("float");
  var float = floatLocal ? Boolean(Number(floatLocal)) : false;

  return {
    float,
    switchFloat: () =>
      set((draft) => {
        var f = !draft.float;
        window.localStorage.setItem("float", f ? "1" : "0");

        return { ...draft, float: f };
      }),
  };
});

export default useStore;
