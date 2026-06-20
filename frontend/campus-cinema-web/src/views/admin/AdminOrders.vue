<template>
  <div class="page">
    <div class="top">
      <h2>后台订单管理</h2>
      <div>
        <el-button @click="$router.push('/admin/movies')">电影管理</el-button>
        <el-button @click="$router.push('/admin/halls')">影厅管理</el-button>
        <el-button @click="$router.push('/admin/schedules')">排片管理</el-button>
        <el-button @click="$router.push('/')">返回前台</el-button>
      </div>
    </div>

    <el-table :data="orders" border style="margin-top: 20px">
      <el-table-column prop="id" label="编号" width="70" />
      <el-table-column prop="orderNo" label="订单编号" width="190" />

      <el-table-column label="电影名称" width="160">
        <template #default="scope">
          {{ scope.row.schedule?.movie?.name || '未知电影' }}
        </template>
      </el-table-column>

      <el-table-column label="影厅" width="130">
        <template #default="scope">
          {{ scope.row.schedule?.hall?.name || '未知影厅' }}
        </template>
      </el-table-column>

      <el-table-column prop="seatCodes" label="座位" width="120" />
      <el-table-column prop="ticketCount" label="票数" width="80" />
      <el-table-column prop="totalAmount" label="金额" width="90" />
      <el-table-column prop="status" label="状态" width="100" />
      <el-table-column prop="createdAt" label="创建时间" width="190" />
      <el-table-column prop="paidAt" label="支付时间" width="190" />

      <el-table-column label="操作" width="240">
        <template #default="scope">
          <el-button
            size="small"
            type="success"
            :disabled="scope.row.status !== '待支付'"
            @click="pay(scope.row.id)"
          >
            支付
          </el-button>

          <el-button
            size="small"
            type="warning"
            :disabled="scope.row.status !== '待支付'"
            @click="cancel(scope.row.id)"
          >
            取消
          </el-button>

          <el-button
            size="small"
            type="danger"
            @click="remove(scope.row.id)"
          >
            删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-empty v-if="orders.length === 0" description="暂无订单" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getOrders, payOrder, cancelOrder, deleteOrder } from '../../api/order'
import { ElMessage, ElMessageBox } from 'element-plus'

const orders = ref([])

const loadOrders = async () => {
  const res = await getOrders()
  orders.value = res.data
}

const pay = async (id) => {
  await payOrder(id)
  ElMessage.success('支付成功')
  loadOrders()
}

const cancel = async (id) => {
  await ElMessageBox.confirm('确定要取消这个订单吗？', '取消确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  })

  await cancelOrder(id)
  ElMessage.success('订单已取消')
  loadOrders()
}

const remove = async (id) => {
  await ElMessageBox.confirm('确定要删除这个订单吗？', '删除确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  })

  await deleteOrder(id)
  ElMessage.success('删除成功')
  loadOrders()
}

onMounted(() => {
  loadOrders()
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
