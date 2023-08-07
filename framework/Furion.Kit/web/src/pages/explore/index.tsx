import {
  Anchor,
  Button,
  Divider,
  Input,
  Layout,
  Popover,
  Rate,
  Space,
  Typography,
} from "antd";
import { AnchorContainer } from "antd/es/anchor/Anchor";
import logo from "../../assets/logo.png";
import IconFont from "../../components/iconfont";
import {
  CardDescription,
  CardIcon,
  CardItem,
  CardMain,
  CardPanel,
  CardTitle,
  CardViewIcon,
  Container,
  Panel,
} from "./style";

const { Title, Text } = Typography;
const { Sider, Content } = Layout;

export default function Explore() {
  return (
    <Panel>
      <Title level={3}>探索</Title>
      <Container id="explore-container">
        <Sider
          theme="light"
          style={{ position: "sticky", overflowY: "auto", top: 0 }}
        >
          <div style={{ marginTop: 20 }}>
            <Anchor
              onClick={(ev) => ev.preventDefault()}
              getContainer={() =>
                document.getElementById("explore-container") as AnchorContainer
              }
              affix={false}
              items={[
                {
                  key: "application",
                  href: "#explore-application",
                  title: (
                    <Space>
                      <IconFont type="icon-tag" />
                      <Text>应用案例</Text>
                    </Space>
                  ),
                },
                {
                  key: "tools",
                  href: "#explore-tools",
                  title: (
                    <Space>
                      <IconFont type="icon-tag" />
                      <Text>开发工具</Text>
                    </Space>
                  ),
                },
              ]}
            />
          </div>
        </Sider>
        <Content style={{ marginLeft: 15 }}>
          <div
            style={{
              padding: "0 0 25px 0",
            }}
          >
            <Input
              style={{ width: 180 }}
              placeholder="ERP WMS OA"
              suffix={<IconFont type="icon-search" />}
            />
          </div>
          <Space>
            <IconFont type="icon-tag" style={{ fontSize: 18 }} />
            <Title level={5} style={{ marginTop: 0 }} id="explore-application">
              应用案例
            </Title>
          </Space>
          <CardPanel>
            <Popover
              content={
                <Space>
                  <Button type="primary">安装</Button>
                  <Button disabled>卸载</Button>
                </Space>
              }
              title={
                <Space direction="vertical">
                  <Space size={5}>
                    <Text>版本：</Text>
                    <Text style={{ color: "#595959" }}>5.0.0</Text>
                  </Space>
                  <Space size={5}>
                    <Text>推荐指数</Text>
                    <IconFont type="icon-info" style={{ color: "#8c8c8c" }} />
                  </Space>
                  <Rate allowHalf defaultValue={5} disabled />
                </Space>
              }
              placement="bottom"
            >
              <CardItem>
                <CardIcon src={logo} />
                <CardMain>
                  <CardTitle>Furioin</CardTitle>
                  <CardDescription>
                    一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。
                  </CardDescription>
                </CardMain>
                <CardViewIcon>
                  <IconFont
                    type="icon-view-arrow-right"
                    style={{ color: "#8c8c8c" }}
                  />
                </CardViewIcon>
              </CardItem>
            </Popover>

            <Popover
              content={
                <Space>
                  <Button type="primary">安装</Button>
                  <Button disabled>卸载</Button>
                </Space>
              }
              title={
                <Space direction="vertical">
                  <Space size={5}>
                    <Text>版本：</Text>
                    <Text style={{ color: "#595959" }}>5.0.0</Text>
                  </Space>
                  <Space size={5}>
                    <Text>推荐指数</Text>
                    <IconFont type="icon-info" style={{ color: "#8c8c8c" }} />
                  </Space>
                  <Rate allowHalf defaultValue={4.5} disabled />
                </Space>
              }
              placement="bottom"
            >
              <CardItem>
                <CardIcon src={logo} />
                <CardMain>
                  <CardTitle>Furion Kit</CardTitle>
                  <CardDescription>Furion 框架开发工具箱。</CardDescription>
                </CardMain>
                <CardViewIcon>
                  <IconFont
                    type="icon-view-arrow-right"
                    style={{ color: "#8c8c8c" }}
                  />
                </CardViewIcon>
              </CardItem>
            </Popover>
          </CardPanel>
          <Divider />

          <Space>
            <IconFont type="icon-tag" style={{ fontSize: 18 }} />
            <Title level={5} style={{ marginTop: 0 }} id="explore-tools">
              开发工具
            </Title>
          </Space>
          <CardPanel>
            <Popover
              content={
                <Space>
                  <Button type="primary">安装</Button>
                  <Button disabled>卸载</Button>
                </Space>
              }
              title={
                <Space direction="vertical">
                  <Space size={5}>
                    <Text>版本：</Text>
                    <Text style={{ color: "#595959" }}>5.0.0</Text>
                  </Space>
                  <Space size={5}>
                    <Text>推荐指数</Text>
                    <IconFont type="icon-info" style={{ color: "#8c8c8c" }} />
                  </Space>
                  <Rate allowHalf defaultValue={3} disabled />
                </Space>
              }
              placement="bottom"
            >
              <CardItem>
                <CardIcon src={logo} />
                <CardMain>
                  <CardTitle>Furioin</CardTitle>
                  <CardDescription>
                    一个应用程序框架，您可以将它集成到任何 .NET/C# 应用程序中。
                  </CardDescription>
                </CardMain>
                <CardViewIcon>
                  <IconFont
                    type="icon-view-arrow-right"
                    style={{ color: "#8c8c8c" }}
                  />
                </CardViewIcon>
              </CardItem>
            </Popover>
          </CardPanel>
          <Divider />
        </Content>
      </Container>
    </Panel>
  );
}
