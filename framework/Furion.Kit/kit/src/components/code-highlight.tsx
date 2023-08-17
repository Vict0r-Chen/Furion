import { Highlight, HighlightProps, themes } from "prism-react-renderer";
import Prism from "prismjs";
import React from "react";
import { css, styled } from "styled-components";

require("prismjs/components/prism-csharp");

const Container = styled.div``;

const LineContainer = styled.div<{ $highlight?: boolean }>`
  ${(props) =>
    props.$highlight === true &&
    css`
      background-color: #fff0f0;
      border: 1px solid rgb(255, 214, 214);
      border-left: 0;
      border-right: 0;
    `}
`;

const LineNumber = styled.span<{ $highlight?: boolean }>`
  color: #00000073;

  ${(props) =>
    props.$highlight === true &&
    css`
      color: #ff4d4f;
    `}
`;

type CodeHighlightProps = Omit<HighlightProps, "children"> & {
  lineNumber?: number;
  startingLineNumber?: number;
};

const isSameLine = (line: number, lineNumber?: number) => {
  if (!lineNumber) {
    return false;
  }

  return lineNumber === line;
};

const CodeHighlight: React.FC<CodeHighlightProps> = (props) => {
  const { lineNumber, startingLineNumber = 1 } = props;

  return (
    <Container>
      <Highlight theme={themes.vsLight} prism={Prism} {...props}>
        {({ className, style, tokens, getLineProps, getTokenProps }) => (
          <pre style={style}>
            {tokens.map((line, i) => (
              <LineContainer
                key={i}
                {...getLineProps({ line })}
                $highlight={isSameLine(i + startingLineNumber, lineNumber)}
              >
                <LineNumber
                  $highlight={isSameLine(i + startingLineNumber, lineNumber)}
                >
                  {i + startingLineNumber}
                </LineNumber>
                {line.map((token, key) => (
                  <span key={key} {...getTokenProps({ token })} />
                ))}
              </LineContainer>
            ))}
          </pre>
        )}
      </Highlight>
    </Container>
  );
};

export default CodeHighlight;
