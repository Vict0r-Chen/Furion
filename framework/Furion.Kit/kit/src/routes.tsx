import { createHashRouter } from "react-router-dom";
import Panel from "./applications/panel";
import Home from "./home";
import ErrorPage from "./pages/errorpage";

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
        ],
      },
    ],
  },
]);

export { router };

