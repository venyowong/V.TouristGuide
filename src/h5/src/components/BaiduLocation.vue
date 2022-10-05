<template>
  <van-config-provider :theme-vars="custom">
    <van-row class="address-row">
      <van-icon class="address-icon" name="location-o" size="23px"/>
      <van-tag class="address" round type="primary" @click="modifyLocation">{{address}}</van-tag>
    </van-row>
  </van-config-provider>
</template>

<script>
  import { Toast } from 'vant';

  let geolocation = new window.BMapGL.Geolocation();
  // 开启SDK辅助定位
  geolocation.enableSDKLocation();
  let gc = new window.BMapGL.Geocoder();//创建地理编码器

  export default {
    setup() {
        const custom = {
            searchPadding: "20px",
            tabFontSize: "18px",
            searchInputHeight: "40px"
        };
        return {
            custom
        };
    },
    data() {
      return {
        address: "点击获取当前位置",
        lng: null,
        lat: null
      };
    },
    methods: {
      getLocation() {
        let _this = this;
        geolocation.getCurrentPosition(function(r){
            if(this.getStatus() == window.BMAP_STATUS_SUCCESS){
              gc.getLocation(r.point, function(rs){    
                let l = {
                  address: rs.address,
                  lng: rs.point.lng,
                  lat: rs.point.lat
                };
                localStorage.setItem('location', JSON.stringify(l));
                _this.address = l.address;
                _this.lng = l.lng;
                _this.lat = l.lat;
              }); 
            }
            else {
              Toast("获取当前位置失败");
            }        
        });
      },
      modifyLocation() {
        if (this.lng) {
          this.$router.push({
            path: "/modifyLocation"
          });
        }
        else {
          this.getLocation();
        }
      }
    },
    activated() {
      let json = localStorage.getItem('location');
      if (!json) {
        this.getLocation();
        return;
      }

      let l = JSON.parse(json);
      this.address = l.address;
      this.lng = l.lng;
      this.lat = l.lat;
      this.$forceUpdate();
    }
  }
</script>

<style>
  .address-row {
    padding-top: 24px;
    padding-left: 14px;
    padding-right: 14px;
  }
  .address {
    padding: 4px;
  }
  .address-icon {
    padding-right: 3px;
  }
</style>