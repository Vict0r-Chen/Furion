import { Layout } from "antd";
import { css, styled } from "styled-components";

const Panel = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const Container = styled(Layout)`
  overflow: hidden;
  flex: 1;
  background-color: #ffffff;

  &:hover {
    overflow: auto;
  }
`;

const CardPanel = styled.div`
  margin-top: 15px;
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: flex-start;
`;

const activeCss = css`
  background-color: #fff;
  border: 1px solid #69b1ff;
  box-shadow: 0px 0px 10px #69b1ff;
`;

const CardItem = styled.div`
  box-sizing: border-box;
  width: 218px;
  height: 70px;
  margin-right: 20px;
  margin-bottom: 20px;
  background-color: #0092ff;
  display: flex;
  flex-direction: row;
  align-items: center;
  padding: 10px;

  overflow: hidden;
  border-radius: 8px;
  cursor: pointer;
  transition: transform 0.4s;
  background-color: #e6f4ff;

  &:hover {
    transform: translateY(-6px);

    ${activeCss}
  }
`;

const CardIcon = styled.img`
  height: 40px;
  width: 40px;
  min-width: 40px;
  margin-right: 10px;
  display: block;
`;

const CardMain = styled.div`
  overflow: hidden;
  flex: 1;
`;

const CardTitle = styled.div`
  font-size: 14px;
  font-weight: bold;
  color: #000000e0;
`;

const CardDescription = styled.div`
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  font-size: 12px;
  color: #777;
`;

const CardViewIcon = styled.div`
  margin-left: 5px;
`;

export {
  CardDescription,
  CardIcon,
  CardItem,
  CardMain,
  CardPanel,
  CardTitle,
  CardViewIcon,
  Container,
  Panel
};

