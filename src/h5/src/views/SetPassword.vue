<template>
  <div>
    <van-nav-bar v-if="mode == 0" title="设置密码"></van-nav-bar>
    <van-nav-bar v-if="mode == 1" title="重置密码" left-text="返回" left-arrow @click-left="onClickLeft"></van-nav-bar>
    <van-field v-if="mode == 1" v-model="oldPwd" type="password" label="旧密码" />
    <van-field v-model="newPwd" type="password" label="新密码" />
    <van-field v-model="repeatPwd" type="password" label="重复新密码" />
    <div style="margin: 16px;">
      <van-button round block type="primary" @click="submit" v-model:disabled="fetching">
        <van-loading size="14" color="white" v-if="fetching">提交</van-loading>
        <div v-if="!fetching">提交</div>
      </van-button>
    </div>
  </div>
</template>

<script>
  import common from '@/common';
  import { Toast } from 'vant';

  export default {
    data() {
      return {
        mode: null, // 0 设置密码 1 重置密码
        fetching: false,
        oldPwd: null,
        newPwd: null,
        repeatPwd: null,
        token: null
      };
    },
    created() {
      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=setPassword&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=setPassword');
        }
      }
      
      this.mode = this.$route.query.mode;
      this.token = localStorage.getItem("token.user");
      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }
    },
    methods: {
      onClickLeft() {
        history.back();
      },
      submit() {
        if (!this.newPwd) {
          Toast("请输入新密码");
          return;
        }
        if (this.newPwd != this.repeatPwd) {
          Toast("两次新密码输入不一致");
          return;
        }
        if (this.mode == 1 && !this.oldPwd) {
          Toast("请输入旧密码");
          return;
        }

        let md5 = require("md5");
        let _this = this;
        this.fetching = true;
        if (this.mode == 0) {
          fetch(`${common.baseUrl}/usermodule/setpassword`, {
            method: "POST",
            body: JSON.stringify({
              password: md5(this.newPwd),
              token: this.token
            })
          })
          .then(response => response.json())
          .then(data => {
            _this.fetching = false;
            if (data.code == 0) {
              _this.$router.push({
                path: "/user"
              });
            }
            else {
              if (data.msg) {
                Toast(data.msg);
              }
              else {
                Toast("设置密码失败");
              }
            }
          })
          .catch(e => {
            console.log(e);
            _this.fetching = false;
            Toast("设置密码失败");
          });
        }
        else {
          fetch(`${common.baseUrl}/usermodule/resetpassword`, {
            method: "POST",
            body: JSON.stringify({
              oldPwd: md5(this.oldPwd),
              newPwd: md5(this.newPwd),
              token: this.token
            })
          })
          .then(response => response.json())
          .then(data => {
            _this.fetching = false;
            if (data.code == 0) {
              _this.$router.push({
                path: "/user"
              });
            }
            else {
              if (data.msg) {
                Toast(data.msg);
              }
              else {
                Toast("重置密码失败");
              }
            }
          })
          .catch(e => {
            console.log(e);
            _this.fetching = false;
            Toast("重置密码失败");
          });
        }
      }
    }
  }
</script>