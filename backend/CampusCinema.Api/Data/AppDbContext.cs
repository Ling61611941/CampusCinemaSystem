using CampusCinema.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusCinema.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                Id = 1,
                Name = "流浪地球2",
                Category = "科幻",
                Director = "郭帆",
                Actors = "吴京、刘德华、李雪健",
                Description = "太阳即将毁灭，人类在地球表面建造出巨大的推进器，寻找新的家园。",
                PosterUrl = "https://picsum.photos/300/420?random=1",
                Duration = 173,
                ReleaseDate = new DateTime(2023, 1, 22),
                Score = 8.3m,
                Status = "正在热映"
            },
            new Movie
            {
                Id = 2,
                Name = "满江红",
                Category = "悬疑",
                Director = "张艺谋",
                Actors = "沈腾、易烊千玺、张译",
                Description = "南宋绍兴年间，一场围绕密信展开的悬疑故事。",
                PosterUrl = "https://picsum.photos/300/420?random=2",
                Duration = 159,
                ReleaseDate = new DateTime(2023, 1, 22),
                Score = 7.0m,
                Status = "正在热映"
            }
        );
    }
}
