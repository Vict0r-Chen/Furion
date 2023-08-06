import { CommentOutlined } from "@ant-design/icons";
import { Space, Tooltip } from "antd";
import IconFont from "../../components/iconfont";
import { Container, Main, Menus } from "./style";

export default function Toolbar() {
  return (
    <Container width={30}>
      <Main>
        <Menus>
          <Space direction="vertical" align="center" size={15}>
            <Tooltip placement="left" title="商务洽谈">
              <CommentOutlined
                style={{
                  color: "#8c8c8c",
                  fontSize: "18px",
                  cursor: "pointer",
                }}
              />
            </Tooltip>
          </Space>
        </Menus>
        <div>
          <Space direction="vertical" align="center" size={15}>
            <Tooltip placement="left" title="中文 / English">
              <IconFont
                type="icon-language"
                style={{
                  color: "#8c8c8c",
                  fontSize: "18px",
                  cursor: "pointer",
                }}
              />
            </Tooltip>
            <Tooltip placement="left" title="用户手册">
              <a href="https://furion.net" target="_blank" rel="noreferrer">
                <IconFont
                  type="icon-documentation"
                  style={{
                    color: "#8c8c8c",
                    fontSize: "18px",
                    cursor: "pointer",
                  }}
                />
              </a>
            </Tooltip>
          </Space>
        </div>
      </Main>
    </Container>
  );
}
