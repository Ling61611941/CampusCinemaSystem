import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import MovieDetail from '../views/MovieDetail.vue'
import AdminMovies from '../views/admin/AdminMovies.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/movies/:id', component: MovieDetail },
  { path: '/admin/movies', component: AdminMovies }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
