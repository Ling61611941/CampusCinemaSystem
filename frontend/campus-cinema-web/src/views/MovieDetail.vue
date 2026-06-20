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
        </div>
      </div>
    </el-card>

    <el-card class="schedule-card">
      <template #header>
        <strong>电影场次</strong>
      </template>

      <el-empty v-if="schedules.length === 0" description="暂无排片" />

      <el-table v-else :data="schedules" border>
        <el-table-column label="影厅">
          <template #default="scope">
            {{ scope.row.hall?.name || '未知影厅' }}
          </template>
        </el-table-column>

        <el-table-column prop="startTime" label="开始时间" width="190" />
        <el-table-column prop="endTime" label="结束时间" width="190" />
        <el-table-column prop="price" label="票价" width="100" />
        <el-table-column prop="status" label="状态" width="100" />

        <el-table-column label="操作" width="140">
          <template #default>
            <el-button
  type="primary"
  size="small"
  @click="$router.push(`/booking/${scope.row.id}`)"
>
  选座购票
</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getMovie } from '../api/movie'
import { getSchedulesByMovie } from '../api/schedule'

const route = useRoute()
const movie = ref(null)
const schedules = ref([])

onMounted(async () => {
  const movieRes = await getMovie(route.params.id)
  movie.value = movieRes.data

  const scheduleRes = await getSchedulesByMovie(route.params.id)
  schedules.value = scheduleRes.data
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

.schedule-card {
  margin-top: 20px;
}
</style>
