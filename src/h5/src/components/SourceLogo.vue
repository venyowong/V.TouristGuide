<template>
  <div>
    <div class="source-name">——{{name}}</div>
  </div>
</template>

<script>
  export default {
    props: ["source"],
    data() {
      return {
        name: null,
        logo: null
      };
    },
    created() {
      if (this.source == "zhihu") {
        this.name = "知乎";
      }
      else if (this.source == "tencent") {
        this.name = "腾讯";
      }
      else if (this.source == "ctrip") {
        this.name = "携程";
      }
      else if (this.source == "dianping") {
        this.name = "大众点评";
      }
      else if (this.source == "mafengwo") {
        this.name = "马蜂窝";
      }
      else if (this.source == "qzworldemporium") {
        this.name = "泉州世遗";
      }
      else if (this.source == "sohu") {
        this.name = "搜狐";
      }
      else if (this.source == "toutiao") {
        this.name = "头条";
      }
      else {
        this.name = this.source;
      }
    }
  }
</script>

<style>
  .source-name {
    font-weight: bold;
  }
</style>