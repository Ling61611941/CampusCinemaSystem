using CampusCinema.Api.Data;
using CampusCinema.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusCinema.Api.Controllers;

public class CreateOrderRequest
{
    public int ScheduleId { get; set; }

    public List<string> SeatCodes { get; set; } = new();
}

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.Schedule)
                .ThenInclude(s => s!.Movie)
            .Include(o => o.Schedule)
                .ThenInclude(s => s!.Hall)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Schedule)
                .ThenInclude(s => s!.Movie)
            .Include(o => o.Schedule)
                .ThenInclude(s => s!.Hall)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound("订单不存在");
        }

        return order;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderRequest request)
    {
        if (request.SeatCodes == null || request.SeatCodes.Count == 0)
        {
            return BadRequest("请至少选择一个座位");
        }

        var schedule = await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.Id == request.ScheduleId);

        if (schedule == null)
        {
            return BadRequest("排片不存在");
        }

        if (schedule.Status != "正常")
        {
            return BadRequest("当前排片不可购票");
        }

        var order = new Order
        {
            OrderNo = $"ORD{DateTime.Now:yyyyMMddHHmmssfff}",
            ScheduleId = request.ScheduleId,
            UserName = "测试用户",
            SeatCodes = string.Join(",", request.SeatCodes),
            TicketCount = request.SeatCodes.Count,
            TotalAmount = schedule.Price * request.SeatCodes.Count,
            Status = "待支付",
            CreatedAt = DateTime.Now
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}/pay")]
    public async Task<IActionResult> PayOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound("订单不存在");
        }

        if (order.Status != "待支付")
        {
            return BadRequest("只有待支付订单可以支付");
        }

        order.Status = "已支付";
        order.PaidAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound("订单不存在");
        }

        if (order.Status == "已支付")
        {
            return BadRequest("已支付订单不能直接取消");
        }

        order.Status = "已取消";

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound("订单不存在");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
