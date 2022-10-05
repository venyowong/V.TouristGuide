<template>
  <div class="page">
    <van-nav-bar title="修改当前位置" left-text="返回" left-arrow @click-left="onClickLeft"/>
    <van-search v-model="keyword" placeholder="请输入关键词" @search="search" />
    <div id="map" class="map"></div>
  </div>
</template>

<script>
  import { Dialog, Toast } from 'vant';
  import common from "../common"

  export default {
    setup() {
      const onClickLeft = () => history.back();
      return {
        onClickLeft,
      };
    },
    data() {
      return {
        keyword: null,
        map: null,
        localSearch: null,
        localOverlay: null
      };
    },
    mounted() {
      let _this = this;
      let map = new window.BMapGL.Map("map");          // 创建地图实例 
      let location = JSON.parse(localStorage.getItem('location'));
      var point = new window.BMapGL.Point(location.lng, location.lat);  // 创建点坐标 
      this.localOverlay = common.addOverlay(map, point, "当前位置");
      map.centerAndZoom(point, 15);                 // 初始化地图，设置中心点坐标和地图级别
      map.enableScrollWheelZoom();
      let local = new window.BMapGL.LocalSearch(map, {      
        renderOptions:{map: map}      
      }); 
      this.map = map;
      this.map.addEventListener("click", function(e){    
        if (e.overlay && e.overlay._config.title && e.overlay._config.title.length > 0) {
          Dialog.confirm({
            title: `确认修改`,
            message: `是否将${e.overlay._config.title}设置为当前位置？`
          })
            .then(() =>
            {
              let l = {
                address: e.overlay._config.title,
                lng: e.overlay.latLng.lng,
                lat: e.overlay.latLng.lat
              };
              localStorage.setItem('location', JSON.stringify(l));
              Toast("设置成功");
              _this.onClickLeft();
            });
        }
      });
      this.localSearch = local;
    },
    methods: {
      search() {
        common.removeOverlay(this.map, this.localOverlay);
        this.localSearch.search(this.keyword);
      }
    }
  };
</script>

<style>
  .map {
    position: absolute;
    left: 0;
    width: 100%;
    height: calc(100% - 150px);
    top: 100px;
  }
</style>