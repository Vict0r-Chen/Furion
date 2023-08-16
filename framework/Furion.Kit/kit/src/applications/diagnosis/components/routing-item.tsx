import { Popover, Space } from "antd";
import { useState } from "react";
import ReactJson from "react-json-view";
import { styled } from "styled-components";
import Flexbox from "../../../components/flexbox";
import IconFont from "../../../components/iconfont";
import TextBox from "../../../components/textbox";
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
    color: #000000e0;
  }
`;

const JsonView = styled(Flexbox)`
  padding: 5px 25px;
  align-items: flex-start;
`;

interface RoutingItemProps {
  link: string;
  httpMethod: string;
}

const RoutingItem: React.FC<RoutingItemProps> = ({ link, httpMethod }) => {
  const [showPopover, setShowPopover] = useState(false);

  return (
    <ItemContainer>
      <Main $spaceBetween>
        <Space align="center">
          <IconFont type="icon-link" />
          <HttpMethod value={httpMethod} />
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
              $color={showPopover ? "#1677ff" : "#000000A6"}
              copyable
            >
              {link}
            </Url>
          </Popover>
          <StatusCode code={200} text="OK" />
          <TextBox $color="#00000073">HTTP SSE 请求</TextBox>
        </Space>
        <Space>
          <TextBox $color="#00000073">100ms</TextBox>
        </Space>
      </Main>
      <JsonView $spaceBetween>
        <ReactJson
          collapsed
          src={{
            name: "Furion",
            age: 31,
            version: "0.0.1",
          }}
          name="0HMSRFDAEBL6G:0000002B"
        />
      </JsonView>
    </ItemContainer>
  );
};

export default RoutingItem;
