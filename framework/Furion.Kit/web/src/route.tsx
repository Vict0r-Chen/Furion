import { createHashRouter } from "react-router-dom";
import App from "./app";
import Component from "./pages/component";
import Console from "./pages/console";
import Diagnosis from "./pages/diagnosis";
import ErrorPage from "./pages/error-page";
import Explore from "./pages/explore";
import Generate from "./pages/generate";
import OpenAPI from "./pages/openapi";
import Panel from "./pages/panel";
import Setting from "./pages/setting";
import Starter from "./pages/starter";
import SystemInfo from "./pages/systeminfo";

const router = createHashRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
    children: [
      {
        errorElement: <ErrorPage />,
        children: [
          {
            index: true,
            element: <Panel />,
          },
          {
            path: "component",
            element: <Component />,
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
            path: "starter",
            element: <Starter />,
          },
          {
            path: "generate",
            element: <Generate />,
          },
          {
            path: "systeminfo",
            element: <SystemInfo />,
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

export default router;
