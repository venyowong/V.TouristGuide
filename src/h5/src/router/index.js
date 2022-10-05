import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: "/modifyLocation",
    name: "ModifyLocation",
    component: () => import("../views/ModifyLocation.vue")
  },
  {
    path: "/map",
    name: "Map",
    component: () => import("../views/MapView.vue")
  },
  {
    path: "/placeDetail",
    name: "PlaceDetail",
    component: () => import("../views/PlaceDetail.vue")
  },
  {
    path: "/user",
    name: "User",
    component: () => import("../views/UserView.vue")
  },
  {
    path: "/login",
    name: "Login",
    component: () => import("../views/LoginView.vue")
  },
  {
    path: "/comment",
    name: "Comment",
    component: () => import("../views/CommentView.vue")
  },
  {
    path: "/setting",
    name: "Setting",
    component: () => import("../views/SettingView.vue")
  },
  {
    path: "/setPassword",
    name: "SetPassword",
    component: () => import("../views/SetPassword.vue")
  },
  {
    path: "/changeMobile",
    name: "ChangeMobile",
    component: () => import("../views/ChangeMobile.vue")
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
