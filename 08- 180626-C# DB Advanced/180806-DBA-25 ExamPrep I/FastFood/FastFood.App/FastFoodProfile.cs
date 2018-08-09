using AutoMapper;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models;
using System.Linq;

namespace FastFood.App
{
	public class FastFoodProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public FastFoodProfile()
		{
            CreateMap<Category, CategoryDto>()
                .ForMember(cfg => cfg.Item,
                            opt => opt.MapFrom(c => c.Items.Where(i => i.Name == c.Name).Select(i => new ItemDto()
                            {
                                Name = i.Name,
                                TotalMade = i.OrderItems.Sum(p => i.Price * p.Quantity),
                                TimesSold = i.OrderItems.Sum(p => p.Quantity)
                            })));
		}
	}
}
