import { Layout } from "antd";

const { Content } = Layout;

const contentStyle: React.CSSProperties = {
  backgroundColor: "#ffffff",
};

export default function ContentPanel() {
  return <Content style={contentStyle}></Content>;
}
