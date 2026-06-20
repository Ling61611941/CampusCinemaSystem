namespace CampusCinema.Api.Models;

public class Movie
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Director { get; set; } = string.Empty;

    public string Actors { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string PosterUrl { get; set; } = string.Empty;

    public int Duration { get; set; }

    public DateTime ReleaseDate { get; set; }

    public decimal Score { get; set; }

    public string Status { get; set; } = "正在热映";
}
