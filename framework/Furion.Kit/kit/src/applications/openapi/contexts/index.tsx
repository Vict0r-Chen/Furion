import React from "react";
import { OpenApiDescription } from "../../../databases/types/openapi";

interface OpenApiGroupContextProps {
  apiDescription?: OpenApiDescription;
  setApiDescription: (data: OpenApiDescription) => void;
}

const OpenApiGroupContext = React.createContext<OpenApiGroupContextProps>({
  setApiDescription: () => {},
});

export default OpenApiGroupContext;
