<template>
  <div class="home-page">
    <section class="home-hero">
      <div class="hero-content">
        <div class="hero-badge">Campus Cinema System</div>

        <h1>校园影院购票与排片管理系统</h1>

        <p>
          集电影浏览、场次排片、在线选座、订单管理于一体的校园影院信息系统
        </p>

        <div class="hero-actions">
          <el-button type="primary" size="large" @click="scrollToMovies">
            浏览电影
          </el-button>

          <el-button size="large" @click="$router.push('/admin/movies')">
            进入后台管理
          </el-button>
        </div>
      </div>
    </section>

    <main class="home-container">
      <el-card class="search-card">
        <div class="search-row">
          <div>
            <h2>正在热映</h2>
            <p>选择喜欢的电影，查看场次并进行选座购票</p>
          </div>

          <div class="search-box">
            <el-input
              v-model="keyword"
              placeholder="请输入电影名称、类型或导演"
              clearable
            />
            <el-button type="primary" @click="loadMovies">刷新</el-button>
          </div>
        </div>
      </el-card>

      <div id="movie-list" class="movie-list">
        <el-row :gutter="24">
          <el-col
            v-for="movie in filteredMovies"
            :key="movie.id"
            :xs="24"
            :sm="12"
            :md="8"
            :lg="6"
          >
            <el-card class="movie-card" shadow="hover">
              <div class="poster-box">
                <img
                  v-if="movie.posterUrl"
                  :src="movie.posterUrl"
                  :alt="movie.name"
                />

                <div v-else class="poster-placeholder">
                  <span>🎬</span>
                  <p>{{ movie.name }}</p>
                </div>

                <el-tag class="status-tag" :type="getStatusType(movie.status)">
                  {{ movie.status || '未知状态' }}
                </el-tag>
              </div>

              <div class="movie-info">
                <h3>{{ movie.name }}</h3>

                <div class="movie-meta">
                  <span>{{ movie.category || '未分类' }}</span>
                  <span>{{ movie.duration || 0 }} 分钟</span>
                </div>

                <p class="director">
                  导演：{{ movie.director || '暂无' }}
                </p>

                <p class="score">
                  评分：
                  <strong>{{ movie.score || 0 }}</strong>
                </p>

                <el-button
                  type="primary"
                  class="detail-button"
                  @click="$router.push(`/movies/${movie.id}`)"
                >
                  查看详情
                </el-button>
              </div>
            </el-card>
          </el-col>
        </el-row>

        <el-empty
          v-if="filteredMovies.length === 0"
          description="暂无匹配电影"
        />
      </div>
    </main>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue'
import { getMovies } from '../api/movie'
import { ElMessage } from 'element-plus'

const movies = ref([])
const keyword = ref('')

const loadMovies = async () => {
  try {
    const res = await getMovies()
    movies.value = res.data
  } catch (error) {
    ElMessage.error('电影数据加载失败，请检查后端接口是否启动')
  }
}

const filteredMovies = computed(() => {
  const key = keyword.value.trim().toLowerCase()

  if (!key) {
    return movies.value
  }

  return movies.value.filter(movie => {
    return (
      movie.name?.toLowerCase().includes(key) ||
      movie.category?.toLowerCase().includes(key) ||
      movie.director?.toLowerCase().includes(key)
    )
  })
})

const getStatusType = (status) => {
  if (status === '正在热映') return 'success'
  if (status === '即将上映') return 'warning'
  if (status === '下架') return 'info'
  return 'primary'
}

const scrollToMovies = () => {
  document.querySelector('#movie-list')?.scrollIntoView({
    behavior: 'smooth'
  })
}

onMounted(() => {
  loadMovies()
})
</script>

<style scoped>
.home-page {
  min-height: 100vh;
  background: #f3f6fb;
}

.home-hero {
  min-height: 330px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 48px 24px;
  background:
    linear-gradient(135deg, rgba(37, 99, 235, 0.9), rgba(14, 165, 233, 0.78)),
    url('https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?auto=format&fit=crop&w=1600&q=80');
  background-size: cover;
  background-position: center;
  color: #ffffff;
}

.hero-content {
  max-width: 900px;
  text-align: center;
}

.hero-badge {
  display: inline-block;
  padding: 8px 18px;
  margin-bottom: 18px;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.18);
  font-size: 14px;
  letter-spacing: 1px;
}

.hero-content h1 {
  margin: 0;
  font-size: 42px;
  font-weight: 800;
  color: #ffffff;
}

.hero-content p {
  margin: 18px auto 28px;
  max-width: 720px;
  color: #eaf2ff;
  font-size: 17px;
  line-height: 1.8;
}

.hero-actions {
  display: flex;
  justify-content: center;
  gap: 14px;
}

.home-container {
  max-width: 1200px;
  margin: -36px auto 0;
  padding: 0 24px 40px;
}

.search-card {
  margin-bottom: 28px;
}

.search-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 24px;
}

.search-row h2 {
  margin: 0 0 8px;
  color: #1e3a8a;
  font-size: 26px;
}

.search-row p {
  margin: 0;
  color: #64748b;
}

.search-box {
  display: flex;
  gap: 12px;
  min-width: 420px;
}

.movie-list {
  margin-top: 12px;
}

.movie-card {
  margin-bottom: 24px;
  height: 100%;
}

.poster-box {
  position: relative;
  height: 230px;
  border-radius: 12px;
  overflow: hidden;
  background: #e5e7eb;
}

.poster-box img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.poster-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #dbeafe, #eff6ff);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.poster-placeholder span {
  font-size: 52px;
}

.poster-placeholder p {
  color: #1e3a8a;
  font-weight: 700;
  padding: 0 14px;
  text-align: center;
}

.status-tag {
  position: absolute;
  top: 12px;
  right: 12px;
}

.movie-info {
  padding-top: 16px;
}

.movie-info h3 {
  margin: 0 0 12px;
  color: #1e3a8a;
  font-size: 20px;
}

.movie-meta {
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
  color: #64748b;
  font-size: 14px;
}

.director {
  margin: 8px 0;
  color: #4b5563;
}

.score {
  margin: 8px 0 16px;
  color: #4b5563;
}

.score strong {
  color: #f59e0b;
  font-size: 18px;
}

.detail-button {
  width: 100%;
}

@media (max-width: 768px) {
  .hero-content h1 {
    font-size: 30px;
  }

  .search-row {
    flex-direction: column;
    align-items: stretch;
  }

  .search-box {
    min-width: 0;
    flex-direction: column;
  }
}
</style>