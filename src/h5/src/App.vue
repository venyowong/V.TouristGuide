<template>
  <!-- <nav>
    <router-link to="/">Home</router-link> |
    <router-link to="/about">About</router-link>
  </nav> -->

  <!-- <van-button type="primary">主要按钮</van-button>
  <van-button type="success">成功按钮</van-button>
  <van-button type="default">默认按钮</van-button>
  <van-button type="warning">警告按钮</van-button>
  <van-button type="danger">危险按钮</van-button> -->

  <router-view/>

  <van-tabbar route>
    <van-tabbar-item replace to="/" icon="home-o">首页</van-tabbar-item>
    <van-tabbar-item replace to="/map" icon="location-o">地图</van-tabbar-item>
    <van-tabbar-item replace to="/user" icon="user-o">我的</van-tabbar-item>
  </van-tabbar>
</template>

<script>
  import vconsole from 'vconsole';
  import { createApp } from 'vue';
  // 1. 引入你需要的组件
  import { Button, Tabbar, TabbarItem, Image as VanImage, Toast, Cell, CellGroup, Col, Row, ActionSheet, Form, Field, Tab, Tabs, Dialog } from 'vant';
  // 2. 引入组件样式
  import 'vant/lib/index.css';

  const app = createApp();
  // const isProd = process.env.NODE_ENV === 'production'

  // 3. 注册你需要的组件
  app.use(Button);
  app.use(Tabbar);
  app.use(TabbarItem);
  app.use(VanImage);
  app.use(Toast);
  app.use(Cell);
  app.use(CellGroup);
  app.use(Col);
  app.use(Row);
  app.use(ActionSheet);
  app.use(Form);
  app.use(Field);
  app.use(Tab);
  app.use(Tabs);
  app.use(Dialog);
  const vConsole = new vconsole();
  app.use(vConsole);
  // if (!isProd) {
  // }

  export default {
    data() {
      return {
        active: "home"
      }
    }
  }
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
