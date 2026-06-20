<template>
  <div class="page">
    <div class="top">
      <h2>后台电影管理</h2>
      <div>
        <el-button type="primary" @click="openAddDialog">新增电影</el-button>
        <el-button @click="$router.push('/')">返回前台</el-button>
      </div>
    </div>

    <el-table :data="movies" border style="margin-top: 20px">
      <el-table-column prop="id" label="编号" width="80" />
      <el-table-column prop="name" label="电影名称" />
      <el-table-column prop="category" label="类型" />
      <el-table-column prop="director" label="导演" />
      <el-table-column prop="duration" label="片长" width="100" />
      <el-table-column prop="score" label="评分" width="100" />
      <el-table-column prop="status" label="状态" width="120" />

      <el-table-column label="操作" width="180">
        <template #default="scope">
          <el-button size="small" @click="openEditDialog(scope.row)">
            编辑
          </el-button>
          <el-button size="small" type="danger" @click="removeMovie(scope.row.id)">
            删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑电影' : '新增电影'"
      width="650px"
    >
      <el-form label-width="90px">
        <el-form-item label="电影名称">
          <el-input v-model="form.name" placeholder="请输入电影名称" />
        </el-form-item>

        <el-form-item label="电影类型">
          <el-input v-model="form.category" placeholder="例如：科幻、悬疑、动画" />
        </el-form-item>

        <el-form-item label="导演">
          <el-input v-model="form.director" placeholder="请输入导演" />
        </el-form-item>

        <el-form-item label="主演">
          <el-input v-model="form.actors" placeholder="请输入主演" />
        </el-form-item>

        <el-form-item label="简介">
          <el-input v-model="form.description" type="textarea" :rows="4" />
        </el-form-item>

        <el-form-item label="海报地址">
          <el-input v-model="form.posterUrl" />
        </el-form-item>

        <el-form-item label="片长">
          <el-input-number v-model="form.duration" :min="1" />
          <span style="margin-left: 10px">分钟</span>
        </el-form-item>

        <el-form-item label="上映时间">
          <el-date-picker
            v-model="form.releaseDate"
            type="datetime"
            value-format="YYYY-MM-DDTHH:mm:ss"
          />
        </el-form-item>

        <el-form-item label="评分">
          <el-input-number
            v-model="form.score"
            :min="0"
            :max="10"
            :precision="1"
            :step="0.1"
          />
        </el-form-item>

        <el-form-item label="状态">
          <el-select v-model="form.status">
            <el-option label="正在热映" value="正在热映" />
            <el-option label="即将上映" value="即将上映" />
            <el-option label="已下架" value="已下架" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="saveMovie">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getMovies, createMovie, updateMovie, deleteMovie } from '../../api/movie'
import { ElMessage, ElMessageBox } from 'element-plus'

const movies = ref([])
const dialogVisible = ref(false)
const isEdit = ref(false)

const emptyForm = () => ({
  id: 0,
  name: '',
  category: '',
  director: '',
  actors: '',
  description: '',
  posterUrl: 'https://picsum.photos/300/420?random=10',
  duration: 120,
  releaseDate: '2026-04-20T00:00:00',
  score: 8.0,
  status: '正在热映'
})

const form = ref(emptyForm())

const loadMovies = async () => {
  const res = await getMovies()
  movies.value = res.data
}

const openAddDialog = () => {
  isEdit.value = false
  form.value = emptyForm()
  dialogVisible.value = true
}

const openEditDialog = (row) => {
  isEdit.value = true
  form.value = {
    ...row,
    releaseDate: row.releaseDate ? row.releaseDate.substring(0, 19) : '2026-04-20T00:00:00'
  }
  dialogVisible.value = true
}

const saveMovie = async () => {
  if (!form.value.name) {
    ElMessage.warning('请输入电影名称')
    return
  }

  if (isEdit.value) {
    await updateMovie(form.value.id, form.value)
    ElMessage.success('电影修改成功')
  } else {
    await createMovie(form.value)
    ElMessage.success('电影新增成功')
  }

  dialogVisible.value = false
  loadMovies()
}

const removeMovie = async (id) => {
  await ElMessageBox.confirm('确定要删除这部电影吗？', '删除确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  })

  await deleteMovie(id)
  ElMessage.success('删除成功')
  loadMovies()
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
