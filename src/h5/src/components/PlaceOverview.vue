<template>
  <div @click="toDetail">
    <img class="place-overview-img" :src="place.cover"/>
    <div class="place-overview-corner">📍{{formatDistance(place.distance)}}</div>
    <div class="place-overview-name">{{place.name}}</div>
  </div>
</template>

<script>
  export default {
    props: ["place"],
    methods: {
      formatDistance(m) {
        if (m < 1000) {
          return `${Math.round(m)}m`;
        }

        return `${(m / 1000).toFixed(2)}km`;
      },
      toDetail() {
        this.$router.push({
          path: "/placeDetail",
          query: {
            id: this.place.id
          }
        });
      }
    }
  }
</script>

<style>
  .place-overview-img {
    border-radius: 15px;
    width: 100%;
  }
  .place-overview-corner {
    position: absolute;
    bottom: 34px;
    right: 5%;
    color: white;
  }
  .place-overview-name {
    padding-left: 10px;
    font-weight: bold;
  }
</style>