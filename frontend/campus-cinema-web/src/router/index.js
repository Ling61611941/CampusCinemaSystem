import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import MovieDetail from '../views/MovieDetail.vue'
import AdminMovies from '../views/admin/AdminMovies.vue'
import AdminHalls from '../views/admin/AdminHalls.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/movies/:id', component: MovieDetail },
  { path: '/admin/movies', component: AdminMovies },
  { path: '/admin/halls', component: AdminHalls }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
