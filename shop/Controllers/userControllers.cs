using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Models;

namespace shop.Controllers
{
    public class userControllers : Controller
    {
        private readonly shopcontext _context;

        public userControllers(shopcontext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Bill()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Bill(BillingAddress billingAddress)
        {
                _context.BillingAddresses.Add(billingAddress);
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction(nameof(Order));
                }
                else
                {
                    TempData["add"] = "fail";
                }
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var final_quantity = 0;
            var final_amount = 0;
            int id =Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var carts = await _context.carts.FromSqlRaw("select *  from carts where  id = '" + id + "'  ").ToListAsync();
            foreach (var item in carts)
            {
                final_quantity += item.quantity;
                final_amount += item.totalprice;
            }

            TempData["id"] = id;
            TempData["quantity"] = final_quantity;
            TempData["amount"] = final_amount;
            return View();
        }
        
        public async Task<IActionResult> Orders()
        {
            var final_quantity = 0;
            var final_amount = 0;
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var carts = await _context.carts.FromSqlRaw("select *  from carts where  id = '" + id + "'  ").ToListAsync();
            foreach (var item in carts)
            {
                final_quantity += item.quantity;
                final_amount += item.totalprice;
            }
            orders order = new orders();
            order.id = id;
            order.quantity = final_quantity;
            order.totalprice = final_amount;
            _context.Add(order);
            await _context.SaveChangesAsync();
            foreach (var item in carts)
            {
                _context.carts.Remove(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ordersById));
        }

        public async Task<IActionResult> ordersById()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }
            var orders = await _context.orders.FromSqlRaw("select *  from orders where  id = '" + id + "'  ").ToListAsync();
            
            return View(orders);


        }

        public async Task<IActionResult> AllOrders()
        {
            return _context.orders != null ?
                        View(await _context.orders.ToListAsync()) :
                        Problem("Entity set 'shopcontext.orders'  is null.");
        }

        public async Task<IActionResult> DeleteUser(int orderId)
        {
            using (shopcontext db = new shopcontext())
            {
                if (orderId == null)
                {
                    return NotFound();
                }
                var orders = db.orders.FirstOrDefault(x => x.orderId == orderId);
                if (orders != null)
                {
                    db.orders.Remove(orders);
                }
                if(await db.SaveChangesAsync()>0)
                {
                    TempData["cancel"] = "ok";
                }
                return RedirectToAction(nameof(ordersById));
            }
        }


    }
}
