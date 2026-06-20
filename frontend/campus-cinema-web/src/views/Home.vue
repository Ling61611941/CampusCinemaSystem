<template>
  <div class="page">
    <div class="header">
      <h1>校园影院购票与排片管理系统</h1>
      <p>基于 Vue 3 + ASP.NET Core Web API 9.0 + SQL Server</p>
    </div>

    <div class="search-box">
      <el-input
        v-model="keyword"
        placeholder="请输入电影名称"
        style="width: 300px"
        @keyup.enter="loadMovies"
      />

      <el-button type="primary" @click="loadMovies">搜索</el-button>

      <el-button @click="$router.push('/admin/movies')">
        进入后台电影管理
      </el-button>
    </div>

    <el-alert
      v-if="errorMessage"
      :title="errorMessage"
      type="error"
      show-icon
      style="margin-bottom: 20px"
    />

    <el-row :gutter="20">
      <el-col
        v-for="movie in movies"
        :key="movie.id"
        :xs="24"
        :sm="12"
        :md="8"
        :lg="6"
      >
        <el-card class="movie-card" shadow="hover">
          <img :src="movie.posterUrl" class="poster" />

          <h3>{{ movie.name }}</h3>

          <p>类型：{{ movie.category }}</p>
          <p>导演：{{ movie.director }}</p>
          <p>片长：{{ movie.duration }} 分钟</p>
          <p>评分：{{ movie.score }}</p>

          <el-button type="primary" @click="$router.push(`/movies/${movie.id}`)">
            查看详情
          </el-button>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getMovies } from '../api/movie'

const movies = ref([])
const keyword = ref('')
const errorMessage = ref('')

const loadMovies = async () => {
  try {
    errorMessage.value = ''
    const res = await getMovies({
      keyword: keyword.value
    })

    movies.value = res.data
  } catch (error) {
    console.error(error)
    errorMessage.value = '电影数据加载失败，请确认后端 5080 端口正在运行。'
  }
}

onMounted(() => {
  loadMovies()
})
</script>

<style scoped>
.page {
  padding: 30px;
  background: #f5f7fa;
  min-height: 100vh;
}

.header {
  text-align: center;
  margin-bottom: 30px;
}

.header h1 {
  margin-bottom: 10px;
  color: #303133;
}

.header p {
  color: #606266;
}

.search-box {
  margin-bottom: 25px;
  display: flex;
  gap: 10px;
}

.movie-card {
  margin-bottom: 20px;
}

.poster {
  width: 100%;
  height: 300px;
  object-fit: cover;
  border-radius: 6px;
}
</style>
