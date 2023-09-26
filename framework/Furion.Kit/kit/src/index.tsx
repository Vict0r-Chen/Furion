import { i18n } from "@lingui/core";
import { I18nProvider } from "@lingui/react";
import "github-markdown-css";
import React from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider } from "react-router-dom";
import { i18nInit } from "./i18n";
import "./index.css";
import { router } from "./routes";

i18nInit("zh-hans");

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLDivElement
);

root.render(
  <React.StrictMode>
    <I18nProvider i18n={i18n}>
      <RouterProvider router={router} />
    </I18nProvider>
  </React.StrictMode>
);
