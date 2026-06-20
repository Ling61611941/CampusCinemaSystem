import axios from 'axios'

const request = axios.create({
  baseURL: 'https://fuzzy-engine-v67qj7rpvwg636qqv-5080.app.github.dev/api',
  timeout: 10000
})

export default request
