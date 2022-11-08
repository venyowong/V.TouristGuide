<template>
  <div>
    <van-nav-bar title="评价" left-text="返回" left-arrow @click-left="onClickLeft"/>
    <van-cell-group :title="name">
      <van-field v-model="comment" rows="4" autosize type="textarea" maxlength="100" placeholder="请输入评价~" show-word-limit />
    </van-cell-group>
    <div style="margin: 16px;">
      <van-button round block type="primary" @click="submitComment" v-model:disabled="fetching">
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
        id: this.$route.query.id,
        name: this.$route.query.name,
        comment: null,
        fetching: false,
        token: null
      };
    },
    methods: {
      onClickLeft() {
        history.back();
      },
      submitComment() {
        if (!this.comment) {
          Toast("请输入评价");
          return;
        }

        let _this = this;
        this.fetching = true;
        fetch(`${common.baseUrl}/place/comment`, {
          method: "POST",
          body: JSON.stringify({
            placeId: Number(this.id),
            comment: this.comment,
            token: this.token
          }),
          headers: { 
            "Content-Type": "application/json"
          }
        })
        .then(response => response.json())
        .then(data => {
          _this.fetching = false;
          if (data.code != 0) {
            Toast("评论失败");
          }
          else {
            history.back();
          }
        })
        .catch(e => {
          console.log(e);
          _this.fetching = false;
          Toast("评论失败");
        });
      }
    },
    created() {
      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=comment&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=comment');
        }
      }

      this.token = localStorage.getItem("token.user");

      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }
    }
  }
</script>