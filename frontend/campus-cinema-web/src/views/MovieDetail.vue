<template>
  <div class="page" v-if="movie">
    <el-button @click="$router.push('/')">返回首页</el-button>

    <el-card class="detail-card">
      <div class="detail">
        <img :src="movie.posterUrl" class="poster" />

        <div>
          <h2>{{ movie.name }}</h2>
          <p>类型：{{ movie.category }}</p>
          <p>导演：{{ movie.director }}</p>
          <p>主演：{{ movie.actors }}</p>
          <p>片长：{{ movie.duration }} 分钟</p>
          <p>上映时间：{{ movie.releaseDate }}</p>
          <p>评分：{{ movie.score }}</p>
          <p>状态：{{ movie.status }}</p>
          <p class="desc">{{ movie.description }}</p>

          <el-button type="primary">
            查看场次并购票
          </el-button>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getMovie } from '../api/movie'

const route = useRoute()
const movie = ref(null)

onMounted(async () => {
  const res = await getMovie(route.params.id)
  movie.value = res.data
})
</script>

<style scoped>
.page {
  padding: 30px;
  background: #f5f7fa;
  min-height: 100vh;
}

.detail-card {
  margin-top: 20px;
}

.detail {
  display: flex;
  gap: 30px;
}

.poster {
  width: 260px;
  height: 360px;
  object-fit: cover;
  border-radius: 6px;
}

.desc {
  margin-top: 20px;
  line-height: 1.8;
}
</style>
