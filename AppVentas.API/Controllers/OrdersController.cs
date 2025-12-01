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
    public class OrdersController : ControllerBase
    {
        private readonly AppVentasDbbContext _context;

        public OrdersController(AppVentasDbbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Order>>>> GetOrder()
        {
            try
            {
                var data = await _context.Orders.ToListAsync();
                return ApiResult<List<Order>>.Ok(data);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Order>>.Fail(ex.Message);
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Order>>> GetOrder(int id)
        {
            try
            {
                var order = await _context
                    .Orders
                    .Include(e => e.User)
                    .Include(a => a.Product)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (order == null)
                {
                    return ApiResult<Order>.Fail("Datos no encontrados");
                }

                return ApiResult<Order>.Ok(order);
            }
            catch (Exception ex)
            {
                return ApiResult<Order>.Fail(ex.Message);
            }
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Order>>> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return ApiResult<Order>.Fail("Identificador no coincide");
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!OrderExists(id))
                {
                    return ApiResult<Order>.Fail("Datos no encontrados");
                }
                else
                {
                    return ApiResult<Order>.Fail(ex.Message);
                }
            }

            return ApiResult<Order>.Ok(null);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult<Order>>> PostOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return ApiResult<Order>.Ok(order);
            }
            catch (Exception ex)
            {
                return ApiResult<Order>.Fail(ex.Message);
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<Order>>> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return ApiResult<Order>.Fail("datos no encontrados");
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return ApiResult<Order>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<Order>.Fail(ex.Message);
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
