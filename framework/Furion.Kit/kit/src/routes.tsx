import { createHashRouter } from "react-router-dom";
import Diagnostic from "./applications/diagnostic";
import Discussion from "./applications/discussion";
import Explore, { ExploreDetail } from "./applications/explore";
import Generate from "./applications/generate";
import OpenAPI from "./applications/openapi";
import Panel from "./applications/panel";
import Setting from "./applications/setting";
import Starter from "./applications/starter";
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
            path: "diagnostic",
            element: <Diagnostic />,
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
            path: "discussion",
            element: <Discussion />,
          },
          {
            path: "explore",
            element: <Explore />,
            children: [
              {
                path: "detail/:name",
                element: <ExploreDetail />,
              },
            ],
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

