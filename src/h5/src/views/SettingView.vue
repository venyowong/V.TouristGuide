<template>
  <div>
    <van-nav-bar title="设置" left-text="返回" left-arrow @click-left="onClickLeft"></van-nav-bar>
    <EditableCell ref="avatarCell" title="头像" type="img" :onSet="updateAvatar"></EditableCell>
    <EditableCell ref="nameCell" title="昵称" type="text" :onSet="updateName"></EditableCell>
    <EditableCell ref="bioCell" title="简介" type="textarea" :onSet="updateBio"></EditableCell>
    <van-cell title="手机号" :value="user.maskMobile" is-link @click="changeMobile"/>
    <van-cell title="修改密码" is-link @click="resetPwd"/>
    <van-cell title="意见反馈" is-link url="mailto:venyowong@163.com" />
    <van-cell title="开发者" value="木叉一人工作室"/>
    <div style="margin: 16px;">
      <van-button round block type="danger" @click="logout">
        退出登录
      </van-button>
    </div>
  </div>
</template>

<script>
  import EditableCell from '@/components/EditableCell.vue';
  import common from '@/common';

  export default {
    data() {
      return {
        token: null,
        user: {}
      };
    },
    methods: {
      onClickLeft() {
          history.back();
      },
      logout() {
        console.log(localStorage.getItem("token.user"));
        localStorage.removeItem("token.user");
        console.log(localStorage.getItem("token.user"));
        localStorage.removeItem("user");
        this.$router.push({
          path: "/login"
        })
      },
      updateAvatar(_title, _oldValue, newValue) {
        fetch(`${common.baseUrl}/usermodule/updateinfo`, {
          method: "POST",
          body: JSON.stringify({
            avatar: newValue,
            token: this.token
          })
        });
      },
      updateName(_title, _oldValue, newValue) {
        fetch(`${common.baseUrl}/usermodule/updateinfo`, {
          method: "POST",
          body: JSON.stringify({
            name: newValue,
            token: this.token
          })
        });
      },
      updateBio(_title, _oldValue, newValue) {
        fetch(`${common.baseUrl}/usermodule/updateinfo`, {
          method: "POST",
          body: JSON.stringify({
            bio: newValue,
            token: this.token
          })
        });
      },
      resetPwd() {
        this.$router.push({
          path: "setPassword",
          query: {
            mode: 1
          }
        });
      },
      changeMobile() {
        this.$router.push({
          path: "changeMobile"
        });
      }
    },
    components: { EditableCell },
    created() {
      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=setting&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=setting');
        }
      }

      this.token = localStorage.getItem("token.user");
      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }

      let _this = this;
      fetch(`${common.baseUrl}/usermodule/info`, {
        method: "GET",
        headers: {
          token: this.token
        }
      })
      .then(response => response.json())
      .then(data => {
        _this.user = data;
        
        if (!_this.user.avatar) {
          _this.user.avatar = "../assets/logo.png";
        }
        if (!_this.user.name) {
          _this.user.name = "未设置昵称";
        }
        if (!_this.user.bio) {
          _this.user.bio = "这个人很懒，没有留下任何介绍~";
        }

        _this.$refs.nameCell.changeValue(_this.user.name);
        _this.$refs.avatarCell.changeValue(_this.user.avatar);
        _this.$refs.bioCell.changeValue(_this.user.bio);
      })
      .catch(e => {
        console.log(e);
        if (!_this.user.avatar) {
          _this.user.avatar = "../assets/logo.png";
        }
        if (!_this.user.name) {
          _this.user.name = "未设置昵称";
        }
        if (!_this.user.bio) {
          _this.user.bio = "这个人很懒，没有留下任何介绍~";
        }

        _this.$refs.nameCell.changeValue(_this.user.name);
        _this.$refs.avatarCell.changeValue(_this.user.avatar);
        _this.$refs.bioCell.changeValue(_this.user.bio);
      });
    }
}
</script>