import React from "react";

interface DiagnosisContextProps {
  showModal: () => void;
  hidePopover:()=>void;
}

const DiagnosisContext = React.createContext<DiagnosisContextProps>({
  showModal: () => {},
  hidePopover: () => {},
});

export default DiagnosisContext;
