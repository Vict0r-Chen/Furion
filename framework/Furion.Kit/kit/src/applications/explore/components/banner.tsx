import { Image, ImageProps } from "antd";
import React from "react";
import { styled } from "styled-components";

const Img = styled(Image)`
  height: 100%;
  width: 100%;
  display: block;
`;

const Banner: React.FC<ImageProps> = (props) => {
  return <Img preview={false} {...props} />;
};

export default Banner;
