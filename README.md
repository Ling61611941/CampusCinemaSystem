# 校园影院购票与排片管理系统

## 一、项目简介

本项目是《信息系统开发》课程大作业，系统名称为“校园影院购票与排片管理系统”。

系统面向校园影院日常管理和学生观影购票场景，主要实现电影信息管理、影厅管理、排片管理、前台电影浏览、电影详情查看、选座购票、订单生成、模拟支付和后台订单管理等功能。

本系统采用前后端分离架构开发：

- 前端：Vue 3 + Vite + Element Plus
- 后端：ASP.NET Core Web API 9.0
- 数据库：SQL Server
- ORM：Entity Framework Core
- 接口文档：Swagger
- 代码托管：GitHub

---

## 二、项目技术栈

### 1. 前端技术

- Vue 3
- Vue Router
- Axios
- Element Plus
- Vite

### 2. 后端技术

- ASP.NET Core Web API 9.0
- Entity Framework Core
- SQL Server
- Swagger

### 3. 开发环境

- GitHub Codespaces
- Visual Studio Code
- Docker
- Git / GitHub

---

## 三、系统主要功能

### 1. 前台功能

#### 电影列表展示

用户进入首页后，可以查看当前影院中的电影信息，包括电影名称、类型、导演、片长、评分和上映状态等。

#### 电影详情查看

用户可以点击电影卡片中的“查看详情”按钮，进入电影详情页面，查看电影的详细介绍和对应排片场次。

#### 场次查看

电影详情页中展示该电影的排片信息，包括影厅、开始时间、结束时间、票价和状态。

#### 选座购票

用户可以点击某一场次的“选座购票”按钮，进入选座页面，选择座位后提交订单。

#### 模拟支付

订单创建后，用户可以选择“模拟支付”，系统会将订单状态从“待支付”更新为“已支付”。

---

### 2. 后台管理功能

#### 电影管理

管理员可以进行电影信息的新增、编辑、删除和查看。

主要字段包括：

- 电影名称
- 类型
- 导演
- 主演
- 简介
- 海报地址
- 片长
- 上映日期
- 评分
- 状态

#### 影厅管理

管理员可以维护影院影厅信息。

主要字段包括：

- 影厅名称
- 行数
- 列数
- 影厅类型
- 状态

#### 排片管理

管理员可以为电影安排放映场次。

主要字段包括：

- 电影
- 影厅
- 开始时间
- 结束时间
- 票价
- 状态

系统会校验同一影厅在同一时间段内是否存在排片冲突。

#### 订单管理

管理员可以查看用户创建的订单，并对订单进行支付、取消和删除操作。

主要字段包括：

- 订单编号
- 电影名称
- 影厅
- 座位
- 票数
- 总金额
- 订单状态
- 创建时间
- 支付时间

---

## 四、系统业务流程

系统核心业务流程如下：

```text
管理员维护电影信息
        ↓
管理员维护影厅信息
        ↓
管理员创建电影排片
        ↓
用户浏览电影列表
        ↓
用户查看电影详情和场次
        ↓
用户选择场次并选座
        ↓
系统生成订单
        ↓
用户模拟支付
        ↓
后台查看和管理订单
```

---

## 五、项目目录结构

```text
CampusCinemaSystem
├── backend
│   └── CampusCinema.Api
│       ├── Controllers
│       │   ├── MoviesController.cs
│       │   ├── HallsController.cs
│       │   ├── SchedulesController.cs
│       │   └── OrdersController.cs
│       ├── Data
│       │   └── AppDbContext.cs
│       ├── Models
│       │   ├── Movie.cs
│       │   ├── Hall.cs
│       │   ├── Schedule.cs
│       │   └── Order.cs
│       ├── Migrations
│       ├── Program.cs
│       └── appsettings.json
│
├── frontend
│   └── campus-cinema-web
│       ├── src
│       │   ├── api
│       │   │   ├── request.js
│       │   │   ├── movie.js
│       │   │   ├── hall.js
│       │   │   ├── schedule.js
│       │   │   └── order.js
│       │   ├── router
│       │   │   └── index.js
│       │   ├── views
│       │   │   ├── Home.vue
│       │   │   ├── MovieDetail.vue
│       │   │   ├── Booking.vue
│       │   │   └── admin
│       │   │       ├── AdminMovies.vue
│       │   │       ├── AdminHalls.vue
│       │   │       ├── AdminSchedules.vue
│       │   │       └── AdminOrders.vue
│       │   ├── App.vue
│       │   └── main.js
│       └── package.json
│
├── docs
├── video
├── README.md
└── .gitignore
```

---

## 六、数据库设计

### 1. Movie 电影表

| 字段名 | 类型 | 说明 |
|---|---|---|
| Id | int | 电影编号 |
| Name | string | 电影名称 |
| Category | string | 电影类型 |
| Director | string | 导演 |
| Actors | string | 主演 |
| Description | string | 电影简介 |
| PosterUrl | string | 海报地址 |
| Duration | int | 片长 |
| ReleaseDate | DateTime | 上映日期 |
| Score | decimal | 评分 |
| Status | string | 状态 |

### 2. Hall 影厅表

