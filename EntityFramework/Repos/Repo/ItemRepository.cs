using AutoMapper;
using EntityFramework.Data;
using EntityFramework.Models;
using EntityFramework.Models.DTOs;
using EntityFramework.Repos.IRepo;
using Microsoft.EntityFrameworkCore;
namespace EntityFramework.Repos.Repo
{
    public class ItemRepository : IItemRepository
    {
        private readonly MyAppContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(MyAppContext context, IMapper mapper, ILogger<ItemRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {

            var items = await _context.Items.ToListAsync();
            if (items == null || !items.Any())
            {
                _logger.LogWarning("No items found in the database.");
                return Enumerable.Empty<ItemDTO>();
            }
            _logger.LogInformation($"Retrieved {items.Count} items from the database.");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }
        public async Task<ItemDTO> GetItemByIdAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                _logger.LogWarning($"Item with ID {id} not found.");
                return null;
            }
            _logger.LogInformation($"Retrieved item with ID {id} from the database.");
            return _mapper.Map<ItemDTO>(item);
        }
        public async Task<ItemDTO> AddItemAsync(ItemDTO itemDto)
        {
            if (itemDto == null)
            {
                _logger.LogError("Attempted to add a null item.");
                throw new ArgumentNullException(nameof(itemDto), "Item cannot be null.");
            }
            var item = _mapper.Map<Item>(itemDto);
            try
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while adding item to the database.");
                throw;
            }
            return _mapper.Map<ItemDTO>(item);
        }
        public async Task<ItemDTO> UpdateItemAsync(ItemDTO itemDto)
        {
            if (itemDto == null)
            {
                _logger.LogError("Attempted to update a null item.");
                throw new ArgumentNullException(nameof(itemDto), "Item cannot be null.");
            }
            try
            {
                var item = await _context.Items.FindAsync(itemDto.Id);
                if (item == null) return null;
                _mapper.Map(itemDto, item);
                _context.Items.Update(item);
                await _context.SaveChangesAsync();

                return _mapper.Map<ItemDTO>(item);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while updating item in the database.");
                throw;
            }
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Attempted to delete an item with an invalid ID.");
                throw new ArgumentException("ID must be greater than zero.", nameof(id));
            }
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null) return false;
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting item.");
                throw;
            }
        }
    }
}
