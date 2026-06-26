# 校园影院购票与排片管理系统

## 项目简介

校园影院购票与排片管理系统是一套基于 ASP.NET Core Web API 和 Vue3 开发的前后端分离系统，实现了校园影院电影管理、影厅管理、排片管理、在线购票、订单管理及模拟支付等功能。

---

## 技术栈

### 后端

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger

### 前端

- Vue3
- Vite
- Axios
- Element Plus

---

## 系统功能

### 用户端

- 浏览电影
- 查看电影详情
- 查看排片
- 在线选座
- 创建订单
- 模拟支付
- 取消订单

### 管理端

- 电影管理（增删改查）
- 影厅管理（增删改查）
- 排片管理（增删改查）
- 订单管理

---

## 项目结构

```
backend/
    CampusCinema.Api/

frontend/
    campus-cinema-web/

docs/
    系统设计文档.md
    用例分析.md
    技术指导与系统架构说明.md
```

---

## 启动方式

### 后端

```bash
cd backend/CampusCinema.Api
dotnet run
```

Swagger：

```
http://localhost:5186/swagger
```

---

### 前端

```bash
cd frontend/campus-cinema-web
npm install
npm run dev
```

默认访问：

```
http://localhost:5173
```

---

## 项目特色

- 前后端分离
- RESTful API
- Swagger接口测试
- 在线购票流程
- 排片冲突检测
- 模拟支付
- 数据持久化(SQLite)

---

## 作者

Ling

2026