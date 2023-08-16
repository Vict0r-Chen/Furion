import { Highlight, themes } from "prism-react-renderer";
import Prism from "prismjs";
import React from "react";
import { styled } from "styled-components";
import { EndpointDiagnosticModel } from "../../../databases/types/endpoint.diagnostic";

require("prismjs/components/prism-csharp");

const Container = styled.div`
  max-height: 300px;
  overflow-y: auto;
`;

const Category: React.FC<{
  title?: React.ReactNode;
  children?: React.ReactNode;
}> = ({ title, children }) => {
  return (
    <div>
      <div style={{ fontWeight: 600, backgroundColor: "rgb(255, 251, 230)" }}>
        {title}
      </div>
      <div style={{ padding: "0 10px" }}>{children}</div>
    </div>
  );
};

const EndpointDetail: React.FC<EndpointDiagnosticModel> = (props) => {
  return (
    <Container>
      <Category title="ControllerAction">
        {props.controllerAction &&
          Object.keys(props.controllerAction).map((item) => (
            <div key={item}>
              {item} {(props.controllerAction as Record<string, any>)![item]}
            </div>
          ))}
      </Category>
      <Category title="Endpoint">
        {props.endpoint &&
          Object.keys(props.endpoint).map((item) => (
            <div key={item}>
              {item} {(props.endpoint as Record<string, any>)![item]}
            </div>
          ))}
      </Category>
      <Category title="Query">
        {props.query &&
          Object.keys(props.query).map((item) => (
            <div key={item}>
              {item} {props.query![item]}
            </div>
          ))}
      </Category>
      <Category title="Request Headers">
        {props.requestHeaders &&
          Object.keys(props.requestHeaders).map((item) => (
            <div key={item}>
              {item} {props.requestHeaders![item]}
            </div>
          ))}
      </Category>
      <Category title="Cookies">
        {props.cookies &&
          Object.keys(props.cookies).map((item) => (
            <div key={item}>
              {item} {props.cookies![item]}
            </div>
          ))}
      </Category>
      <Category title="Route Values">
        {props.routeValues &&
          Object.keys(props.routeValues).map((item) => (
            <div key={item}>
              {item} {props.routeValues![item]}
            </div>
          ))}
      </Category>
      <Category title="Exception">
        {props.exception &&
          Object.keys(props.exception)
            .filter((k) => k !== "details")
            .map((item) => (
              <div key={item}>
                {item} {(props.exception as Record<string, any>)![item]}
              </div>
            ))}
      </Category>
      <Category title="Stack">
        {props.exception?.details &&
          props.exception.details.map((item, z) => (
            <div key={z}>
              <div>
                {item.fileName} {item.lineNumber}
              </div>
              <div>{item.targetLineText!}</div>
              <div>
                <Highlight
                  theme={themes.vsLight}
                  code={item.surroundingLinesText!}
                  language="cs"
                  prism={Prism}
                >
                  {({
                    className,
                    style,
                    tokens,
                    getLineProps,
                    getTokenProps,
                  }) => (
                    <pre style={style}>
                      {tokens.map((line, i) => (
                        <div
                          key={i}
                          {...getLineProps({ line })}
                          style={{
                            backgroundColor:
                              i + (item.startLineNumber || 1) ===
                              item.lineNumber
                                ? "#ff4d4f"
                                : undefined,
                          }}
                        >
                          <span>{i + (item.startLineNumber || 1)}</span>
                          {line.map((token, key) => (
                            <span key={key} {...getTokenProps({ token })} />
                          ))}
                        </div>
                      ))}
                    </pre>
                  )}
                </Highlight>
              </div>
            </div>
          ))}
      </Category>
    </Container>
  );
};

export default EndpointDetail;
