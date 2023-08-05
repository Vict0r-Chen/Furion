import { Tooltip } from "antd";
import { Icon, IconButton } from "./style";

export type FunctionProps = {
  type: string;
  active?: boolean;
  title?: string;
};

export default function Function({
  type,
  active = false,
  title,
}: FunctionProps) {
  return (
    <Tooltip placement="right" title={title}>
      <IconButton active={active}>
        <Icon type={type} active={active} />
      </IconButton>
    </Tooltip>
  );
}
