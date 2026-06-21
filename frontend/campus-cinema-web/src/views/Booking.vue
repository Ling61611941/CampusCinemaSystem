<template>
  <div class="booking-page" v-if="schedule">
    <div class="booking-container">
      <el-button class="back-button" @click="$router.back()">
        返回上一页
      </el-button>

      <div class="booking-header">
        <div>
          <h1>选座购票</h1>
          <p>请选择心仪座位，系统将自动生成购票订单</p>
        </div>

        <el-tag size="large" type="success">
          {{ schedule.status }}
        </el-tag>
      </div>

      <div class="booking-layout">
        <el-card class="main-card">
          <template #header>
            <div class="card-title">
              <span>🎬 场次信息</span>
            </div>
          </template>

          <div class="schedule-info">
            <div class="info-item">
              <span>电影名称</span>
              <strong>{{ schedule.movie?.name }}</strong>
            </div>

            <div class="info-item">
              <span>放映影厅</span>
              <strong>{{ schedule.hall?.name }}</strong>
            </div>

            <div class="info-item">
              <span>开始时间</span>
              <strong>{{ formatDateTime(schedule.startTime) }}</strong>
            </div>

            <div class="info-item">
              <span>结束时间</span>
              <strong>{{ formatDateTime(schedule.endTime) }}</strong>
            </div>

            <div class="info-item">
              <span>单张票价</span>
              <strong class="price">￥{{ schedule.price }}</strong>
            </div>
          </div>

          <div class="seat-section">
            <div class="screen">银幕 SCREEN</div>

            <div class="seat-legend">
              <span><i class="seat-demo available"></i>可选</span>
              <span><i class="seat-demo selected"></i>已选</span>
            </div>

            <div class="seat-area">
              <div v-for="row in rows" :key="row" class="seat-row">
                <span class="row-label">{{ row }}</span>

                <button
                  v-for="col in cols"
                  :key="`${row}${col}`"
                  class="seat-button"
                  :class="{ active: selectedSeats.includes(`${row}${col}`) }"
                  @click="toggleSeat(`${row}${col}`)"
                >
                  {{ row }}{{ col }}
                </button>
              </div>
            </div>
          </div>
        </el-card>

        <el-card class="summary-card">
          <template #header>
            <div class="card-title">
              <span>订单结算</span>
            </div>
          </template>

          <div class="summary-content">
            <div class="summary-line">
              <span>已选座位</span>
              <strong>
                {{ selectedSeats.length ? selectedSeats.join('、') : '暂无' }}
              </strong>
            </div>

            <div class="summary-line">
              <span>票数</span>
              <strong>{{ selectedSeats.length }} 张</strong>
            </div>

            <div class="summary-line">
              <span>单价</span>
              <strong>￥{{ schedule.price }}</strong>
            </div>

            <div class="summary-total">
              <span>合计金额</span>
              <strong>￥{{ totalAmount }}</strong>
            </div>

            <el-button
              type="primary"
              size="large"
              class="submit-button"
              @click="submitOrder"
            >
              提交订单
            </el-button>

            <p class="tip">
              提交后将生成待支付订单，可选择模拟支付或稍后到后台订单管理处理。
            </p>
          </div>
        </el-card>
      </div>

      <el-dialog v-model="orderDialogVisible" title="订单创建成功" width="520px">
        <div class="order-result">
          <p><span>订单编号：</span>{{ order?.orderNo }}</p>
          <p><span>座位：</span>{{ order?.seatCodes }}</p>
          <p><span>票数：</span>{{ order?.ticketCount }}</p>
          <p><span>金额：</span>￥{{ order?.totalAmount }}</p>
          <p><span>状态：</span>{{ order?.status }}</p>
        </div>

        <template #footer>
          <el-button @click="orderDialogVisible = false">
            稍后支付
          </el-button>

          <el-button type="primary" @click="payCurrentOrder">
            模拟支付
          </el-button>
        </template>
      </el-dialog>
    </div>
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

const formatDateTime = (value) => {
  if (!value) return '暂无'
  return value.toString().replace('T', ' ').substring(0, 16)
}

