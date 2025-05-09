using AutoMapper;
using InventoryApp.Core.Entities;
using InventoryApp.Core.Interfaces;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;

namespace InventoryApp.Service.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAll(int? id)
        {
            if (id.HasValue)
            {
                var filteredProducts = await _unitOfWork.Products.GetAll(p => p.CategoryId == id); // filtered
                return _mapper.Map<IEnumerable<ProductDto>>(filteredProducts);
            }
            var products = await _unitOfWork.Products.GetAll(id);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto> GetById(int productId)
        {
            var products = await _unitOfWork.Products.GetById(productId);
            return _mapper.Map<ProductDto>(products);
        }
        public async Task<bool> AddAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.Add(product);
            return await CompletedAsync() > 0;
        }
        public async Task<bool> Update(int id, ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.Update(id, product);
            return await CompletedAsync() > 0;
        }
        public async Task<bool> Delete(int id)
        {
            await _unitOfWork.Products.Remove(id);
            return await CompletedAsync() > 0;
        }
        public async Task<int> CompletedAsync()
        {
            return await _unitOfWork.CompletedAsync();
        }

    }
}
