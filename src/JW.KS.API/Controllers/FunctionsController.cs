using System;
using System.Linq;
using System.Threading.Tasks;
using JW.KS.API.Data;
using JW.KS.API.Data.Entities;
using JW.KS.ViewModels;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JW.KS.API.Controllers
{
    public class FunctionsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public FunctionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostFunction([FromBody]FunctionCreateRequest request)
        {
            var function = new Function()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Url = request.Url,
                SortOrder = request.SortOrder,
                ParentId = request.ParentId
            };
            _context.Functions.Add(function);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new {id = function.Id}, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFunctions()
        {
            var functionVms = await _context.Functions.Select(f => new Function()
            {
                Id = f.Id,
                Name = f.Name,
                Url = f.Url,
                SortOrder = f.SortOrder,
                ParentId = f.ParentId
            }).ToListAsync();

            return Ok(functionVms);
        }
        
        [HttpGet("filter")]
        public async Task<IActionResult> GetFunctionsPaging(string filter, int page, int size)
        {
            var query = _context.Functions.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter)
                                         || x.Id.Contains(filter)
                                         || x.Url.Contains(filter)).AsQueryable();
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip((page - 1 * size))
                .Take(size)
                .Select(f => new FunctionVm()
                {
                    Id = f.Id,
                    Name = f.Name,
                    Url = f.Url,
                    SortOrder = f.SortOrder,
                    ParentId = f.ParentId
                })
                .ToListAsync();

            var pagination = new Pagination<FunctionVm>
            {
                Items = items,
                TotalRecords = totalRecords
            };
            
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var function = await _context.Functions.FindAsync(id);
            if (function == null)
                return NotFound();
            var functionVm = new FunctionVm()
            {
                Id = function.Id,
                Name = function.Name,
                Url = function.Url,
                SortOrder = function.SortOrder,
                ParentId = function.ParentId
            };
            return Ok(functionVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFunction(string id, [FromBody]FunctionCreateRequest request)
        {
            var function = await _context.Functions.FindAsync(id);
            if (function == null)
                return NotFound();
            
            function.Name = request.Name;
            function.Url = request.Url;
            function.SortOrder = request.SortOrder;
            function.ParentId = request.ParentId;
            
            _context.Functions.Update(function);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFunction(string id)
        {
            var function = await _context.Functions.FindAsync(id);
            if (function == null)
                return NotFound();

            _context.Functions.Remove(function);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var functionVm = new FunctionVm()
                {
                    Id = function.Id,
                    Name = function.Name,
                    Url = function.Url,
                    SortOrder = function.SortOrder,
                    ParentId = function.ParentId
                };
                return Ok(functionVm);
            }

            return BadRequest();
        }
        
        [HttpGet("{functionId}/commands")]
        public async Task<IActionResult> GetCommands(string functionId)
        {
            var functions = _context.Functions;
            var query = from c in _context.Commands
                join cif in _context.CommandInFunctions on c.Id equals cif.CommandId into result1
                from commandInFunction in result1.DefaultIfEmpty()
                join f in _context.Functions on commandInFunction.FunctionId equals f.Id into result2
                select new
                {
                    c.Id,
                    c.Name,
                    commandInFunction.FunctionId
                };
            
            query = query.Where(x => !x.FunctionId.Equals(functionId));
            
            var data = await query.Select(x => new CommandVm()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return Ok(data);
        }
        
        [HttpGet("{functionId}/commands/not-in-function")]
        public async Task<IActionResult> GetCommantsNotInFunction(string functionId)
        {
            var functions = _context.Functions;
            var query = from c in _context.Commands
                join cif in _context.CommandInFunctions on c.Id equals cif.CommandId into result1
                from commandInFunction in result1.DefaultIfEmpty()
                join f in _context.Functions on commandInFunction.FunctionId equals f.Id into result2
                select new
                {
                    c.Id,
                    c.Name,
                    commandInFunction.FunctionId
                };
            
            query = query.Where(x => !x.FunctionId.Equals(functionId)).Distinct();
            
            var data = await query.Select(x => new CommandVm()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return Ok(data);
        }
        
        [HttpPost("{functionId}/commands")]
        public async Task<IActionResult> PostCommandToFunction(string functionId, [FromBody] AddCommandToFunctionRequest request)
        {
            var commandInFunction = await _context.CommandInFunctions.FindAsync(request.CommandId, request.FunctionId);
            if (commandInFunction != null)
                return BadRequest($"This command has been added to function");

            var entity = new CommandInFunction()
            {
                CommandId = request.CommandId,
                FunctionId = request.FunctionId
            };
            _context.CommandInFunctions.Add(entity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { commandId = request.CommandId, functionId = request.FunctionId }, request);
            }
            else
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{functionId}/commands/{commandId}")]
        public async Task<IActionResult> PostCommandToFunction(string functionId, string commandId)
        {
            var commandInFunction = await _context.CommandInFunctions.FindAsync(functionId, commandId);
            if (commandInFunction == null)
                return BadRequest($"This command is not existed in function");

            var entity = new CommandInFunction()
            {
                CommandId = commandId,
                FunctionId = functionId
            };
            _context.CommandInFunctions.Remove(entity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}