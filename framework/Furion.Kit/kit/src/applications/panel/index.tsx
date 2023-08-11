import IconFont from "../../components/iconfont";
import Content from "../../home/content";

const Panel: React.FC = () => {
  return (
    <div>
      <Content.Title
        extra={
          <Content.Menu
            menu={{
              items: [
                {
                  key: 1,
                  label: "卸载",
                  icon: <IconFont type="icon-uninstall" $size={16} />,
                },
                {
                  key: 2,
                  label: "配置",
                  icon: <IconFont type="icon-configuration" $size={16} />,
                },
              ],
            }}
          />
        }
      >
        面板
      </Content.Title>
    </div>
  );
};

export default Panel;
