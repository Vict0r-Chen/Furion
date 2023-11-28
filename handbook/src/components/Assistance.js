import Link from "@docusaurus/Link";
import useBaseUrl from "@docusaurus/useBaseUrl";
import Tooltip from "@uiw/react-tooltip";
import React from "react";
import classes from "./Assistance.module.css";

export default function Assistance({ style = {}, onClick }) {
  const count = 344;
  const tip = "已有 " + count + " 位用户开通 VIP 服务";

  return (
    <Tooltip placement="top" autoAdjustOverflow>
    </Tooltip>
  );
}
