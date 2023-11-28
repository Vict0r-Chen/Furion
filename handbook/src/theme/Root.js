import BrowserOnly from "@docusaurus/BrowserOnly";
import Link from "@docusaurus/Link";
import useBaseUrl from "@docusaurus/useBaseUrl";
import Modal from "@uiw/react-modal";
import React, { useContext, useEffect, useState } from "react";
import Assistance from "../components/Assistance";
import FloatBar from "../components/FloatBar";
import GlobalContext from "../components/GlobalContext";
import Vip from "../components/Vip";
import VipDesc from "../components/VipDesc.mdx";
import VipImageList from "../components/VipImageList";

function Root({ children }) {
  const [donate, setDonate] = useState(false);
  const [showVip, setVip] = useState(false);
  const [adv, setAdv] = useState(true);
  const [drawer, showDrawer] = useState(true); // 弹窗
  const [rightVip, setRightVip] = useState(false);
  const [topVip, setTopVip] = useState(true);

  const onClosed = () => {
    setDonate(false);
  };

  useEffect(() => {
    if (!drawer) {
      setTimeout(() => {
        setTopVip(false);
      }, 5000);
    }
  }, [drawer]);

  return (
    <GlobalContext.Provider
      value={{
        showDrawer,
        rightVip,
        setRightVip,
        topVip,
        setTopVip,
      }}
    >
      <FloatBar />
      {/* {!drawer && topVip && <TopBanner />} */}
      {children}
      <BrowserOnly children={() => <VipShow />} />
    </GlobalContext.Provider>
  );
}

function VipShow() {
  const { drawer, showDrawer, setVip, setTopVip } = useContext(GlobalContext);

  return (
    <>
    </>
  );
}

export default Root;
