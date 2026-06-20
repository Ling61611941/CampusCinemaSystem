import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import MovieDetail from '../views/MovieDetail.vue'
import AdminMovies from '../views/admin/AdminMovies.vue'
import AdminHalls from '../views/admin/AdminHalls.vue'
import AdminSchedules from '../views/admin/AdminSchedules.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/movies/:id', component: MovieDetail },
  { path: '/admin/movies', component: AdminMovies },
  { path: '/admin/halls', component: AdminHalls },
  { path: '/admin/schedules', component: AdminSchedules }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
