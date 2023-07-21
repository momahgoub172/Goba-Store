
using AutoMapper;
using Goba_Store.Application.Services.Interfaces;
using Goba_Store.Application.View_Models;
using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;

namespace Goba_Store.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository catRepo, IMapper mapper)
        {
            _catRepo = catRepo;
            _mapper = mapper;
        }

        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            var categories = _catRepo.GetAll();
            return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }

        public void CreateCategory(CategoryViewModel viewModel)
        {
                var category = _mapper.Map<Category>(viewModel);
                _catRepo.Add(category);
                _catRepo.Save();
        }

        public CategoryViewModel GetCategoryById(int id)
        {
            var category = _catRepo.Find(id);
            return _mapper.Map<CategoryViewModel>(category);
        }

        public void UpdateCategory(CategoryViewModel viewModel)
        {
                var category = _mapper.Map<Category>(viewModel);
                _catRepo.Update(category);
                _catRepo.Save();
        }

        public void DeleteCategory(int id)
        {
            var category = _catRepo.Find(id);
            if (category != null)
            {
                _catRepo.Remove(category);
                _catRepo.Save();
            }
        }

    }
}
