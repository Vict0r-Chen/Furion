import { createHashRouter } from "react-router-dom";
import Home from "./home";

const router = createHashRouter([
  {
    path: "/",
    element: <Home />,
  },
]);

export { router };

