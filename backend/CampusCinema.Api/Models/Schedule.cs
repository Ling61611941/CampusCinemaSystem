namespace CampusCinema.Api.Models;

public class Schedule
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public Movie? Movie { get; set; }

    public int HallId { get; set; }

    public Hall? Hall { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = "正常";
}
