using System.Diagnostics;
using EntityFramework.Models;
using EntityFramework.Repos.IRepo;
using Microsoft.AspNetCore.Mvc;
using EntityFramework.Models.DTOs;

namespace EntityFramework.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemRepository _itemRepository;

        public HomeController(ILogger<HomeController> logger, IItemRepository itemRepository)
        {
            _logger = logger;
            _itemRepository = itemRepository;
        }

        // GET: Item
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all items from the repository.");
            var items = await _itemRepository.GetAllItemsAsync();
            if (items == null || !items.Any())
            {
                _logger.LogWarning("No items found in the repository.");
                TempData["ErrorMessage"] = "No items found.";
                return View(new List<ItemDTO>());
            }

            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Creates a new item and adds it to the repository.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price")] ItemDTO item)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating a new item with Name: {Name}, Price: {Price}", item.Name, item.Price);
                await _itemRepository.AddItemAsync(item);
                return RedirectToAction("Index");
            }
            _logger.LogError("Model state is invalid for item creation.");
            return View("Index", item);
        }
        /// <summary>
        /// Updates an item 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ItemDTO item)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating item with ID: {Id}, Name: {Name}, Price: {Price}", item.Id, item.Name, item.Price);
                await _itemRepository.UpdateItemAsync(item);
                TempData["SuccessMessage"] = "Record updated successfully";
            }
            else
            {
                _logger.LogError("Model state is invalid for item update.");
                TempData["ErrorMessage"] = "Error while updating record";
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ID provided for deletion: {Id}", id);
                TempData["ErrorMessage"] = "Invalid ID provided for deletion";
                return RedirectToAction(nameof(Index));
            }            
            else
            {
                _logger.LogInformation("Deleting item with ID: {Id}", id);
                await _itemRepository.DeleteItemAsync(id);
                TempData["SuccessMessage"] = "Record deleted successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
