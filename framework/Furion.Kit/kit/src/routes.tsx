import { createHashRouter } from "react-router-dom";
import Component from "./applications/component";
import Configuration from "./applications/configuration";
import Console from "./applications/console";
import Diagnosis from "./applications/diagnosis";
import Explore from "./applications/explore";
import Generate from "./applications/generate";
import OpenAPI from "./applications/openapi";
import Panel from "./applications/panel";
import Setting from "./applications/setting";
import Starter from "./applications/starter";
import SystemInfo from "./applications/systeminfo";
import Home from "./home";
import ErrorPage from "./pages/error-page";

const router = createHashRouter([
  {
    path: "/",
    element: <Home />,
    errorElement: <ErrorPage />,
    children: [
      {
        children: [
          {
            index: true,
            element: <Panel />,
          },
          {
            path: "console",
            element: <Console />,
          },
          {
            path: "diagnosis",
            element: <Diagnosis />,
          },
          {
            path: "openapi",
            element: <OpenAPI />,
          },
          {
            path: "systeminfo",
            element: <SystemInfo />,
          },
          {
            path: "component",
            element: <Component />,
          },
          {
            path: "configuration",
            element: <Configuration />,
          },
          {
            path: "starter",
            element: <Starter />,
          },
          {
            path: "generate",
            element: <Generate />,
          },
          {
            path: "explore",
            element: <Explore />,
          },
          {
            path: "setting",
            element: <Setting />,
          },
        ],
      },
    ],
  },
]);

export { router };

