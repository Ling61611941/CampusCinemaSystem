<template>
  <div class="page">
    <div class="top">
      <div>
        <h2>后台订单管理</h2>
        <p class="page-desc">查看购票订单，并进行支付、取消和删除操作</p>
      </div>

      <div class="nav-buttons">
        <el-button @click="$router.push('/admin/movies')">电影管理</el-button>
        <el-button @click="$router.push('/admin/halls')">影厅管理</el-button>
        <el-button @click="$router.push('/admin/schedules')">排片管理</el-button>
        <el-button type="primary" @click="$router.push('/')">返回前台</el-button>
      </div>
    </div>

    <el-card>
      <template #header>
        <div class="card-header">
          <span>订单列表</span>
          <el-button type="primary" @click="loadOrders">刷新订单</el-button>
        </div>
      </template>

      <el-table
        v-if="orders.length > 0"
        :data="orders"
        border
        style="width: 100%"
      >
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

        <el-table-column label="金额" width="100">
          <template #default="scope">
            <span class="money">￥{{ scope.row.totalAmount }}</span>
          </template>
        </el-table-column>

        <el-table-column label="状态" width="110">
          <template #default="scope">
            <el-tag :type="getOrderStatusType(scope.row.status)">
              {{ scope.row.status }}
            </el-tag>
          </template>
        </el-table-column>

        <el-table-column label="创建时间" width="180">
          <template #default="scope">
            {{ formatDateTime(scope.row.createdAt) }}
          </template>
        </el-table-column>

        <el-table-column label="支付时间" width="180">
          <template #default="scope">
            {{ scope.row.paidAt ? formatDateTime(scope.row.paidAt) : '未支付' }}
          </template>
        </el-table-column>

        <el-table-column label="操作" width="240" fixed="right">
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

      <el-empty
        v-else
        description="暂无订单，请先在前台选座购票创建订单"
      />
    </el-card>
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

const getOrderStatusType = (status) => {
  if (status === '待支付') return 'warning'
  if (status === '已支付') return 'success'
  if (status === '已取消') return 'info'
  return 'primary'
}

const formatDateTime = (value) => {
  if (!value) return '暂无'
  return value.toString().replace('T', ' ').substring(0, 16)
}

onMounted(() => {
  loadOrders()
})
</script>

<style scoped>
.page-desc {
  margin: 8px 0 0;
  color: #64748b;
  font-size: 14px;
}

.nav-buttons {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: #1e3a8a;
  font-weight: 700;
}

.money {
  color: #dc2626;
  font-weight: 700;
}
</style>