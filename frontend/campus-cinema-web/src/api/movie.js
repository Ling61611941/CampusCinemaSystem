import request from './request'

export function getMovies(params) {
  return request.get('/Movies', { params })
}

export function getMovie(id) {
  return request.get(`/Movies/${id}`)
}

export function createMovie(data) {
  return request.post('/Movies', data)
}

export function updateMovie(id, data) {
  return request.put(`/Movies/${id}`, data)
}

export function deleteMovie(id) {
  return request.delete(`/Movies/${id}`)
}
