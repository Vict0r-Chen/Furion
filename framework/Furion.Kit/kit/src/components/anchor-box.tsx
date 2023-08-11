import { Anchor, AnchorProps } from "antd";
import { AnchorContainer } from "antd/es/anchor/Anchor";
import React from "react";

const AnchorBox: React.FC<AnchorProps & { containerId?: string }> = (props) => {
  return (
    <Anchor
      affix={false}
      onClick={(ev) => ev.preventDefault()}
      getContainer={() =>
        document.getElementById(
          props.containerId || "scroll-content"
        ) as AnchorContainer
      }
      {...props}
    />
  );
};

export default AnchorBox;
