<template>
  <van-config-provider :theme-vars="custom">
    <keep-alive>
      <BaiduLocation></BaiduLocation>
    </keep-alive>
    <van-search class="search-box" v-model="keyword" placeholder="搜索景点/美食" @search="getNearbyPlaces"/>
    <div id="map" class="map"></div>

    <van-action-sheet v-model:show="showPlace" :title="place.name">
      <div class="content">
        <div class="map-place-cover">
          <img class="map-place-cover-img" :src="place.cover" />
        </div>
        <van-cell v-if="place.openTime != null && place.openTime.length > 0" title="开放时间" :value="place.openTime" icon="shop-o" size="large"/>
        <van-cell v-if="place.tel != null && place.tel.length > 0" title="联系方式" :value="place.tel" icon="phone-o" size="large"/>
        <van-cell title="距离" :value="place.distance" icon="guide-o" size="large"/>
        <van-cell-group title="地址">
          <van-cell :title="place.address" is-link icon="location-o" @click="selectMap"/>
        </van-cell-group>
      </div>
    </van-action-sheet>

    <van-action-sheet v-model:show="mapSelection" :actions="mapActions" @select="onMapSelected" cancel-text="取消" close-on-click-action/>
  </van-config-provider>
</template>

<script>
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
        map: null,
        placeOverlays: [],
        places: [],
        showPlace: false,
        place: {},
        lat: null,
        lng: null,
        address: null,
        mapActions: [
          {name: "百度地图"},
          {name: "高德地图"}
        ],
        mapSelection: false
      };
    },
    components: { BaiduLocation },
    methods: {
      getNearbyPlaces() {
        let _this = this;
        let l = JSON.parse(localStorage.getItem('location'));
        let url = `${common.baseUrl}/place/nearby?lng=${l.lng}&lat=${l.lat}&pageIndex=0&pageCap=10000&range=${this.getRange()}`;
        if (this.keyword && this.keyword.length > 0) {
          url = `${common.baseUrl}/place/search?keyword=${this.keyword}&lng=${l.lng}&lat=${l.lat}&pageIndex=0&pageCap=10000&range=${this.getRange()}`;
        }
        fetch(url)
          .then(response => response.json())
          .then(data => {
            if (data.code == 0) {
              _this.placeOverlays.forEach(x => common.removeOverlay(_this.map, x));
              data.data.forEach(x => x.distance = common.formatDistance(x.distance));
              _this.places = data.data;
              data.data.forEach(x => {
                let point = new window.BMapGL.Point(x.lng, x.lat);  // 创建点坐标 
                _this.placeOverlays.push(common.addOverlay(_this.map, point, x.name));
              });
            }
          });
      },
      getRange() {
        var z = this.map.getZoom();
        return 20000000 * Math.pow(Math.E, -0.594 * z);
      },
      selectMap() {
        this.showPlace = false;
        this.mapSelection = true;
      },
      onMapSelected(item) {
        this.mapSelection = false;

        let url = "";
        if (item.name == "百度地图") {
          url = `http://api.map.baidu.com/direction?origin=latlng:${this.lat},${this.lng}|name:${this.address}&destination=latlng:${this.place.lat},${this.place.lng}|name:${this.place.address}&mode=driving&output=html&coord_type=bd09ll&src=webapp.venyo.touristguide&region=${this.place.region}`;
        }
        else if (item.name == "高德地图") {
          let local = common.toGaodeLatlng(this.lat, this.lng);
          let destination = common.toGaodeLatlng(this.place.lat, this.place.lng);
          url = `https://uri.amap.com/navigation?from=${local.lng},${local.lat},${this.address}&to=${destination.lng},${destination.lat},${this.place.address}&src=webapp.venyo.touristguide&callnative=1`;
        }
        window.open(url);
      }
    },
    mounted() {
      if (common.addPageView) {
        let user = localStorage.user;
        if (user) {
          user = JSON.parse(user);
          fetch(`https://vbranch.cn/talog/metric/pg/add?index=tg&page=map&user=${user.id}`);
        }
        else {
          fetch('https://vbranch.cn/talog/metric/pg/add?index=tg&page=map');
        }
      }

      let map = new window.BMapGL.Map("map");          // 创建地图实例 
      let location = JSON.parse(localStorage.getItem('location'));
      this.lat = location.lat;
      this.lng = location.lng;
      this.address = location.address;
      let point = new window.BMapGL.Point(location.lng, location.lat);  // 创建点坐标 
      map.centerAndZoom(point, 15);                 // 初始化地图，设置中心点坐标和地图级别
      map.enableScrollWheelZoom();
      this.map = map;
      let _this = this;
      this.map.addEventListener("zoomend", function() {
        _this.getNearbyPlaces();
      });

      this.map.addEventListener("click", function(e){    
        if (e.overlay) {
          let d = common.getDistance(location.lat, location.lng, e.overlay.latLng.lat, e.overlay.latLng.lng);

          let min = 1000000;
          let place = null;
          _this.places.forEach(p => {
            let distance = common.getDistance(p.lat, p.lng, e.overlay.latLng.lat, e.overlay.latLng.lng);
            if (distance < min) {
              min = distance;
              place = p;
            }
          });
          if (d < min) {
            return;
          }

          _this.showPlace = true;
          _this.place = place;
        }
      });

      this.getNearbyPlaces();
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
  .map {
    position: absolute;
    left: 0;
    width: 100%;
    height: calc(100% - 178px);
    top: 128px;
  }
  .map-place-cover {
    text-align: center;
  }
  .map-place-cover-img {
    height: 150px;
  }
</style>