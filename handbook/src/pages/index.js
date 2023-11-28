import Link from "@docusaurus/Link";
import { useColorMode } from "@docusaurus/theme-common";
import useBaseUrl from "@docusaurus/useBaseUrl";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Layout from "@theme/Layout";
import components from "@theme/MDXComponents";
import Tooltip from "@uiw/react-tooltip";
import React, { useContext } from "react";
import Assistance from "../components/Assistance";
import Donate from "../components/Donate";
import GlobalContext from "../components/GlobalContext";
import SpecDonate from "../components/SpecDonate";
import urls from "../data/urls";
import DockerIcon from "./docker.svg";
import "./index.css";
import "./index.own.css";
import KubernetesIcon from "./kubernetes.svg";
import LinuxIcon from "./linux.svg";
import MacOSIcon from "./macos.svg";
import NetConf from "./netconf-bot.svg";
import WindowIcon from "./windows.svg";

function Home() {
  const context = useDocusaurusContext();
  const { siteConfig = {} } = context;

  return (
    <Layout
      title={`让 .NET 开发更简单，更通用，更流行。 ${siteConfig.title}`}
      description="让 .NET 开发更简单，更通用，更流行。"
    >
      <Banner />
      <Gitee />
    </Layout>
  );
}

function Banner() {
  return (
    <div className="furion-banner">
      <div className="furion-banner-container">
        <img
          src={useBaseUrl("img/netconf-drone.gif")}
          className="furioin-index-gif"
          style={{ pointerEvents: "none" }}
        />
      </div>
    </div>
  );
}

function Gitee() {
  const { colorMode, setLightTheme, setDarkTheme } = useColorMode();
  const isDarkTheme = colorMode === "dark";

  return (
    <div className="furion-content">
      <p className={"furion-small-title" + (isDarkTheme ? " dark" : "")}>
        MIT 宽松开源协议/商用项目首选
      </p>
      <h1 className={"furion-big-title" + (isDarkTheme ? " dark" : "")}>
        ⭐️ MIT 开源协议，代码在 Gitee/GitHub 平台托管 ⭐️
      </h1>
      <div className="furion-gitee-log">
        <div
          className="furion-log-item"
          style={{ border: "6px solid #723cff" }}
        >
          <div
            className={"furion-log-jiao" + (isDarkTheme ? " dark" : "")}
          ></div>
          <div className="furion-log-number">
            <div style={{ color: "#723cff" }}>12,000 +</div>
            <span className={isDarkTheme ? " dark" : ""}>Stars</span>
          </div>
        </div>
        <div
          className="furion-log-item"
          style={{ border: "6px solid #3fbbfe" }}
        >
          <div
            className={"furion-log-jiao" + (isDarkTheme ? " dark" : "")}
          ></div>
          <div className="furion-log-number">
            <div style={{ color: "#3fbbfe" }}>4,200 +</div>
            <span className={isDarkTheme ? " dark" : ""}>Forks</span>
          </div>
        </div>
        <div
          className="furion-log-item"
          style={{ border: "6px solid #1fd898" }}
        >
          <div
            className={"furion-log-jiao" + (isDarkTheme ? " dark" : "")}
          ></div>
          <div className="furion-log-number">
            <div style={{ color: "#1fd898" }}>11,648,355</div>
            <span className={isDarkTheme ? " dark" : ""}>Downloads</span>
          </div>
        </div>
      </div>
    </div>
  );
}


function CodeSection(props) {
  let { language, replace, section, source } = props;

  source = source.replace(/\/\/ <.*?\n/g, "");

  if (replace) {
    for (const [pattern, value] of Object.entries(replace)) {
      source = source.replace(new RegExp(pattern, "gs"), value);
    }
  }

  source = source.trim();
  if (!source.includes("\n")) {
    source += "\n";
  }

  return (
    <components.pre>
      <components.code
        children={source}
        className={`language-${language}`}
        mdxType="code"
        originalType="code"
        parentName="pre"
      />
    </components.pre>
  );
}

function SystemWindow(systemWindowProps) {
  const { children, className, ...props } = systemWindowProps;
  return (
    <div
      {...props}
      className={"system-window blue-accent preview-border " + className}
    >
      <div className="system-top-bar">
        <span
          className="system-top-bar-circle"
          style={{ backgroundColor: "#8759ff" }}
        />
        <span
          className="system-top-bar-circle"
          style={{ backgroundColor: "#3fc4fe" }}
        />
        <span
          className="system-top-bar-circle"
          style={{ backgroundColor: "#42ffac" }}
        />
      </div>
      {children}
    </div>
  );
}

function Bifa() {
  return (
    <>
    </>
  );
}

function Wzi(props) {
  return <div className="furion-wzi">{props.children}</div>;
}

function FuChi() {
  const { colorMode, setLightTheme, setDarkTheme } = useColorMode();
  const isDarkTheme = colorMode === "dark";
  return (
    <>
    </>
  );
}

export default Home;
