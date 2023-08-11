import { Popover, Space } from "antd";
import IconFont from "../../components/iconfont";
import TextBox from "../../components/textbox";
import Content from "../../home/content";

const Panel: React.FC = () => {
  return (
    <div>
      <Content.Title
        extra={
          <Popover
            placement="bottomRight"
            content={
              <Space direction="vertical">
                <Space align="baseline">
                  <IconFont type="icon-uninstall" />
                  <TextBox>卸载应用</TextBox>
                </Space>
              </Space>
            }
            trigger="click"
          >
            <Content.Menu />
          </Popover>
        }
      >
        面板
      </Content.Title>
    </div>
  );
};

export default Panel;
