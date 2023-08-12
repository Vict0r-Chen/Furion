import { Trans } from "@lingui/macro";
import IconFont from "../../components/iconfont";
import Content from "../../home/content";
import Mdx from "../../mdxs/index.mdx";

const Panel: React.FC = () => {
  return (
    <Content.Main>
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
        <Trans>面板</Trans>
      </Content.Title>

      <Mdx />
    </Content.Main>
  );
};

export default Panel;
