<template>
  <div>
    <van-cell center :title="title" v-if="!edit" size="large">
      <template #value>
        <img class="editable-cell-img" v-if="type == 'img'" :src="val" />
        <div v-else>{{val}}</div>
      </template>
      <template #right-icon>
        <van-icon name="edit" @click="onEdit"/>
      </template>
    </van-cell>

    <van-field v-model="val" :label="title" v-if="edit && type == 'text'" size="large">
      <template #button>
        <van-button size="small" type="primary" @click="onSave">保存</van-button>
      </template>
    </van-field>
    <van-field :label="title" v-if="edit && type == 'img'" size="large">
      <template #button>
        <van-uploader :after-read="afterRead" preview-size="36"/>
      </template>
    </van-field>
    <van-field type="textarea" rows="4" v-model="val" :label="title" v-if="edit && type == 'textarea'" size="large">
      <template #button>
        <van-button size="small" type="primary" @click="onSave">保存</van-button>
      </template>
    </van-field>
  </div>
</template>

<script>
import common from '@/common';

  export default {
    props: ["title", "value", "onSet", "type"],
    data() {
      return {
        edit: false,
        val: null,
        token: null
      };
    },
    methods: {
      onEdit() {
        this.edit = true;
      },
      onSave() {
        if (this.onSet) {
          this.onSet(this.title, this.value, this.val);
        }
        this.edit = false;
      },
      changeValue(v) {
        this.val = v;
      },
      afterRead(file) {
        let _this = this;
        let formData = new FormData();
        formData.append('file', file.file);
        fetch(`${common.baseUrl}/user/file/upload`, {
          method: 'POST',
          body: formData,
          headers: {
            token: this.token
          }
        })
        .then(response => response.json())
        .then(data => {
          _this.val = data.data.value;
          if (_this.onSet) {
            _this.onSet(_this.title, _this.value, _this.val);
          }
          _this.edit = false;
        })
        .catch(e => {
          console.log(e);
        });
      }
    },
    beforeMount() {
      this.val = this.value;
      this.token = localStorage.getItem("token.user");
      if (!this.token) {
        this.$router.push({
          path: "/login"
        });
        return;
      }
    }
  }
</script>

<style>
  .editable-cell-img {
    height: 36px;
  }
</style>