using AppService.Interfaces;
using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void RegisterNewCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("O Nome da Categoria é obrigatório.", nameof(category.CategoryName));
            }

            if (_categoryRepository.ExistsWithName(category.CategoryName))
            {
                throw new InvalidOperationException($"Uma categoria com o nome '{category.CategoryName}' já existe.");
            }

            _categoryRepository.Create(category);
        }

        public void UpdateCategory(Category category)
        {
            if(category.Id <= 0 || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException("Dados de categoria para atualização são inválidos.");
            }

            if (_categoryRepository.ExistsWithName(category.CategoryName, category.Id))
            {
                throw new InvalidOperationException($"Uma categoria com o nome '{category.CategoryName}' já existe.");
            }
            _categoryRepository.Update(category);
        }

        public bool RemoveCategory(int id)
        {
            if (id <= 0) 
            { 
                throw new ArgumentException("ID inválido para remoção.", nameof(id)); 
            }

            return _categoryRepository.Delete(id);
        }

        public Category GetCategoryDetails(int id)
        {
            if (id <= 0) 
            { 
                throw new ArgumentException("ID inválido para consulta.", nameof(id)); 
            }

            Category? category = _categoryRepository.Retrieve(id);

            if (category == null)
            {
                throw new InvalidOperationException($"Categoria com ID {id} não encontrada.");
            }

            return category;
        }

        public List<Category> GetCategoryList()
        {
            return _categoryRepository.RetrieveAll();
        }

        public Category? GetByName(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return null;
            }

            return _categoryRepository.GetByName(categoryName);
        }

        public bool IsCategoryNameUnique(string categoryName, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return false;
            }

            var allCategories = _categoryRepository.RetrieveAll();

            return !_categoryRepository.ExistsWithName(categoryName, excludeId);
        }
        
        public List<Category> GetActiveCategories()
        {
            return _categoryRepository.RetrieveAll()
                .Where(c => c.IsActive)
                .ToList();
        }
    }
}
