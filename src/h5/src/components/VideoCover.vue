<template>
  <div class="video-cover" :style="{height: height}">
    <img :style="{width: width}" :src="url"/>
    <img class="video-play" src="../assets/play-video.png"/>
  </div>
</template>

<script>
  export default {
    props: ["url", "width", "height"]
  }
</script>

<style>
  .video-cover {
    background-color: black;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    display:flex;
    align-items: center;
  }
  .video-play {
    position: absolute;
    width: 35px;
    left: 75px;
    top: 75px;
  }
</style>