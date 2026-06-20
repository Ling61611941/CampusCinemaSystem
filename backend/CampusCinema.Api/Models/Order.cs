namespace CampusCinema.Api.Models;

public class Order
{
    public int Id { get; set; }

    public string OrderNo { get; set; } = string.Empty;

    public int ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    public string UserName { get; set; } = "测试用户";

    public string SeatCodes { get; set; } = string.Empty;

    public int TicketCount { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "待支付";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? PaidAt { get; set; }
}
