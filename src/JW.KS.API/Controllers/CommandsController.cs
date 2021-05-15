using System.Linq;
using System.Threading.Tasks;
using JW.KS.API.Data;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JW.KS.API.Controllers
{
    public class CommandsController : BaseController
    {
        private ApplicationDbContext _context;
        public CommandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommands()
        {
            var commands = _context.Commands;
            var commandVms = await commands.Select(c => new CommandVm()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
            return Ok(commandVms);

        }
    }
}