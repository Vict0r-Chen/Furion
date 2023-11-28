import Link from "@docusaurus/Link";
import useBaseUrl from "@docusaurus/useBaseUrl";
import { useContext } from "react";
import GlobalContext from "./GlobalContext";

export default function VipImageList({ padding = 5 }) {
  const { setDonate } = useContext(GlobalContext);

  return (
    <>
    </>
  );
}

function Image({ index, padding }) {
  return (
    <div
      style={{
        flex: 1,
        margin: `${padding}px ${padding}px 5px 0`,
        userSelect: "none",
        pointerEvents: "none",
      }}
    >
      <img
        src={useBaseUrl("img/vip" + index + ".jpeg")}
        style={{
          width: "100%",
          height: "100%",
          display: "block",
          borderRadius: 8,
        }}
      />
    </div>
  );
}
