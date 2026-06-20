<template>
  <div class="page">
    <div class="top">
      <h2>后台排片管理</h2>
      <div>
      <el-button type="primary" @click="openAddDialog">新增排片</el-button>
<el-button @click="$router.push('/admin/movies')">电影管理</el-button>
<el-button @click="$router.push('/admin/halls')">影厅管理</el-button>
<el-button @click="$router.push('/admin/orders')">订单管理</el-button>
<el-button @click="$router.push('/')">返回前台</el-button>
      </div>
    </div>

    <el-table :data="schedules" border style="margin-top: 20px">
      <el-table-column prop="id" label="编号" width="80" />

      <el-table-column label="电影名称">
        <template #default="scope">
          {{ scope.row.movie?.name || '未知电影' }}
        </template>
      </el-table-column>

      <el-table-column label="影厅">
        <template #default="scope">
          {{ scope.row.hall?.name || '未知影厅' }}
        </template>
      </el-table-column>

      <el-table-column prop="startTime" label="开始时间" width="190" />
      <el-table-column prop="endTime" label="结束时间" width="190" />
      <el-table-column prop="price" label="票价" width="100" />
      <el-table-column prop="status" label="状态" width="100" />

      <el-table-column label="操作" width="180">
        <template #default="scope">
          <el-button size="small" @click="openEditDialog(scope.row)">编辑</el-button>
          <el-button size="small" type="danger" @click="removeSchedule(scope.row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑排片' : '新增排片'"
      width="650px"
    >
      <el-form label-width="90px">
        <el-form-item label="电影">
          <el-select v-model="form.movieId" placeholder="请选择电影" style="width: 100%">
            <el-option
              v-for="movie in movies"
              :key="movie.id"
              :label="movie.name"
              :value="movie.id"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="影厅">
          <el-select v-model="form.hallId" placeholder="请选择影厅" style="width: 100%">
            <el-option
              v-for="hall in halls"
              :key="hall.id"
              :label="hall.name"
              :value="hall.id"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="开始时间">
          <el-date-picker
            v-model="form.startTime"
            type="datetime"
            value-format="YYYY-MM-DDTHH:mm:ss"
            placeholder="请选择开始时间"
            style="width: 100%"
          />
        </el-form-item>

        <el-form-item label="结束时间">
          <el-date-picker
            v-model="form.endTime"
            type="datetime"
            value-format="YYYY-MM-DDTHH:mm:ss"
            placeholder="请选择结束时间"
            style="width: 100%"
          />
        </el-form-item>

        <el-form-item label="票价">
          <el-input-number v-model="form.price" :min="1" :precision="2" :step="5" />
          <span style="margin-left: 10px">元</span>
        </el-form-item>

        <el-form-item label="状态">
          <el-select v-model="form.status">
            <el-option label="正常" value="正常" />
            <el-option label="取消" value="取消" />
            <el-option label="已结束" value="已结束" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="saveSchedule">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getSchedules, createSchedule, updateSchedule, deleteSchedule } from '../../api/schedule'
import { getMovies } from '../../api/movie'
import { getHalls } from '../../api/hall'
import { ElMessage, ElMessageBox } from 'element-plus'

const schedules = ref([])
const movies = ref([])
const halls = ref([])
const dialogVisible = ref(false)
const isEdit = ref(false)

const emptyForm = () => ({
  id: 0,
  movieId: null,
  hallId: null,
  startTime: '2026-04-20T19:00:00',
  endTime: '2026-04-20T21:00:00',
  price: 35,
  status: '正常'
})

const form = ref(emptyForm())

const loadData = async () => {
  const scheduleRes = await getSchedules()
  const movieRes = await getMovies()
  const hallRes = await getHalls()

  schedules.value = scheduleRes.data
  movies.value = movieRes.data
  halls.value = hallRes.data
}

const openAddDialog = () => {
  isEdit.value = false
  form.value = emptyForm()

  if (movies.value.length > 0) {
    form.value.movieId = movies.value[0].id
  }

  if (halls.value.length > 0) {
    form.value.hallId = halls.value[0].id
  }

  dialogVisible.value = true
}

const openEditDialog = (row) => {
  isEdit.value = true
  form.value = {
    id: row.id,
    movieId: row.movieId,
    hallId: row.hallId,
    startTime: row.startTime ? row.startTime.substring(0, 19) : '',
    endTime: row.endTime ? row.endTime.substring(0, 19) : '',
    price: row.price,
    status: row.status
  }
  dialogVisible.value = true
}

const saveSchedule = async () => {
  if (!form.value.movieId) {
    ElMessage.warning('请选择电影')
    return
  }

  if (!form.value.hallId) {
    ElMessage.warning('请选择影厅')
    return
  }

  if (!form.value.startTime || !form.value.endTime) {
    ElMessage.warning('请选择开始时间和结束时间')
    return
  }

  try {
    if (isEdit.value) {
      await updateSchedule(form.value.id, form.value)
      ElMessage.success('排片修改成功')
    } else {
      await createSchedule(form.value)
      ElMessage.success('排片新增成功')
    }

    dialogVisible.value = false
    loadData()
  } catch (error) {
    ElMessage.error(error.response?.data || '保存失败，请检查排片时间是否冲突')
  }
}

const removeSchedule = async (id) => {
  await ElMessageBox.confirm('确定要删除这个排片吗？', '删除确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  })

  await deleteSchedule(id)
  ElMessage.success('删除成功')
  loadData()
}

onMounted(() => {
  loadData()
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
