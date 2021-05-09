using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.KS.ViewModels;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JW.KS.API.Controllers
{
    public class RolesController : BaseController
    {
        private readonly RoleManager<IdentityRole> _manager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _manager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(RoleCreateRequest request)
        {
            var role = new IdentityRole()
            {
                Id = request.Id,
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };
            var result = await _manager.CreateAsync(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new {id = role.Id}, request);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _manager.Roles.Select(r => new RoleVm
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();

            return Ok(roles);
        }
        
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllRolesPaging(string filter, int page, int size)
        {
            var query = _manager.Roles;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Id.Contains(filter) || x.Name.Contains(filter));
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip(page - 1 * size)
                .Take(size)
                .Select(x => new RoleVm()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            var pagination = new Pagination<RoleVm>
            {
                Items = items,
                TotalRecords = totalRecords
            };
            
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _manager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var roleVm = new RoleVm()
            {
                Id = role.Id,
                Name = role.Name
            };
            return Ok(roleVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody]RoleCreateRequest request)
        {
            if (id != request.Id)
                return BadRequest();
            var role = await _manager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();

            var result = await _manager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _manager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await _manager.DeleteAsync(role);
            if (result.Succeeded)
            {
                var roleVm = new RoleVm()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(roleVm);
            }

            return BadRequest(result.Errors);
        }
    }
}