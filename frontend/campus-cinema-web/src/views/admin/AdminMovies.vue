<template>
  <div class="page">
    <div class="top">
      <h2>后台电影管理</h2>
      <el-button @click="$router.push('/')">返回前台</el-button>
    </div>

    <el-table :data="movies" border style="margin-top: 20px">
      <el-table-column prop="id" label="编号" width="80" />
      <el-table-column prop="name" label="电影名称" />
      <el-table-column prop="category" label="类型" />
      <el-table-column prop="director" label="导演" />
      <el-table-column prop="duration" label="片长" />
      <el-table-column prop="score" label="评分" />
      <el-table-column prop="status" label="状态" />
    </el-table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getMovies } from '../../api/movie'

const movies = ref([])

const loadMovies = async () => {
  const res = await getMovies()
  movies.value = res.data
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

.top {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
