import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider } from "react-router-dom";
import "./index.css";
import { router } from "./routes";

import { i18n } from "@lingui/core";
import { I18nProvider } from "@lingui/react";
import { defaultLocale, dynamicActivate } from "./i18n";

dynamicActivate(defaultLocale);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <I18nProvider i18n={i18n}>
      <RouterProvider router={router} />
    </I18nProvider>
  </React.StrictMode>
);
