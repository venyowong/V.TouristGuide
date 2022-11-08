<template>
  <div>
    <van-row class="user-head-row">
      <van-col span="8">
        <img class="user-avatar" :src="user.avatar"/>
      </van-col>
      <van-col span="15" offset="1">
        <van-row>
          <van-col class="user-name" span="18">{{user.name}}</van-col>
          <van-col span="6">
            <van-icon name="setting-o" size="26" @click="toSetting"/>
          </van-col>
        </van-row>
        <van-row class="user-bio">
          {{user.bio}}
        </van-row>
      </van-col>
    </van-row>

    <van-tabs>
      <van-tab title="收藏">
        <van-empty description="没有任何收藏~" v-if="!collections || collections.length == 0"/>
        <van-steps class="user-collection" v-else direction="vertical" active-icon="star" inactive-icon="star" active-color="#969799" inactive-color="#969799">
          <van-step v-for="c in collections" :key="c.id">
            <p>{{`${c.name} ${c.updateTime}`}}</p>
            <img class="user-collection-cover" :src="c.cover" @click="toPlace(c.placeId)"/>
          </van-step>
          <van-step>
            没啦~
          </van-step>
        </van-steps>
      </van-tab>
      <van-tab title="评论">
        <van-empty description="还没有评价过~" v-if="!comments || comments.length == 0"/>
        <div class="user-comment" v-else>
          <van-row v-for="c in comments" :key="c.id">
            <van-col span="12">
              <img class="user-comment-cover" :src="c.cover"/>
            </van-col>
            <van-col span="12">
              <van-row>
                <van-col span="20">
                  <div class="user-comment-name">{{c.name}}</div>
                </van-col>
                <van-col>
                  <van-icon class="user-comment-delete" name="delete-o" color="#ee0a24" @click="deleteComment(c.id)"/>
                </van-col>
              </van-row>
              <div class="user-comment-content">{{c.remark}}</div>
              <div class="user-comment-time">{{c.updateTime}}</div>
            </van-col>
          </van-row>
        </div>
      </van-tab>
    </van-tabs>
  </div>
</template>

<script>
  import common from '@/common';
import { Dialog, Toast } from 'vant';

  export default {
    data() {
      return {
        token: null,
        user: {},
        collections: null,
        comments: null
      };
    },
    created() {
      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=user&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=user');
        }
      }

      this.token = this.$route.query.token;
      if (!this.token) {
        this.token = localStorage.getItem("token.user");
      }
      else {
        localStorage.setItem("token.user", this.token);
      }
      
      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }

      this.refresh();
    },
    methods: {
      toSetting() {
        this.$router.push({
          path: "/setting"
        })
      },
      refresh() {
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
            _this.user.avatar = "./img/logo.png";
          }
          if (!_this.user.name) {
            _this.user.name = "未设置昵称";
          }
          if (!_this.user.bio) {
            _this.user.bio = "这个人很懒，没有留下任何介绍~";
          }

          if (data.canSetPwd) {
            _this.$router.push({
              path: "/setPassword",
              query: {
                mode: 0
              }
            });
          }
        })
        .catch(e => {
          console.log(e);
          if (!_this.user.avatar) {
            _this.user.avatar = "./img/logo.png";
          }
          if (!_this.user.name) {
            _this.user.name = "未设置昵称";
          }
          if (!_this.user.bio) {
            _this.user.bio = "这个人很懒，没有留下任何介绍~";
          }
        });

        fetch(`${common.baseUrl}/user/operations`, {
          method: "GET",
          headers: {
            token: this.token
          }
        })
        .then(response => response.json())
        .then(data => {
          _this.collections = data.collections;
          _this.comments = data.comments;
        });
      },
      toPlace(placeId) {
        this.$router.push({
          path: "/placeDetail",
          query: {
            id: placeId
          }
        });
      },
      deleteComment(id) {
        let _this = this;
        Dialog.confirm({
          title: `确认删除`,
          message: `是否确认删除该评论？`
        })
        .then(() => {
          fetch(`${common.baseUrl}/place/comment/delete?id=${id}`, {
            method: "POST",
            headers: {
              token: this.token
            }
          })
          .then(response => response.json())
          .then(data => {
            if (data.code == 0) {
              _this.refresh();
            }
            else {
              if (data.msg) {
                Toast(data.msg);
              }
              else {
                Toast("删除评论失败");
              }
            }
          })
          .catch(e => {
            console.log(e);
            Toast("删除评论失败");
          });
        });
      }
    },
    activated() {
      this.refresh();
    }
  }
</script>

<style>
  .user-head-row {
    padding-top: 80px;
  }
  .user-avatar {
    width: 110px;
    clear: both;
    display: block;
    margin: auto;
    border-radius: 50%;
  }
  .user-name {
    font-size: 22px;
    font-weight: bold;
  }
  .user-bio {
    padding-top: 20px;
  }
  .user-collection {
    padding-bottom: 50px;
  }
  .user-collection-cover {
    width: calc(100% - 12px);
  }
  .user-comment {
    padding-bottom: 50px;
  }
  .user-comment-cover {
    width: calc(100% - 20px);
    padding: 10px;
  }
  .user-comment-name {
    font-size: 18px;
    font-weight: bold;
    padding-top: 10px;
    padding-bottom: 10px;
  }
  .user-comment-delete {
    padding-top: 14px;
  }
  .user-comment-content {
    font-size: 14px;
  }
  .user-comment-time {
    font-size: 12px;
    color: #969799;
    float: right;
    padding: 10px;
  }
</style>