export interface EndpointDiagnosticModel {
  traceId?: string;
  traceIdentifier?: string;
  httpMethod?: string;
  path?: string;
  urlAddress?: string;
  statusCode?: number;
  statusText?: string;
  contentType?: string;
  requestStartTime?: Date;
  requestEndTime?: Date;
  query?: Record<string, string | null>;
  cookies?: Record<string, string | null>;
  requestHeaders?: Record<string, string | null>;
  responseHeaders?: Record<string, string | null>;
  routeValues?: Record<string, any>;
  endpoint?: EndpointModel;
  exception?: ExceptionModel;
  controllerAction?: ControllerActionModel;
  filters?: string[];
}

export interface ControllerActionModel {
  controllerName?: string;
  actionName?: string;
  controllerType?: string;
  methodName?: string;
  signature?: string;
  displayName?: string;
}

export interface EndpointModel {
  displayName?: string;
  routePattern?: string;
  order?: number;
  httpMethods?: string;
}

export interface ExceptionModel {
  message?: string;
  stackTrace?: string;
  hResult?: number;
  source?: string;
  helpLink?: string;
  type?: string;
  rawText?: string;
  details?: ExceptionSourceCode[];
}

export interface ExceptionSourceCode {
  fileName?: string;
  lineNumber?: number;
  startingLineNumber?: number;
  surroundingLinesText?: string;
  targetLineText?: string;
}
