<template>
  <div style="margin: 16px;">
    <van-button round block type="primary" @click="onClick" v-model:disabled="fetching">
      <van-loading size="14" color="white" v-if="fetching">{{text}}</van-loading>
      <div v-if="!fetching">{{text}}</div>
    </van-button>
  </div>
</template>

<script>
  export default {
    props: ["text", "onClick"],
    data() {
      return {
        fetching: false
      };
    },
    methods: {
      setFetching(f) {
        this.fetching = f;
      }
    }
  }
</script>