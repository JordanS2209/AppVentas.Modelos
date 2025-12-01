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
    public class ProductsController : ControllerBase
    {
        private readonly AppVentasDbbContext _context;

        public ProductsController(AppVentasDbbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Product>>>> GetProduct()
        {
            try
            {
                var data = await _context.Products.ToListAsync();
                return ApiResult<List<Product>>.Ok(data);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Product>>.Fail(ex.Message);
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Product>>> GetProduct(int id)
        {
            try
            {
                var product = await _context
                    .Products
                    .Include(e => e.Orders)
                    .FirstOrDefaultAsync(e => e.Id== id);

                if (product == null)
                {
                    return ApiResult<Product>.Fail("Datos no encontrados");
                }

                return ApiResult<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                return ApiResult<Product>.Fail(ex.Message);
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Product>>> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return ApiResult<Product>.Fail("No coinciden los identificadores");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    return ApiResult<Product>.Fail("Datos no encontrados");
                }
                else
                {
                    return ApiResult<Product>.Fail(ex.Message);
                }
            }


            return ApiResult<Product>.Ok(null);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult<Product>>> PostProduct(Product product)
        {

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return ApiResult<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                return ApiResult<Product>.Fail(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<Product>>> DeleteProduct(int id)
        {
            try
            {
                var especie = await _context.Products.FindAsync(id);
                if (especie == null)
                {
                    return ApiResult<Product>.Fail("datos no encontrados");
                }

                _context.Products.Remove(especie);
                await _context.SaveChangesAsync();

                return ApiResult<Product>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<Product>.Fail(ex.Message);
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
