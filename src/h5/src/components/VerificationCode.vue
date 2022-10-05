<template>
  <div>
    <van-field v-model="code" center clearable label="验证码" placeholder="请输入短信验证码" >
      <template #button>
        <van-button size="small" type="primary" @click="refreshCodeButton(-1)" v-model:disabled="codeButtonDisabled">
          <van-loading size="14" color="white" v-if="sendingSms">{{codeButtonText}}</van-loading>
          <div v-if="!sendingSms">{{codeButtonText}}</div>
        </van-button>
      </template>
    </van-field>
  </div>
</template>

<script>
  export default {
    props: ["sendCode"],
    data() {
      return {
        code: null,
        codeButtonText: "发送验证码",
        codeButtonDisabled: false,
        sendingSms: false
      };
    },
    methods: {
      refreshCodeButton(seconds) {
        if (seconds < 0) {
          this.sendCode();
        } 
        else if (seconds == 0) {
          this.codeButtonDisabled = false;
          this.codeButtonText = "发送验证码";
        }
        else {
          this.codeButtonText = `${seconds}s`;
          let _this = this;
          setTimeout(() => _this.refreshCodeButton(seconds - 1), 1000);
        }
      },
      setSending(s) {
        this.sendingSms = s;
      },
      setDisabled(d) {
        this.codeButtonDisabled = d;
      }
    }
  }
</script>