using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;
using ProjetNet.Models.ViewModel;
using ProjetNet.Models.ViewModel;

namespace ProjetNet.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ------------------------------
        // 1) CHECKOUT (affiche panier)
        // ------------------------------
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            var vm = new CheckoutViewModel
            {
                CartItems = cartItems,
                Total = cartItems.Sum(c => c.Product.Price * c.Quantity)
            };

            return View(vm);   // ✔️ maintenant c’est correct
        }

        // ------------------------------
        // 2) PLACE ORDER
        // ------------------------------
        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any()) return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                UserId = userId,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderDate = DateTime.Now,
                Status = "Confirmed",
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        // ------------------------------
        // 3) CONFIRMATION
        // ------------------------------
        public IActionResult Confirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.Id == id);

            return View(order);
        }

    }

}
