import request from './request'

export function getHalls(params) {
  return request.get('/Halls', { params })
}

export function getHall(id) {
  return request.get(`/Halls/${id}`)
}

export function createHall(data) {
  return request.post('/Halls', data)
}

export function updateHall(id, data) {
  return request.put(`/Halls/${id}`, data)
}

export function deleteHall(id) {
  return request.delete(`/Halls/${id}`)
}
