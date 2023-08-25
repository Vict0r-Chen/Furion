import { Empty, Typography } from "antd";
import React from "react";
import { styled } from "styled-components";
import CodeHighlight from "../../../components/code-highlight";
import HttpMethod from "../../../components/http-method";
import TextBox from "../../../components/textbox";
import { EndpointDiagnosticModel } from "../../../databases/types/endpoint.diagnostic";
import StatusCode from "./statuscode";
import Url from "./url";

const Container = styled.div`
  max-height: calc(100vh - 300px);
  overflow-y: auto;
`;

const CategoryContainer = styled.div`
  letter-spacing: 0.5px;
`;

const CategoryTitle = styled.div`
  font-weight: 600;
  background-color: #f1f6fd;
  padding: 5px 15px;
  border-radius: 5px;
`;

const CategoryContent = styled.div`
  padding: 10px 20px;
`;

const Category: React.FC<{
  title?: React.ReactNode;
  children?: React.ReactNode;
}> = ({ title, children }) => {
  return (
    <CategoryContainer>
      <CategoryTitle>{title}</CategoryTitle>
      <CategoryContent>
        {children ? children : <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />}
      </CategoryContent>
    </CategoryContainer>
  );
};

const ItemContainer = styled.div`
  padding: 2px 0;
`;

const ItemTitle = styled.span`
  color: #000000e0;
  font-weight: 600;
  margin-right: 2px;
  display: inline-block;
  width: 200px;
`;

const ItemContent = styled(TextBox)`
  color: #595959;
`;

const ListItem: React.FC<{
  title?: React.ReactNode;
  content?: React.ReactNode;
}> = ({ title, content }) => {
  return (
    <ItemContainer>
      {title && <ItemTitle>{title}</ItemTitle>}
      <ItemContent>{content}</ItemContent>
    </ItemContainer>
  );
};

const EndpointDetail: React.FC<EndpointDiagnosticModel> = (props) => {
  return (
    <Container>
      <Category title="Basic">
        <ListItem
          title="requestStartTime"
          content={props.requestStartTime?.toLocaleString()}
        />
        <ListItem title="contentType" content={props.contentType} />
        <ListItem
          title="requestEndTime"
          content={props.requestEndTime?.toLocaleString()}
        />
        <ListItem
          title="httpMethod"
          content={<HttpMethod value={props.httpMethod!} />}
        />
        <ListItem
          title="status"
          content={
            <StatusCode code={props.statusCode!} text={props.statusText} />
          }
        />
        <ListItem title="traceIdentifier" content={props.traceIdentifier} />
        <ListItem title="path" content={props.path} />
        <ListItem
          title="urlAddress"
          content={
            <Url
              underline
              $color={"#000000e0"}
              copyable={{
                tooltips: ["复制", "复制成功"],
              }}
            >
              {decodeURIComponent(props.urlAddress!)}
            </Url>
          }
        />
      </Category>
      <Category title="ControllerAction">
        {props.controllerAction &&
          Object.keys(props.controllerAction).map((item) => (
            <ListItem
              key={item}
              title={item}
              content={(props.controllerAction as Record<string, any>)![item]}
            />
          ))}
      </Category>
      <Category title="Cookies">
        {props.cookies &&
          Object.keys(props.cookies).map((item) => (
            <ListItem key={item} title={item} content={props.cookies![item]} />
          ))}
      </Category>
      <Category title="Endpoint">
        {props.endpoint &&
          Object.keys(props.endpoint).map((item) => (
            <ListItem
              key={item}
              title={item}
              content={(props.endpoint as Record<string, any>)![item]}
            />
          ))}
      </Category>
      <Category title="Exception">
        {props.exception && (
          <>
            {Object.keys(props.exception)
              .filter(
                (k) => k !== "details" && k !== "stackTrace" && k !== "rawText"
              )
              .map((item) => (
                <ListItem
                  key={item}
                  title={item}
                  content={(props.exception as Record<string, any>)![item]}
                />
              ))}
            <CodeHighlight
              language="cs"
              code={(props.exception?.rawText || props.exception.stackTrace)!}
            />
          </>
        )}
      </Category>
      <Category title="Stack">
        {props.exception?.details &&
          props.exception.details.map((item, i) => (
            <div key={i}>
              <ListItem
                content={
                  <Typography.Link
                    underline
                    copyable={{
                      tooltips: ["复制", "复制成功"],
                    }}
                  >
                    {item.fileName}
                  </Typography.Link>
                }
              />
              <CodeHighlight
                language="cs"
                code={item.surroundingLinesText!}
                lineNumber={item.lineNumber}
                startingLineNumber={item.startingLineNumber}
              />
            </div>
          ))}
      </Category>
      <Category title="Filters">
        {props.filters &&
          props.filters.length > 0 &&
          props.filters.map((item) => <ListItem key={item} content={item} />)}
      </Category>
      <Category title="Query">
        {props.query &&
          Object.keys(props.query).length > 0 &&
          Object.keys(props.query).map((item) => (
            <ListItem key={item} title={item} content={props.query![item]} />
          ))}
      </Category>
      <Category title="Request Headers">
        {props.requestHeaders &&
          Object.keys(props.requestHeaders).length > 0 &&
          Object.keys(props.requestHeaders).map((item) => (
            <ListItem
              key={item}
              title={item}
              content={props.requestHeaders![item]}
            />
          ))}
      </Category>
      <Category title="Response Headers">
        {props.responseHeaders &&
          Object.keys(props.responseHeaders).length > 0 &&
          Object.keys(props.responseHeaders).map((item) => (
            <ListItem
              key={item}
              title={item}
              content={props.responseHeaders![item]}
            />
          ))}
      </Category>
      <Category title="Route Values">
        {props.routeValues &&
          Object.keys(props.routeValues).length > 0 &&
          Object.keys(props.routeValues).map((item) => (
            <ListItem
              key={item}
              title={item}
              content={props.routeValues![item]}
            />
          ))}
      </Category>
    </Container>
  );
};

export default EndpointDetail;
