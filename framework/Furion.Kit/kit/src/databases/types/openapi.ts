export interface OpenApiModel {
  groups?: OpenApiGroup[];
}

export interface OpenApiGroup {
  name?: string;
  tags?: OpenApiTag[];
}

export interface OpenApiTag {
  name?: string;
  descriptions?: OpenApiDescription[];
}

export interface OpenApiDescription {
  id?: string;
  groupName?: string;
  httpMethod?: string;
  relativePath?: string;
  allowAnonymous?: boolean;
}