onMounted(async () => {
  const res = await getSchedule(route.params.scheduleId)
  schedule.value = res.data
})
</script>

<style scoped>
.booking-page {
  min-height: 100vh;
  background: #f3f6fb;
  padding: 32px 24px 48px;
}

.booking-container {
  max-width: 1200px;
  margin: 0 auto;
}

.back-button {
  margin-bottom: 18px;
}

.booking-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.booking-header h1 {
  margin: 0;
  color: #1e3a8a;
  font-size: 34px;
}

.booking-header p {
  margin: 10px 0 0;
  color: #64748b;
}

.booking-layout {
  display: grid;
  grid-template-columns: 1fr 320px;
  gap: 24px;
}

.card-title {
  font-weight: 700;
  color: #1e3a8a;
}

.schedule-info {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 14px;
  margin-bottom: 30px;
}

.info-item {
  padding: 14px 16px;
  border-radius: 12px;
  background: #f8fafc;
  border: 1px solid #e5e7eb;
}

.info-item span {
  display: block;
  margin-bottom: 8px;
  color: #64748b;
  font-size: 13px;
}

.info-item strong {
  color: #1f2937;
  font-size: 15px;
}

.price {
  color: #dc2626 !important;
  font-size: 18px !important;
}

.seat-section {
  padding: 12px 0 8px;
}

.screen {
  width: 420px;
  margin: 0 auto 20px;
  text-align: center;
  padding: 12px;
  border-radius: 999px;
  background: linear-gradient(135deg, #dbeafe, #bfdbfe);
  color: #1e3a8a;
  font-weight: 800;
  border: 1px solid #93c5fd;
}

.seat-legend {
  display: flex;
  justify-content: center;
  gap: 22px;
  margin-bottom: 22px;
  color: #64748b;
  font-size: 14px;
}

.seat-demo {
  display: inline-block;
  width: 18px;
  height: 18px;
  margin-right: 6px;
  border-radius: 5px;
  vertical-align: middle;
}

.seat-demo.available {
  background: #ffffff;
  border: 1px solid #bfdbfe;
}

.seat-demo.selected {
  background: #2563eb;
  border: 1px solid #2563eb;
}

.seat-area {
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: center;
}

.seat-row {
  display: flex;
  align-items: center;
  gap: 10px;
}

.row-label {
  width: 28px;
  color: #1e3a8a;
  font-weight: 800;
}

.seat-button {
  width: 56px;
  height: 38px;
  border-radius: 10px;
  border: 1px solid #bfdbfe;
  background: #ffffff;
  color: #2563eb;
  cursor: pointer;
  font-weight: 700;
  transition: all 0.18s ease;
}

.seat-button:hover {
  background: #eff6ff;
  border-color: #60a5fa;
}

.seat-button.active {
  background: #2563eb;
  color: #ffffff;
  border-color: #2563eb;
  box-shadow: 0 6px 16px rgba(37, 99, 235, 0.25);
}

.summary-card {
  position: sticky;
  top: 24px;
  align-self: start;
}

.summary-content {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.summary-line {
  display: flex;
  justify-content: space-between;
  gap: 18px;
  padding-bottom: 14px;
  border-bottom: 1px solid #e5e7eb;
}

.summary-line span {
  color: #64748b;
}

.summary-line strong {
  color: #1f2937;
  text-align: right;
}

.summary-total {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 0;
}

.summary-total span {
  color: #1e3a8a;
  font-weight: 700;
}

.summary-total strong {
  color: #dc2626;
  font-size: 28px;
}

.submit-button {
  width: 100%;
}

.tip {
  margin: 0;
  color: #94a3b8;
  font-size: 13px;
  line-height: 1.7;
}

.order-result {
  padding: 6px 0;
}

.order-result p {
  margin: 12px 0;
  color: #374151;
}

.order-result span {
  color: #64748b;
  display: inline-block;
  width: 88px;
}

@media (max-width: 960px) {
  .booking-layout {
    grid-template-columns: 1fr;
  }

  .schedule-info {
    grid-template-columns: 1fr;
  }

  .summary-card {
    position: static;
  }

  .screen {
    width: 100%;
  }
}
</style>