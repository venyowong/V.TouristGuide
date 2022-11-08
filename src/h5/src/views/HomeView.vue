<template>
  <van-config-provider :theme-vars="custom">
    <keep-alive>
      <BaiduLocation></BaiduLocation>
    </keep-alive>
    <van-search class="search-box" v-model="keyword" placeholder="搜索景点/美食" @search="onSearch"/>
    <van-list class="home-list" v-model:loading="listLoading" :finished="listFinished" finished-text="没有更多了" @load="onListLoad">
      <van-row>
        <van-col span="12">
          <div v-for="(item, i) in list" :key="item.id">
            <van-cell v-if="i % 2 == 0">
              <PlaceOverview :place="item"></PlaceOverview>
            </van-cell>
          </div>
        </van-col>
        <van-col span="12">
          <div v-for="(item, i) in list" :key="item.id">
            <van-cell v-if="i % 2 == 1">
              <PlaceOverview :place="item"></PlaceOverview>
            </van-cell>
          </div>
        </van-col>
      </van-row>
    </van-list>
  </van-config-provider>
</template>

<script>
  import PlaceOverview from '@/components/PlaceOverview.vue';
  import BaiduLocation from '../components/BaiduLocation.vue'
  import common from '../common'

  export default {
    setup() {
        const custom = {
            searchPadding: "20px",
            tabFontSize: "18px",
            searchInputHeight: "40px"
        };
        return {
            custom,
        };
    },
    data() {
        return {
          keyword: null,
          listLoading: false,
          listFinished: false,
          listPageIndex: 0,
          list: [],
          map: null,
          localMarker: null,
          token: null
        };
    },
    components: { PlaceOverview, BaiduLocation },
    methods: {
      onListLoad() {
        let json = localStorage.getItem('location');
        if (!json) {
          setTimeout(this.onListLoad, 500);
          return;
        }

        let l = JSON.parse(json);
        let _this = this;
        let url = `${common.baseUrl}/place/nearby?lng=${l.lng}&lat=${l.lat}&pageIndex=${this.listPageIndex}&pageCap=10`;
        if (this.keyword && this.keyword.length > 0) {
          url = `${common.baseUrl}/place/search?keyword=${this.keyword}&lng=${l.lng}&lat=${l.lat}&pageIndex=${this.listPageIndex}&pageCap=10`;
        }
        fetch(url)
          .then(response => response.json())
          .then(data => {
            if (data.code == 0) {
              _this.listPageIndex = _this.listPageIndex + 1;
              _this.listLoading = false;
              data.data.forEach(x => _this.list.push(x));
              if (data.data.length < 10) {
                _this.listFinished = true;
              }
            }
            else {
              _this.listLoading = false;
            }
          });
      },
      onSearch() {
        this.listPageIndex = 0;
        this.list = [];
        this.listFinished = false;
        this.listLoading = true;
        this.onListLoad();
      }
    },
    created() {
      this.token = this.$route.query.token;
      if (!this.token) {
        this.token = localStorage.getItem("token.user");
      }
      else {
        localStorage.setItem("token.user", this.token);
      }

      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=home&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=home');
        }
      }
    }
  }
</script>

<style>
  .van-tabs {
    padding-top: 14px;
  }
  .search-box {
    font-size: 15px;
  }
  .home-list {
    padding-bottom: 50px;
  }
</style>