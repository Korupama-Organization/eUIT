using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eUIT.API.Data;
using eUIT.API.DTOs;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly eUITDbContext _context;

    public StudentsController(eUITDbContext context)
    {
        _context = context;
    }

    private class PostQueryResult
    {
        public string tieu_de { get; set; } = string.Empty;
        public DateTimeOffset ngay_dang { get; set; }        
    }



}