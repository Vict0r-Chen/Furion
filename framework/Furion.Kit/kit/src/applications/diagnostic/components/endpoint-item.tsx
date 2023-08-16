import { Popover, Space } from "antd";
import { useState } from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
import { EndpointDiagnosticModel } from "../../../databases/types/endpoint.diagnostic";
import Mdx from "../../../mdxs/index.mdx";
import HttpMethod from "./httpmethod";
import StatusCode from "./statuscode";

const ItemContainer = styled.div`
  font-size: 13px;
`;

const Main = styled(Flexbox)`
  background-color: #f7f8fb;
  align-items: center;
  padding: 5px 10px 5px 5px;
  border-radius: 5px;
`;

const Url = styled(TextBox)`
  font-size: 14px;
  cursor: pointer;

  &:hover {
    color: #1677ff;
  }
`;

const JsonView = styled(Flexbox)`
  padding: 5px 25px;
  align-items: flex-start;
`;

const EndpointItem: React.FC<EndpointDiagnosticModel> = (props) => {
  const [showPopover, setShowPopover] = useState(false);

  return (
    <ItemContainer>
      <Main $spaceBetween>
        <Space align="center">
          <IconFont type="icon-link" />
          <HttpMethod value={props.httpMethod!} />
          <Popover
            placement="right"
            trigger="click"
            destroyTooltipOnHide
            content={
              <div style={{ width: 400, height: 300, overflowY: "auto" }}>
                <Mdx />
              </div>
            }
            onOpenChange={(v) => {
              setShowPopover(v);
            }}
          >
            <Url
              underline
              $color={showPopover ? "#1677ff" : "#000000e0"}
              copyable
            >
              {decodeURIComponent(props.urlAddress!)}
            </Url>
          </Popover>
          <StatusCode code={props.statusCode!} text={props.statusText} />
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
      <JsonView $spaceBetween>
        <ReactJson collapsed src={props} name={props.traceIdentifier} />
      </JsonView>
    </ItemContainer>
  );
};

export default EndpointItem;
