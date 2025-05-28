using AutoMapper;
using EntityFramework.Models;
using EntityFramework.Models.DTOs;

namespace EntityFramework.Mappings
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemDTO>();

            CreateMap<ItemDTO, Item>();

            /// <summary>
            /// I also learned that some advanced mapping techniques could look like:
            /// CreateMap<Book, BookDTO>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title + " by " + src.Author)); for custom values
            /// CreateMap<BookDTO, Book>().ForMember((dest => dest.Price, opt => opt.Condition(src => src.Price > 0)); Conditionally map properties

        }
    }
}
