import React from "react";

export default function ReplitEmbed(
  props: React.IframeHTMLAttributes<HTMLIFrameElement>
) {
  return (
    <iframe
      height="400"
      width="100%"
      src="https://replit.com/@moderation/Welcome?embed=1"
      scrolling="no"
      frameBorder="no"
      allowTransparency
      allowFullScreen
      sandbox="allow-forms allow-pointer-lock allow-popups allow-same-origin allow-scripts allow-modals"
    />
  );
}
