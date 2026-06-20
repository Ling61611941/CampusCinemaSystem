using CampusCinema.Api.Data;
using CampusCinema.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusCinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SchedulesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
        return await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Schedule>> GetSchedule(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (schedule == null)
        {
            return NotFound("排片不存在");
        }

        return schedule;
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByMovie(int movieId)
    {
        return await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Schedule>> CreateSchedule(Schedule schedule)
    {
        if (schedule.EndTime <= schedule.StartTime)
        {
            return BadRequest("结束时间必须晚于开始时间");
        }

        if (schedule.Price <= 0)
        {
            return BadRequest("票价必须大于0");
        }

        var movieExists = await _context.Movies.AnyAsync(m => m.Id == schedule.MovieId);
        var hallExists = await _context.Halls.AnyAsync(h => h.Id == schedule.HallId);

        if (!movieExists)
        {
            return BadRequest("电影不存在");
        }

        if (!hallExists)
        {
            return BadRequest("影厅不存在");
        }

        var hasConflict = await _context.Schedules.AnyAsync(s =>
            s.HallId == schedule.HallId &&
            schedule.StartTime < s.EndTime &&
            schedule.EndTime > s.StartTime
        );

        if (hasConflict)
        {
            return BadRequest("该影厅在当前时间段已有排片，请重新选择时间。");
        }

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return BadRequest("排片编号不一致");
        }

        if (schedule.EndTime <= schedule.StartTime)
        {
            return BadRequest("结束时间必须晚于开始时间");
        }

        var hasConflict = await _context.Schedules.AnyAsync(s =>
            s.Id != id &&
            s.HallId == schedule.HallId &&
            schedule.StartTime < s.EndTime &&
            schedule.EndTime > s.StartTime
        );

        if (hasConflict)
        {
            return BadRequest("该影厅在当前时间段已有排片，请重新选择时间。");
        }

        _context.Entry(schedule).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);

        if (schedule == null)
        {
            return NotFound("排片不存在");
        }

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
