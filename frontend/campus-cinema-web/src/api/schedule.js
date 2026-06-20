import request from './request'

export function getSchedules() {
  return request.get('/Schedules')
}

export function getSchedule(id) {
  return request.get(`/Schedules/${id}`)
}

export function createSchedule(data) {
  return request.post('/Schedules', data)
}

export function updateSchedule(id, data) {
  return request.put(`/Schedules/${id}`, data)
}

export function deleteSchedule(id) {
  return request.delete(`/Schedules/${id}`)
}
