<template>
  <div>
    <van-nav-bar title="设置手机号" left-text="返回" left-arrow @click-left="onClickLeft"></van-nav-bar>
    <van-field v-model="mobile" name="mobile" label="手机号" placeholder="请输入大陆手机号"
      :rules="[{ required: true, message: '请输入手机号' }]" />
    <VerificationCode ref="code" :sendCode="sendCode"></VerificationCode>
    <SubmitButton ref="button" text="提交" :onclick="submit"></SubmitButton>
  </div>
</template>

<script>
  import VerificationCode from '@/components/VerificationCode.vue';
  import { Toast } from 'vant';
  import SubmitButton from '@/components/SubmitButton.vue';
  import common from '@/common';

  export default {
    data() {
      return {
        mobile: null,
        token: null,
        contextId: null
      };
    },
    methods: {
      onClickLeft() {
        history.back();
      },
      sendCode() {
        if (!this.mobile) {
          Toast("请输入手机号");
          return;
        }

        let _this = this;
        this.$refs.code.setSending(true);
        this.$refs.code.setDisabled(true);
        fetch(`${common.baseUrl}/usermodule/updateinfo`, {
          method: "POST",
          body: JSON.stringify({
            mobile: this.mobile,
            token: this.token
          })
        })
        .then(response => response.json())
        .then(data => {
          _this.$refs.code.setSending(false);
          if (data.code == 0) {
            _this.contextId = data.data;
            _this.$refs.code.refreshCodeButton(60);
          }
          else {
            _this.$refs.code.setDisabled(false);
            if (data.msg) {
              Toast(data.msg);
            }
            else {
              Toast("发送短信验证码失败");
            }
          }
        })
        .catch(e => {
          console.log(e);
          _this.$refs.code.setSending(false);
          _this.$refs.code.setDisabled(false);
          Toast("发送短信验证码失败");
        });
      },
      submit() {
        if (!this.mobile) {
          Toast("请输入手机号");
          return;
        }
        if (!this.$refs.code.code) {
          Toast("请输入验证码");
          return;
        }

        let _this = this;
        this.$refs.button.setFetching(true);
        fetch(`${common.baseUrl}/usermodule/verifycode`, {
          method: "POST",
          body: JSON.stringify({
            contextId: this.contextId,
            code: this.$refs.code.code
          })
        })
        .then(response => response.json())
        .then(data => {
          _this.$refs.button.setFetching(false);
          if (data.code != 0) {
            Toast(data.msg);
            return;
          }

          history.back();
        })
        .catch(e => {
          console.log(e);
          _this.$refs.button.setFetching(false);
        });
      }
    },
    created() {
      this.mode = this.$route.query.mode;
      this.token = localStorage.getItem("token.user");
      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }
    },
    components: { VerificationCode, SubmitButton }
}
</script>