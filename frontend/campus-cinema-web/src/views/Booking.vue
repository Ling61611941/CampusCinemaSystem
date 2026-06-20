<template>
  <div class="page" v-if="schedule">
    <el-button @click="$router.back()">返回</el-button>

    <el-card class="info-card">
      <h2>选座购票</h2>
      <p>电影：{{ schedule.movie?.name }}</p>
      <p>影厅：{{ schedule.hall?.name }}</p>
      <p>时间：{{ schedule.startTime }} 至 {{ schedule.endTime }}</p>
      <p>票价：{{ schedule.price }} 元 / 张</p>
    </el-card>

    <el-card class="seat-card">
      <template #header>
        <strong>请选择座位</strong>
      </template>

      <div class="screen">银幕</div>

      <div class="seat-area">
        <div v-for="row in rows" :key="row" class="seat-row">
          <span class="row-label">{{ row }}</span>

          <el-button
            v-for="col in cols"
            :key="`${row}${col}`"
            size="small"
            :type="selectedSeats.includes(`${row}${col}`) ? 'primary' : 'default'"
            @click="toggleSeat(`${row}${col}`)"
          >
            {{ row }}{{ col }}
          </el-button>
        </div>
      </div>

      <div class="summary">
        <p>已选座位：{{ selectedSeats.length ? selectedSeats.join('、') : '暂无' }}</p>
        <p>合计金额：{{ totalAmount }} 元</p>

        <el-button type="primary" @click="submitOrder">
          提交订单
        </el-button>
      </div>
    </el-card>

    <el-dialog v-model="orderDialogVisible" title="订单创建成功" width="500px">
      <p>订单编号：{{ order?.orderNo }}</p>
      <p>座位：{{ order?.seatCodes }}</p>
      <p>票数：{{ order?.ticketCount }}</p>
      <p>金额：{{ order?.totalAmount }} 元</p>
      <p>状态：{{ order?.status }}</p>

      <template #footer>
        <el-button @click="orderDialogVisible = false">稍后支付</el-button>
        <el-button type="primary" @click="payCurrentOrder">模拟支付</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getSchedule } from '../api/schedule'
import { createOrder, payOrder } from '../api/order'
import { ElMessage } from 'element-plus'

const route = useRoute()
const router = useRouter()

const schedule = ref(null)
const selectedSeats = ref([])
const order = ref(null)
const orderDialogVisible = ref(false)

const rows = ['A', 'B', 'C', 'D', 'E']
const cols = [1, 2, 3, 4, 5, 6, 7, 8]

const totalAmount = computed(() => {
  if (!schedule.value) return 0
  return selectedSeats.value.length * schedule.value.price
})

const toggleSeat = (seatCode) => {
  if (selectedSeats.value.includes(seatCode)) {
    selectedSeats.value = selectedSeats.value.filter(item => item !== seatCode)
  } else {
    selectedSeats.value.push(seatCode)
  }
}

const submitOrder = async () => {
  if (selectedSeats.value.length === 0) {
    ElMessage.warning('请至少选择一个座位')
    return
  }

  const res = await createOrder({
    scheduleId: Number(route.params.scheduleId),
    seatCodes: selectedSeats.value
  })

  order.value = res.data
  orderDialogVisible.value = true
  ElMessage.success('订单创建成功')
}

const payCurrentOrder = async () => {
  await payOrder(order.value.id)
  ElMessage.success('支付成功')
  orderDialogVisible.value = false
  router.push('/')
}

onMounted(async () => {
  const res = await getSchedule(route.params.scheduleId)
  schedule.value = res.data
})
</script>

<style scoped>
.page {
  padding: 30px;
  background: #f5f7fa;
  min-height: 100vh;
}

.info-card {
  margin-top: 20px;
  margin-bottom: 20px;
}

.seat-card {
  margin-top: 20px;
}

.screen {
  width: 360px;
  margin: 0 auto 25px;
  text-align: center;
  background: #dcdfe6;
  padding: 10px;
  border-radius: 20px;
  color: #606266;
}

.seat-area {
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: center;
}

.seat-row {
  display: flex;
  gap: 8px;
  align-items: center;
}

.row-label {
  width: 25px;
  font-weight: bold;
}

.summary {
  margin-top: 25px;
  text-align: center;
}
</style>
