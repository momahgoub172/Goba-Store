using Goba_Store.Application.View_Models;

namespace Goba_Store.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAllCategories();
        void CreateCategory(CategoryViewModel viewModel);
        CategoryViewModel GetCategoryById(int id);
        void UpdateCategory(CategoryViewModel viewModel);
        void DeleteCategory(int id);
    }
}
