using CampusCinema.Api.Data;
using CampusCinema.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusCinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallsController : ControllerBase
{
    private readonly AppDbContext _context;

    public HallsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hall>>> GetHalls(string? keyword)
    {
        var query = _context.Halls.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(h => h.Name.Contains(keyword));
        }

        return await query.OrderByDescending(h => h.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Hall>> GetHall(int id)
    {
        var hall = await _context.Halls.FindAsync(id);

        if (hall == null)
        {
            return NotFound("影厅不存在");
        }

        return hall;
    }

    [HttpPost]
    public async Task<ActionResult<Hall>> CreateHall(Hall hall)
    {
        if (hall.RowCount <= 0 || hall.ColumnCount <= 0)
        {
            return BadRequest("影厅行数和列数必须大于0");
        }

        _context.Halls.Add(hall);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHall), new { id = hall.Id }, hall);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHall(int id, Hall hall)
    {
        if (id != hall.Id)
        {
            return BadRequest("影厅编号不一致");
        }

        if (hall.RowCount <= 0 || hall.ColumnCount <= 0)
        {
            return BadRequest("影厅行数和列数必须大于0");
        }

        _context.Entry(hall).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHall(int id)
    {
        var hall = await _context.Halls.FindAsync(id);

        if (hall == null)
        {
            return NotFound("影厅不存在");
        }

        _context.Halls.Remove(hall);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
