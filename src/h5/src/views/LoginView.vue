<template>
  <div>
    <van-nav-bar v-model:title="title"/>

    <van-form v-if="mode == 0">
      <van-cell-group>
        <van-field v-model="mobile" name="mobile" label="手机号" placeholder="请输入大陆手机号"
          :rules="[{ required: true, message: '请输入手机号' }]" />
        <van-field v-model="password" type="password" name="password" label="密码" placeholder="请输入密码"
          :rules="[{ required: true, message: '请输入密码' }]" />
      </van-cell-group>
      <div style="margin: 16px;">
        <van-button round block type="primary" @click="loginWithPwd" v-model:disabled="fetching">
          <van-loading size="14" color="white" v-if="fetching">登录</van-loading>
          <div v-if="!fetching">登录</div>
        </van-button>
      </div>
      <van-row justify="center" gutter="20">
        <van-col class="login-left-switch" span="8" @click="switchMode(1)">验证码登录</van-col>
        <van-col class="login-right-switch" span="8" @click="switchMode(2)">注册</van-col>
      </van-row>
    </van-form>

    <van-form v-if="mode == 1">
      <van-cell-group>
        <van-field v-model="mobile" name="mobile" label="手机号" placeholder="请输入大陆手机号"
          :rules="[{ required: true, message: '请输入手机号' }]" />
        <van-field v-model="code" center clearable label="验证码" placeholder="请输入短信验证码" >
          <template #button>
            <van-button size="small" type="primary" @click="refreshCodeButton(-1)" v-model:disabled="codeButtonDisabled">
              <van-loading size="14" color="white" v-if="sendingSms">{{codeButtonText}}</van-loading>
              <div v-if="!sendingSms">{{codeButtonText}}</div>
            </van-button>
          </template>
        </van-field>
      </van-cell-group>
      <div style="margin: 16px;">
        <van-button round block type="primary" @click="verifyCode" v-model:disabled="fetching">
          <van-loading size="14" color="white" v-if="fetching">登录</van-loading>
          <div v-if="!fetching">登录</div>
        </van-button>
      </div>
      <van-row justify="center" gutter="20">
        <van-col class="login-left-switch" span="8" @click="switchMode(0)">密码登录</van-col>
        <van-col class="login-right-switch" span="8" @click="switchMode(2)">注册</van-col>
      </van-row>
    </van-form>

    <van-form v-if="mode == 2">
      <van-cell-group>
        <van-field v-model="mobile" name="mobile" label="手机号" placeholder="请输入大陆手机号"
          :rules="[{ required: true, message: '请输入手机号' }]" />
        <van-field v-model="code" center clearable label="验证码" placeholder="请输入短信验证码" >
          <template #button>
            <van-button size="small" type="primary" @click="refreshCodeButton(-1)" v-model:disabled="codeButtonDisabled">
              <van-loading size="14" color="white" v-if="sendingSms">{{codeButtonText}}</van-loading>
              <div v-if="!sendingSms">{{codeButtonText}}</div>
            </van-button>
          </template>
        </van-field>
      </van-cell-group>
      <div style="margin: 16px;">
        <van-button round block type="primary" @click="verifyCode" v-model:disabled="fetching">
          <van-loading size="14" color="white" v-if="fetching">注册</van-loading>
          <div v-if="!fetching">注册</div>
        </van-button>
      </div>
      <van-row justify="center" gutter="20">
        <van-col class="login-left-switch" span="8" @click="switchMode(0)">密码登录</van-col>
        <van-col class="login-right-switch" span="8" @click="switchMode(1)">验证码登录</van-col>
      </van-row>
    </van-form>

    <van-row>
      <van-col span="16" offset="4">
        <van-divider>第三方登录</van-divider>
      </van-col>
    </van-row>
    <div class="login-oauths">
      <img class="login-oauth-logo" src="../assets/baidu.png" @click="oauth('baidu')"/>
      <img class="login-oauth-logo" src="../assets/github.png" @click="oauth('github')"/>
      <img class="login-oauth-logo" src="../assets/gitee.jpg" @click="oauth('gitee')"/>
    </div>
  </div>
</template>

