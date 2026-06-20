import request from './request'

export function getOrders() {
  return request.get('/Orders')
}

export function getOrder(id) {
  return request.get(`/Orders/${id}`)
}

export function createOrder(data) {
  return request.post('/Orders', data)
}

export function payOrder(id) {
  return request.put(`/Orders/${id}/pay`)
}

export function cancelOrder(id) {
  return request.put(`/Orders/${id}/cancel`)
}

export function deleteOrder(id) {
  return request.delete(`/Orders/${id}`)
}
