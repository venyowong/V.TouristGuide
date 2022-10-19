<template>
  <div class="place-detail-page">
    <van-nav-bar :title="data.place.name" left-text="返回" left-arrow @click-left="onClickLeft">
      <template #right>
        <van-icon name="comment-o" style="padding-right: 10px" size="22" @click="toComment()"/>
        <van-icon v-if="data != null && data.hasStar" name="star" size="22" @click="collect">
        </van-icon>
        <van-icon v-else name="star-o" size="20" @click="collect">
        </van-icon>
        {{data.stars}}
      </template>
    </van-nav-bar>
    <img class="place-detail-img" :src="data.place.cover"/>
    <div class="place-detail-name">{{data.place.name}}</div>
    <van-cell-group>
      <van-cell v-if="data.place.openTime != null && data.place.openTime.length > 0" title="开放时间" :value="data.place.openTime" icon="shop-o" size="large"/>
      <van-cell v-if="data.place.tel != null && data.place.tel.length > 0" title="联系方式" :value="data.place.tel" icon="phone-o" size="large"/>
    </van-cell-group>
    <van-cell-group v-if="data.place.address != null && data.place.address.length > 0" title="地址">
      <van-cell :value="data.place.address" icon="location-o" size="large" is-link @click="mapSelection = true"/>
    </van-cell-group>
    <van-cell-group class="place-detail-inline-row" v-if="data.place.type == 0 && data.pictures != null && data.pictures.length > 0" title="菜单">
      <div class="place-detail-inline-col" v-for="pic in data.pictures" :key="pic.id">
        <img class="place-detail-menu-img" :src="pic.img"/>
        <div class="place-detail-menu-title">{{pic.title}}</div>
      </div>
    </van-cell-group>
    <van-cell-group class="place-detail-inline-row" v-if="data.articles != null && data.articles.length > 0" title="他们都在说~">
      <div class="place-detail-inline-col" v-for="article in data.articles" :key="article.id" @click="to(article.url)">
        <img class="place-detail-article-img" :src="article.img"/>
        <div class="place-detail-article-title">{{article.title}}</div>
        <div class="place-detail-article-source">
          <SourceLogo :source="article.source"></SourceLogo>
        </div>
      </div>
    </van-cell-group>
    <van-cell-group class="place-detail-inline-row" v-if="data.videos != null && data.videos.length > 0" title="来康康~">
      <div class="place-detail-inline-col" v-for="video in data.videos" :key="video.id" @click="to(video.url)">
        <VideoCover :url="video.img" height="150px" width="150px"></VideoCover>
        <div class="place-detail-article-title">{{video.title}}</div>
        <div class="place-detail-article-source">
          <SourceLogo :source="video.source"></SourceLogo>
        </div>
      </div>
    </van-cell-group>
    <van-cell-group title="评论">
      <van-cell v-for="comment in data.comments" :key="comment.id">
        <template #title>
          <div class="place-detail-inline-row">
            <img class="place-detail-inline-col place-detail-comment-avatar" :src="comment.img"/>
            <div class="place-detail-inline-col place-detail-comment-name">{{comment.title}}</div>
          </div>
        </template>
        <template #label>
          <div>{{comment.desc}}</div>
          <div class="place-detail-article-source">
            <SourceLogo :source="comment.source"></SourceLogo>
          </div>
        </template>
      </van-cell>

      <van-cell v-for="comment in data.commentOperations" :key="comment.id">
        <template #title>
          <div class="place-detail-inline-row">
            <img class="place-detail-inline-col place-detail-comment-avatar" :src="comment.avatar"/>
            <div class="place-detail-inline-col place-detail-comment-name">{{comment.name}}</div>
          </div>
        </template>
        <template #label>
          <div>{{comment.content}}</div>
        </template>
      </van-cell>
    </van-cell-group>

    <van-action-sheet v-model:show="mapSelection" :actions="mapActions" @select="onMapSelected" cancel-text="取消" close-on-click-action/>
  </div>
</template>

