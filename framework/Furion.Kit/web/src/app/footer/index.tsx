import { Image, Space } from "antd";
import { Container, FooterText } from "./style";

export default function FooterPanel() {
  return (
    <Container>
      <FooterText>
        版权所有 © 2020-2023 百小僧，百签科技（广东）有限公司
      </FooterText>
      <div>
        <Space size={10}>
          <FooterText>开源地址：</FooterText>
          <a
            href="https://gitee.com/dotnetchina/Furion"
            target="_blank"
            rel="noreferrer"
          >
            <Image
              src="https://gitee.com/dotnetchina/Furion/badge/star.svg?theme=gvp"
              preview={false}
              height={20}
            />
          </a>

          <a
            href="https://gitee.com/dotnetchina/Furion"
            target="_blank"
            rel="noreferrer"
          >
            <Image
              src="https://gitee.com/dotnetchina/Furion/badge/fork.svg?theme=gvp"
              preview={false}
              height={20}
            />
          </a>
        </Space>
      </div>
    </Container>
  );
}
