using System;
using System.Linq;
using System.Threading.Tasks;
using JW.KS.API.Data.Entities;
using JW.KS.ViewModels;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JW.KS.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _manager;
        
        public UsersController(UserManager<User> manager)
        {
            _manager = manager;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreateRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Dob = request.Dob,
                UserName = request.UserName,
                LastName = request.LastName,
                FirstName = request.FirstName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _manager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new {id = user.Id}, request);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var uservms = await _manager.Users.Select(u => new UserVm()
            {
                Id = u.Id,
                UserName = u.UserName,
                Dob = u.Dob,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToListAsync();

            return Ok(uservms);
        }
        
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllUsersPaging(string filter, int page, int size)
        {
            var query = _manager.Users;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Email.Contains(filter)
                                         || x.UserName.Contains(filter)
                                         || x.PhoneNumber.Contains(filter));
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip((page - 1 * size))
                .Take(size)
                .Select(u => new UserVm()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Dob = u.Dob,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            var pagination = new Pagination<UserVm>
            {
                Items = items,
                TotalRecords = totalRecords
            };
            
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _manager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var userVm = new UserVm()
            {
                Id = user.Id,
                UserName = user.UserName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return Ok(userVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody]UserCreateRequest request)
        {
            var user = await _manager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob = request.Dob;
            
            var result = await _manager.UpdateAsync(user);
            
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _manager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);
            
            var uservm = new UserVm()
            {
                Id = user.Id,
                UserName = user.UserName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return Ok(uservm);
        }
    }
}