<script>
  import common from '@/common';
  import SourceLogo from '@/components/SourceLogo.vue';
  import VideoCover from '@/components/VideoCover.vue';
  import { Toast } from 'vant';

  export default {
    data() {
      return {
        id: this.$route.query.id,
        data: {
          place: {
            name: null,
          },
          stars: 0
        },
        mapSelection: false,
        mapActions: [
          {name: "百度地图"},
          {name: "高德地图"}
        ],
        lng: null,
        lat: null,
        address: null,
        token: null
      };
    },
    created() {
      let json = localStorage.getItem('location');
      let l = JSON.parse(json);
      this.lng = l.lng;
      this.lat = l.lat;
      this.address = l.address;
      this.token = localStorage.getItem("token.user");
      this.refresh();
    },
    methods: {
      onClickLeft() {
        history.back();
      },
      to(url) {
        window.open(url);
      },
      onMapSelected(item) {
        this.mapSelection = false;

        let url = "";
        if (item.name == "百度地图") {
          url = `http://api.map.baidu.com/direction?origin=latlng:${this.lat},${this.lng}|name:${this.address}&destination=latlng:${this.data.place.lat},${this.data.place.lng}|name:${this.data.place.address}&mode=driving&output=html&coord_type=bd09ll&src=webapp.venyo.touristguide&region=${this.data.place.region}`;
        }
        else if (item.name == "高德地图") {
          let local = common.toGaodeLatlng(this.lat, this.lng);
          let destination = common.toGaodeLatlng(this.data.place.lat, this.data.place.lng);
          url = `https://uri.amap.com/navigation?from=${local.lng},${local.lat},${this.address}&to=${destination.lng},${destination.lat},${this.data.place.address}&src=webapp.venyo.touristguide&callnative=1`;
        }
        window.open(url);
      },
      toComment() {
        this.$router.push({
          path: "/comment",
          query: {
            id: this.id,
            name: this.data.place.name
          }
        });
      },
      collect() {
        if (!this.token) {
          this.$router.push({
            path: "/login"
          });
          return;
        }

        let _this = this;
        fetch(`${common.baseUrl}/place/collect?placeId=${this.id}`, {
          method: "POST",
          headers: {
            token: this.token
          }
        })
        .then(response => response.json())
        .then(data => {
          _this.fetching = false;
          if (data.code != 0) {
            Toast("收藏失败");
          }
          else {
            _this.refresh();
          }
        })
        .catch(e => {
          console.log(e);
          _this.fetching = false;
          Toast("收藏失败");
        });
      },
      refresh() {
        let _this = this;
        let userId = 0;
        let user = localStorage.getItem("user");
        if (user) {
          user = JSON.parse(user);
          userId = user.id;
        }
        fetch(`${common.baseUrl}/place/detail?id=${this.id}&userId=${userId}`)
        .then(response => response.json())
        .then(data => {
          _this.data = data;
        });
      }
    },
    components: { 
      SourceLogo, 
      VideoCover 
    },
    activated() {
      this.refresh();
    }
}
</script>

<style>
  .place-detail-page {
    padding-bottom: 50px;
  }
  .place-detail-img {
    border-radius: 15px;
    width: calc(100% - 32px);
    margin: 16px;
  }
  .place-detail-name {
    padding-left: 16px;
    font-weight: bold;
    font-size: 20px;
    padding-bottom: 8px;
  }
  .place-detail-inline-row {
    overflow: auto;
    padding: 16px;
    white-space: nowrap;
    position: relative;
  }
  .place-detail-inline-col {
    display: inline-block;
    padding-right: 5px;
  }
  .place-detail-menu-img {
    height: 100px;
  }
  .place-detail-menu-title {
    position: absolute;
    bottom: 24px;
    padding-left: 2px;
    color: white;
    font-size: 12px;
  }
  .place-detail-article-img {
    width: 150px;
  }
  .place-detail-article-title {
    white-space: normal;
    width: 150px;
    font-size: 12px;
  }
  .place-detail-article-source {
    float: right;
  }
  .place-detail-comment-avatar {
    border-radius: 50%;
    height: 20px;
    position: absolute;
    left: 0;
    top: 6px;
  }
  .place-detail-comment-name {
    font-size: 14px;
    position: absolute;
    left: 25px;
    top: 4px;
  }
</style>