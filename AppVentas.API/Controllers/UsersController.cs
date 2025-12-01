using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppVentas.Modelos;

namespace AppVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppVentasDbbContext _context;

        public UsersController(AppVentasDbbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<User>>>> GetUser()
        {
            try
            {
                var data = await _context.Users.ToListAsync();
                return ApiResult<List<User>>.Ok(data);
            }
            catch (Exception ex)
            {
                return ApiResult<List<User>>.Fail(ex.Message);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<User>>> GetUser(int id)
        {
            try
            {
                var user = await _context
                    .Users
                    .Include(e => e.Orders)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (user == null)
                {
                    return ApiResult<User>.Fail("Datos no encontrados");
                }

                return ApiResult<User>.Ok(user);
            }
            catch (Exception ex)
            {
                return ApiResult<User>.Fail(ex.Message);
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<User>>> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return ApiResult<User>.Fail("No coinciden los identificadores");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(id))
                {
                    return ApiResult<User>.Fail("Datos no encontrados");
                }
                else
                {
                    return ApiResult<User>.Fail(ex.Message);
                }
            }

            return ApiResult<User>.Ok(null);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<User>>> DeleteUser(int id)
        {
            try
            {
                var especie = await _context.Users.FindAsync(id);
                if (especie == null)
                {
                    return ApiResult<User>.Fail("datos no encontrados");
                }

                _context.Users.Remove(especie);
                await _context.SaveChangesAsync();

                return ApiResult<User>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<User>.Fail(ex.Message);
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
