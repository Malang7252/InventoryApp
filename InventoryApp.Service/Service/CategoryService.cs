using AutoMapper;
using InventoryApp.Core.Entities;
using InventoryApp.Core.Interfaces;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;

namespace InventoryApp.Service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDto>> GetAll(int? id)
        {
            if (id.HasValue)
            {
                var filteredCategories = await _unitOfWork.Categories.GetAll(p => p.Id == id);
                return _mapper.Map<IEnumerable<CategoryDto>>(filteredCategories);
            }
            var categories = await _unitOfWork.Categories.GetAll(id);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        public async Task<CategoryDto> GetById(int productId)
        {
            var category = await _unitOfWork.Categories.GetById(productId);
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<bool> AddAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.Categories.Add(category);
            return await CompletedAsync() > 0;
        }
        public async Task<bool> Update(int id, CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = id;
            await _unitOfWork.Categories.Update(id, category);
            return await CompletedAsync() > 0;
        }
        public async Task<bool> Delete(int id)
        {
            await _unitOfWork.Categories.Remove(id);
            return await CompletedAsync() > 0;
        }
        public async Task<int> CompletedAsync()
        {
            return await _unitOfWork.CompletedAsync();
        }
    }
}
