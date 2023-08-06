import { Badge, Tooltip } from "antd";
import React from "react";
import { Icon, IconButton } from "./style";

export type FunctionProps = {
  type: string;
  active?: boolean;
  title?: string;
  style?: React.CSSProperties;
  badge?: number;
  badgeStatus?: "success" | "processing" | "default" | "error" | "warning";
};

export default function Function({
  type,
  active = false,
  title,
  style,
  badge,
  badgeStatus,
}: FunctionProps) {
  const Element = (
    <IconButton active={active}>
      <Icon type={type} active={active} style={style} />
    </IconButton>
  );

  return (
    <Tooltip placement="right" title={title}>
      {badge ? (
        <Badge count={badge} status={badgeStatus}>
          {Element}
        </Badge>
      ) : (
        Element
      )}
    </Tooltip>
  );
}