<script>
  import common from '@/common';
  import { Toast } from 'vant';

  export default {
    data() {
      return {
        title: "登录",
        mode: 0, // 0 登录 1 验证码登录 2 注册
        mobile: null,
        password: null,
        code: null,
        codeButtonText: "发送验证码",
        contextId: null,
        codeButtonDisabled: false,
        sendingSms: false,
        fetching: false
      };
    },
    methods: {
      switchMode(mode) {
        this.mode = mode;
        if (mode == 2) {
          this.title = "注册";
        }
        else {
          this.title = "登录";
        }
      },
      refreshCodeButton(seconds) {
        let _this = this;
        if (seconds < 0) {
          if (!this.mobile || this.mobile.length < 11) {
            Toast("请输入正确手机号");
            return;
          }

          this.sendingSms = true;
          this.codeButtonDisabled = true;
          let url = `${common.baseUrl}/usermodule/login`;
          if (this.mode == 2) {
            url = `${common.baseUrl}/usermodule/signup`;
          }
          fetch(url, {
            method: "POST",
            body: JSON.stringify({
              mobile: this.mobile
            })
          })
          .then(response => response.json())
          .then(data => {
            _this.sendingSms = false;
            if (data.code != 0) {
              Toast(data.msg);
              _this.codeButtonDisabled = false;
              return;
            }

            _this.contextId = data.data;
            _this.codeButtonText = "60s";
            setTimeout(() => _this.refreshCodeButton(59), 1000);
          })
          .catch(e => {
            console.log(e);
            _this.sendingSms = false;
            _this.codeButtonDisabled = false;
          });
        } 
        else if (seconds == 0) {
          _this.codeButtonDisabled = false;
          _this.codeButtonText = "发送验证码";
        }
        else {
          _this.codeButtonText = `${seconds}s`;
          setTimeout(() => _this.refreshCodeButton(seconds - 1), 1000);
        }
      },
      loginWithPwd() {
        if (!this.mobile || this.mobile.length < 11) {
          Toast("请输入正确手机号");
          return;
        }
        if (!this.password) {
          Toast("请输入密码");
          return;
        }

        let _this = this;
        this.fetching = true;
        let md5 = require('md5');
        fetch(`${common.baseUrl}/usermodule/login`, {
          method: "POST",
          body: JSON.stringify({
            mobile: this.mobile,
            password: md5(this.password)
          })
        })
        .then(response => response.json())
        .then(data => {
          _this.fetching = false;
          if (data.code != 0) {
            Toast(data.msg);
            return;
          }

          localStorage.setItem("user", JSON.stringify(data.data));
          localStorage.setItem("token.user", data.data.token);
          this.$router.push({
            path: "/user"
          });
        })
        .catch(e => {
          console.log(e);
          _this.fetching = false;
        });
      },
      verifyCode() {
        if (!this.mobile || this.mobile.length < 11) {
          Toast("请输入正确手机号");
          return;
        }
        if (!this.code || this.code.length != 6) {
          Toast("请输入 6 位验证码");
          return;
        }

        let _this = this;
        this.fetching = true;
        fetch(`${common.baseUrl}/usermodule/verifycode`, {
          method: "POST",
          body: JSON.stringify({
            contextId: this.contextId,
            code: this.code
          })
        })
        .then(response => response.json())
        .then(data => {
          _this.fetching = false;
          if (data.code != 0) {
            Toast(data.msg);
            return;
          }

          localStorage.setItem("user", JSON.stringify(data.data));
          localStorage.setItem("token.user", data.data.token);
          this.$router.push({
            path: "/user"
          });
        })
        .catch(e => {
          console.log(e);
          _this.fetching = false;
        });
      },
      oauth(source) {
        location.href = `${common.baseUrl}/usermodule/oauth?service=${source}`;
      }
    },
    created() {
      if (common.addPageView) {
        fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=login');
      }
    }
  }
</script>

<style>
  .login-left-switch {
    text-align: end;
    color: #1989fa;
  }
  .login-right-switch {
    text-align: start;
    color: #1989fa;
  }
  .login-oauth-logo {
    height: 20px;
    border-radius: 50%;
    padding: 4px;
  }
  .login-oauths {
    text-align: center;
  }
</style>