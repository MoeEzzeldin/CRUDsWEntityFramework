using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFramework.Models.DTOs;
namespace EntityFramework.Repos.IRepo
{
    public interface IItemRepository
    {
        public Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
        public Task<ItemDTO> GetItemByIdAsync(int id);
        public Task<ItemDTO> AddItemAsync(ItemDTO item);
        public Task<ItemDTO> UpdateItemAsync(ItemDTO item);
        public Task<bool> DeleteItemAsync(int id);
    }
}
