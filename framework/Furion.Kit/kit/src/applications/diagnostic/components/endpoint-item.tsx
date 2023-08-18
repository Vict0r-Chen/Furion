import { Popover, Space } from "antd";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import { EndpointDiagnosticModel } from "../../../databases/types/endpoint.diagnostic";
import EndpointDetail from "./endpoint-detail";
import HttpMethod from "./httpmethod";
import StatusCode from "./statuscode";
import Url from "./url";

import JsonViewr from "../../../components/json-viewer";

const isError = (statusCode?: number | null): boolean => {
  if (!statusCode) {
    return false;
  }

  return statusCode >= 500 && statusCode <= 599;
};

const isWarn = (statusCode?: number | null): boolean => {
  if (!statusCode) {
    return false;
  }

  return statusCode >= 400 && statusCode <= 499;
};

const errorColor = "#ff4d4f";

const ItemContainer = styled.div`
  font-size: 14px;
`;

const Main = styled(Flexbox)<{ $error?: boolean; $warn?: boolean }>`
  background-color: ${(props) =>
    props.$error ? "#fff0f0" : props.$warn ? "#fffbe6" : "#f7f8fb"};
  align-items: center;
  padding: 5px 10px 5px 5px;
  border-radius: 5px;
`;

const JsonViewContainer = styled(Flexbox)`
  padding: 5px 25px;
  align-items: flex-start;
`;

const EndpointItem: React.FC<EndpointDiagnosticModel> = (props) => {
  return (
    <ItemContainer>
      <Main
        $spaceBetween
        $error={isError(props.statusCode)}
        $warn={isWarn(props.statusCode)}
      >
        <Space align="center">
          <IconFont
            type="icon-link"
            $size={15}
            $color={isError(props.statusCode) ? errorColor : undefined}
          />
          <HttpMethod value={props.httpMethod!} />
          <Popover
            placement="right"
            trigger="click"
            destroyTooltipOnHide
            content={<EndpointDetail {...props} />}
          >
            <Url
              underline
              $color={isError(props.statusCode) ? errorColor : "#000000e0"}
              copyable={{
                tooltips: ["复制", "复制成功"],
              }}
            >
              {decodeURIComponent(props.urlAddress!)}
            </Url>
          </Popover>
          <StatusCode
            code={props.statusCode!}
            text={props.statusText}
            message={props.exception?.message}
          />
          <TextBox $color="#00000073">
            {props.controllerAction?.displayName}
          </TextBox>
        </Space>
        <Space>
          {props.beginTimestamp && props.endTimestamp && (
            <TextBox $color="#00000073">
              {props.endTimestamp.getTime() - props.beginTimestamp.getTime()}ms
            </TextBox>
          )}
        </Space>
      </Main>
      <JsonViewContainer $spaceBetween>
        <JsonViewr value={props} keyName={props.traceIdentifier} />
      </JsonViewContainer>
    </ItemContainer>
  );
};

export default EndpointItem;
