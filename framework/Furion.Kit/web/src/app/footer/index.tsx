import { Layout } from "antd";

const { Footer } = Layout;

const footerStyle: React.CSSProperties = {
  height: 30,
  backgroundColor: "#ffffff",
};

export default function FooterPanel() {
  return <Footer style={footerStyle}></Footer>;
}