import React from "react";
import { AppCardType } from "../components/app-card";

interface ExploreContextProps {
  showDrawer: (name: string) => void;
  listType?: AppCardType;
}

const ExploreContext = React.createContext<ExploreContextProps>({
  showDrawer: () => {},
  listType: "card",
});

export default ExploreContext;
