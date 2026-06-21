<template>
  <div class="detail-page" v-if="movie">
    <div class="detail-container">
      <el-button class="back-button" @click="$router.push('/')">
        返回首页
      </el-button>

      <el-card class="movie-detail-card">
        <div class="movie-detail">
          <div class="poster-area">
            <img
              v-if="movie.posterUrl"
              :src="movie.posterUrl"
              :alt="movie.name"
            />

            <div v-else class="poster-placeholder">
              <span>🎬</span>
              <p>{{ movie.name }}</p>
            </div>
          </div>

          <div class="info-area">
            <div class="title-row">
              <h1>{{ movie.name }}</h1>

              <el-tag :type="getStatusType(movie.status)" size="large">
                {{ movie.status || '未知状态' }}
              </el-tag>
            </div>

            <div class="meta-list">
              <div class="meta-item">
                <span class="label">电影类型</span>
                <strong>{{ movie.category || '暂无' }}</strong>
              </div>

              <div class="meta-item">
                <span class="label">导演</span>
                <strong>{{ movie.director || '暂无' }}</strong>
              </div>

              <div class="meta-item">
                <span class="label">主演</span>
                <strong>{{ movie.actors || '暂无' }}</strong>
              </div>

              <div class="meta-item">
                <span class="label">片长</span>
                <strong>{{ movie.duration || 0 }} 分钟</strong>
              </div>

              <div class="meta-item">
                <span class="label">上映日期</span>
                <strong>{{ formatDate(movie.releaseDate) }}</strong>
              </div>

              <div class="meta-item">
                <span class="label">评分</span>
                <strong class="score">{{ movie.score || 0 }}</strong>
              </div>
            </div>

            <div class="description">
              <h3>影片简介</h3>
              <p>{{ movie.description || '暂无影片简介' }}</p>
            </div>
          </div>
        </div>
      </el-card>

      <el-card class="schedule-card">
        <template #header>
          <div class="card-header">
            <span>电影场次</span>
            <el-button @click="$router.push('/admin/schedules')">
              后台排片管理
            </el-button>
          </div>
        </template>

        <el-table
          v-if="schedules.length > 0"
          :data="schedules"
          border
          style="width: 100%"
        >
          <el-table-column label="影厅" width="160">
            <template #default="scope">
              {{ scope.row.hall?.name || '未知影厅' }}
            </template>
          </el-table-column>

          <el-table-column label="开始时间" width="190">
            <template #default="scope">
              {{ formatDateTime(scope.row.startTime) }}
            </template>
          </el-table-column>

          <el-table-column label="结束时间" width="190">
            <template #default="scope">
              {{ formatDateTime(scope.row.endTime) }}
            </template>
          </el-table-column>

          <el-table-column label="票价" width="120">
            <template #default="scope">
              <span class="price">￥{{ scope.row.price }}</span>
            </template>
          </el-table-column>

          <el-table-column label="状态" width="120">
            <template #default="scope">
              <el-tag :type="scope.row.status === '正常' ? 'success' : 'info'">
                {{ scope.row.status }}
              </el-tag>
            </template>
          </el-table-column>

          <el-table-column label="操作" width="160">
            <template #default="scope">
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

        <el-empty
          v-else
          description="该电影暂无排片，请先到后台排片管理中添加场次"
        />
      </el-card>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { getMovie } from '../api/movie'
import { getSchedulesByMovie } from '../api/schedule'
import { ElMessage } from 'element-plus'

const route = useRoute()

const movie = ref(null)
const schedules = ref([])

const loadMovie = async () => {
  try {
    const res = await getMovie(route.params.id)
    movie.value = res.data
  } catch (error) {
    ElMessage.error('电影详情加载失败')
  }
}

const loadSchedules = async () => {
  try {
    const res = await getSchedulesByMovie(route.params.id)
    schedules.value = res.data
  } catch (error) {
    ElMessage.error('排片数据加载失败')
  }
}

const getStatusType = (status) => {
  if (status === '正在热映') return 'success'
  if (status === '即将上映') return 'warning'
  if (status === '下架') return 'info'
  return 'primary'
}

const formatDate = (value) => {
  if (!value) return '暂无'
  return value.toString().replace('T', ' ').substring(0, 10)
}

const formatDateTime = (value) => {
  if (!value) return '暂无'
  return value.toString().replace('T', ' ').substring(0, 16)
}

onMounted(() => {
  loadMovie()
  loadSchedules()
})
</script>

<style scoped>
.detail-page {
  min-height: 100vh;
  background: #f3f6fb;
  padding: 32px 24px 48px;
}

.detail-container {
  max-width: 1200px;
  margin: 0 auto;
}

.back-button {
  margin-bottom: 18px;
}

.movie-detail-card {
  margin-bottom: 26px;
}

.movie-detail {
  display: grid;
  grid-template-columns: 260px 1fr;
  gap: 32px;
  align-items: stretch;
}

.poster-area {
  height: 360px;
  border-radius: 16px;
  overflow: hidden;
  background: #e5e7eb;
}

.poster-area img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.poster-placeholder {
  height: 100%;
  background: linear-gradient(135deg, #dbeafe, #eff6ff);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.poster-placeholder span {
  font-size: 64px;
}

.poster-placeholder p {
  padding: 0 18px;
  text-align: center;
  color: #1e3a8a;
  font-weight: 700;
}

.info-area {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.title-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
}

.title-row h1 {
  margin: 0;
  font-size: 34px;
  color: #1e3a8a;
}

.meta-list {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 14px;
  margin: 26px 0;
}

.meta-item {
  padding: 14px 16px;
  border-radius: 12px;
  background: #f8fafc;
  border: 1px solid #e5e7eb;
}

.meta-item .label {
  display: block;
  margin-bottom: 8px;
  color: #64748b;
  font-size: 13px;
}

.meta-item strong {
  color: #1f2937;
  font-size: 15px;
}

.meta-item .score {
  color: #f59e0b;
  font-size: 20px;
}

.description {
  padding: 18px 20px;
  border-radius: 12px;
  background: #eff6ff;
  border: 1px solid #dbeafe;
}

.description h3 {
  margin: 0 0 10px;
  color: #1e3a8a;
}

.description p {
  margin: 0;
  line-height: 1.8;
  color: #475569;
}

.schedule-card {
  margin-top: 26px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header span {
  font-weight: 700;
  color: #1e3a8a;
}

.price {
  color: #dc2626;
  font-weight: 700;
}

@media (max-width: 900px) {
  .movie-detail {
    grid-template-columns: 1fr;
  }

  .poster-area {
    height: 300px;
  }

  .meta-list {
    grid-template-columns: 1fr;
  }

  .title-row {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>