using AutoMapper;
using StoreMarket.Models;
using StoreMarket.Models.DTO;

namespace StoreMarket.Repo
{
    public class MappingProfiler : Profile
    {
        public MappingProfiler() 
        {
            CreateMap<Product, ProductDTO>(MemberList.Destination).ReverseMap();
            CreateMap<Category, CategoryDTO>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageDTO>(MemberList.Destination).ReverseMap();
        }
    }
}
