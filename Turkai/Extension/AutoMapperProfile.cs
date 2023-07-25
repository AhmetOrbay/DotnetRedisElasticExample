using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Turkai.Model.Dtos;
using Turkai.Model.Entity;
using Turkai.Model.ExtensionModel.DummpyDataModel;

namespace Turkai.Extension
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDummpy>()
                .ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
