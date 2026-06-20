<template>
  <div class="page">
    <div class="top">
      <h2>后台影厅管理</h2>
      <div>
        <el-button type="primary" @click="openAddDialog">新增影厅</el-button>
        <el-button @click="$router.push('/admin/movies')">电影管理</el-button>
        <el-button @click="$router.push('/')">返回前台</el-button>
      </div>
    </div>

    <el-table :data="halls" border style="margin-top: 20px">
      <el-table-column prop="id" label="编号" width="80" />
      <el-table-column prop="name" label="影厅名称" />
      <el-table-column prop="rowCount" label="座位行数" width="120" />
      <el-table-column prop="columnCount" label="座位列数" width="120" />
      <el-table-column prop="type" label="影厅类型" width="140" />
      <el-table-column prop="status" label="状态" width="120" />

      <el-table-column label="操作" width="180">
        <template #default="scope">
          <el-button size="small" @click="openEditDialog(scope.row)">编辑</el-button>
          <el-button size="small" type="danger" @click="removeHall(scope.row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑影厅' : '新增影厅'"
      width="520px"
    >
      <el-form label-width="90px">
        <el-form-item label="影厅名称">
          <el-input v-model="form.name" placeholder="请输入影厅名称" />
        </el-form-item>

        <el-form-item label="座位行数">
          <el-input-number v-model="form.rowCount" :min="1" />
        </el-form-item>

        <el-form-item label="座位列数">
          <el-input-number v-model="form.columnCount" :min="1" />
        </el-form-item>

        <el-form-item label="影厅类型">
          <el-select v-model="form.type" placeholder="请选择影厅类型">
            <el-option label="普通厅" value="普通厅" />
            <el-option label="报告厅" value="报告厅" />
            <el-option label="VIP厅" value="VIP厅" />
            <el-option label="IMAX厅" value="IMAX厅" />
          </el-select>
        </el-form-item>

        <el-form-item label="状态">
          <el-select v-model="form.status" placeholder="请选择状态">
            <el-option label="启用" value="启用" />
            <el-option label="停用" value="停用" />
            <el-option label="维修中" value="维修中" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="saveHall">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getHalls, createHall, updateHall, deleteHall } from '../../api/hall'
import { ElMessage, ElMessageBox } from 'element-plus'

const halls = ref([])
const dialogVisible = ref(false)
const isEdit = ref(false)

const emptyForm = () => ({
  id: 0,
  name: '',
  rowCount: 5,
  columnCount: 8,
  type: '普通厅',
  status: '启用'
})

const form = ref(emptyForm())

const loadHalls = async () => {
  const res = await getHalls()
  halls.value = res.data
}

const openAddDialog = () => {
  isEdit.value = false
  form.value = emptyForm()
  dialogVisible.value = true
}

const openEditDialog = (row) => {
  isEdit.value = true
  form.value = { ...row }
  dialogVisible.value = true
}

const saveHall = async () => {
  if (!form.value.name) {
    ElMessage.warning('请输入影厅名称')
    return
  }

  if (isEdit.value) {
    await updateHall(form.value.id, form.value)
    ElMessage.success('影厅修改成功')
  } else {
    await createHall(form.value)
    ElMessage.success('影厅新增成功')
  }

  dialogVisible.value = false
  loadHalls()
}

const removeHall = async (id) => {
  await ElMessageBox.confirm('确定要删除这个影厅吗？', '删除确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  })

  await deleteHall(id)
  ElMessage.success('删除成功')
  loadHalls()
}

onMounted(() => {
  loadHalls()
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
