import React from "react";

interface ExploreContextProps {
  showDrawer: (name: string) => void;
}

const ExploreContext = React.createContext<ExploreContextProps>({
  showDrawer: () => {},
});

export default ExploreContext;
