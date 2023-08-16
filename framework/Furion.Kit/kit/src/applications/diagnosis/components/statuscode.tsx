import {
    CheckCircleOutlined,
    CloseCircleOutlined,
    ExclamationCircleOutlined,
    InfoCircleOutlined,
} from "@ant-design/icons";
import { Space } from "antd";
import React from "react";
import { styled } from "styled-components";
import TextBox from "../../../components/textbox";

const Container = styled.div`
  display: inline-block;
  font-size: 13px;
`;

export const getIconColor = (code: number): [JSX.Element, string] => {
  if (code >= 200 && code <= 299) {
    return [<CheckCircleOutlined />, "#52c41a"];
  } else if (code >= 400 && code <= 499) {
    return [<ExclamationCircleOutlined />, "#faad14"];
  } else if (code >= 500 && code <= 599) {
    return [<CloseCircleOutlined />, "#ff4d4f"];
  } else {
    return [<InfoCircleOutlined />, "#1677ff"];
  }
};

interface StatusCodeProps {
  code: number;
  text?: string;
}

const StatusCode: React.FC<StatusCodeProps> = ({ code, text = "" }) => {
  const [icon, color] = getIconColor(code);
  const IconColor = React.cloneElement<any>(icon, { style: { color } });

  return (
    <Container>
      <Space align="center" size={5}>
        {IconColor}
        <TextBox $color={color}>
          {code} {text}
        </TextBox>
      </Space>
    </Container>
  );
};

export default StatusCode;