| 字段名 | 类型 | 说明 |
|---|---|---|
| Id | int | 影厅编号 |
| Name | string | 影厅名称 |
| RowCount | int | 座位行数 |
| ColumnCount | int | 座位列数 |
| Type | string | 影厅类型 |
| Status | string | 状态 |

### 3. Schedule 排片表

| 字段名 | 类型 | 说明 |
|---|---|---|
| Id | int | 排片编号 |
| MovieId | int | 电影编号 |
| HallId | int | 影厅编号 |
| StartTime | DateTime | 开始时间 |
| EndTime | DateTime | 结束时间 |
| Price | decimal | 票价 |
| Status | string | 状态 |

### 4. Order 订单表

| 字段名 | 类型 | 说明 |
|---|---|---|
| Id | int | 订单编号 |
| OrderNo | string | 订单号 |
| ScheduleId | int | 排片编号 |
| UserName | string | 用户名 |
| SeatCodes | string | 座位编号 |
| TicketCount | int | 票数 |
| TotalAmount | decimal | 总金额 |
| Status | string | 订单状态 |
| CreatedAt | DateTime | 创建时间 |
| PaidAt | DateTime? | 支付时间 |

---

## 七、后端接口说明

### 电影接口

| 请求方式 | 地址 | 说明 |
|---|---|---|
| GET | /api/Movies | 获取电影列表 |
| GET | /api/Movies/{id} | 获取电影详情 |
| POST | /api/Movies | 新增电影 |
| PUT | /api/Movies/{id} | 修改电影 |
| DELETE | /api/Movies/{id} | 删除电影 |

### 影厅接口

| 请求方式 | 地址 | 说明 |
|---|---|---|
| GET | /api/Halls | 获取影厅列表 |
| GET | /api/Halls/{id} | 获取影厅详情 |
| POST | /api/Halls | 新增影厅 |
| PUT | /api/Halls/{id} | 修改影厅 |
| DELETE | /api/Halls/{id} | 删除影厅 |

### 排片接口

| 请求方式 | 地址 | 说明 |
|---|---|---|
| GET | /api/Schedules | 获取排片列表 |
| GET | /api/Schedules/{id} | 获取排片详情 |
| GET | /api/Schedules/movie/{movieId} | 根据电影获取排片 |
| POST | /api/Schedules | 新增排片 |
| PUT | /api/Schedules/{id} | 修改排片 |
| DELETE | /api/Schedules/{id} | 删除排片 |

### 订单接口

| 请求方式 | 地址 | 说明 |
|---|---|---|
| GET | /api/Orders | 获取订单列表 |
| GET | /api/Orders/{id} | 获取订单详情 |
| POST | /api/Orders | 创建订单 |
| PUT | /api/Orders/{id}/pay | 支付订单 |
| PUT | /api/Orders/{id}/cancel | 取消订单 |
| DELETE | /api/Orders/{id} | 删除订单 |

---

## 八、运行项目

### 1. 启动数据库

```bash
docker start campus-cinema-sqlserver
```

### 2. 启动后端

```bash
cd backend/CampusCinema.Api
dotnet run --urls=http://0.0.0.0:5080
```

后端 Swagger 地址：

```text
http://localhost:5080/swagger
```

### 3. 启动前端

```bash
cd frontend/campus-cinema-web
npm run dev -- --host 0.0.0.0
```

前端地址：

```text
http://localhost:5173
```

---

## 九、系统页面说明

### 前台页面

| 页面 | 路径 | 说明 |
|---|---|---|
| 首页 | / | 展示电影列表 |
| 电影详情 | /movies/:id | 展示电影详情和排片 |
| 选座购票 | /booking/:scheduleId | 选择座位并创建订单 |

### 后台页面

| 页面 | 路径 | 说明 |
|---|---|---|
| 电影管理 | /admin/movies | 管理电影信息 |
| 影厅管理 | /admin/halls | 管理影厅信息 |
| 排片管理 | /admin/schedules | 管理电影排片 |
| 订单管理 | /admin/orders | 管理订单信息 |

---

## 十、项目特色

1. 采用前后端分离架构，结构清晰。
2. 使用 ASP.NET Core Web API 提供 RESTful 接口。
3. 使用 Entity Framework Core 操作 SQL Server 数据库。
4. 使用 Vue 3 和 Element Plus 实现后台管理页面。
5. 实现了电影、影厅、排片、订单的完整业务闭环。
6. 排片功能中加入了影厅时间冲突校验。
7. 订单功能中实现了选座、生成订单、模拟支付和后台管理。

---

## 十一、AI 使用说明

在本项目开发过程中，合理使用了 AI 工具辅助完成：

- 项目需求分析
- 系统功能拆分
- 前后端代码结构设计
- 部分代码生成与问题排查
- 报错信息分析
- README 和实验报告内容整理

AI 主要用于辅助学习和提高开发效率，最终系统功能经过人工调试、运行和验证。

---

## 十二、总结

本项目完成了校园影院购票与排片管理系统的主要功能，实现了从后台基础数据维护到前台选座购票、订单支付管理的完整业务流程。通过本项目开发，进一步熟悉了 Vue 前端开发、ASP.NET Core Web API 后端开发、SQL Server 数据库设计以及前后端分离项目的完整开发过程。