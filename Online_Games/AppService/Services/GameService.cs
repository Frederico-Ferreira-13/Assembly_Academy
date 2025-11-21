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
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryService _categoryService;

        public GameService(IGameRepository gameRepository, ICategoryService categoryService)
        {
            _gameRepository = gameRepository;
            _categoryService = categoryService;
        }

        public void CreateGame(Game model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "O objeto Game não pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(model.TypeGame))
            {
                throw new ArgumentException("O Tipo de Jogo (TypeGame) é obrigatório.", nameof(model.TypeGame));
            }
            if (model.CategoryId <= 0)
            {
                throw new ArgumentException("O ID da Categoria (CategoryId) deve ser positivo.", nameof(model.CategoryId));
            }

            if (!_gameRepository.IsTitleUnique(model.TypeGame, null))
            {
                throw new ArgumentException($"Já existe um jogo com o título '{model.TypeGame}'.");
            }           

            try
            {
                _categoryService.GetCategoryDetails(model.CategoryId);
            }
            catch (ArgumentException)
            {                
                throw new ArgumentException($"A Categoria com ID {model.CategoryId} não existe.", nameof(model.CategoryId));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar o Jogo: {ex.Message}", ex);
            }
        }

        public Game? GetGameById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID deve ser positivo.", nameof(id));
            }

            Game? game = _gameRepository.Retrieve(id);

            if (game == null)
            {
                throw new ArgumentException($"Jogo com ID {id} não encontrado.");
            }
            return game;
        }

        public List<Game> GetAllGames()
        {            
            return _gameRepository.RetrieveAll() ?? new List<Game>();
        }

        public void UpdateGame(Game model)
        {
            // 1. Validação do Modelo
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "O objeto Game não pode ser nulo.");
            }
            if (model.Id <= 0)
            {
                throw new ArgumentException("O ID do Jogo a atualizar deve ser válido.", nameof(model.Id));
            }
            if (string.IsNullOrWhiteSpace(model.TypeGame))
            {
                throw new ArgumentException("O Tipo de Jogo (TypeGame) é obrigatório.", nameof(model.TypeGame));
            }
            if (model.CategoryId <= 0)
            {
                throw new ArgumentException("O ID da Categoria (CategoryId) deve ser positivo.", nameof(model.CategoryId));
            }

            
            if (!_gameRepository.IsTitleUnique(model.TypeGame, model.Id))
            {
                throw new ArgumentException($"Já existe outro jogo com o título '{model.TypeGame}'.");
            }
            
            try
            {
                _categoryService.GetCategoryDetails(model.CategoryId);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"A Categoria com ID {model.CategoryId} não existe.", nameof(model.CategoryId));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar o Jogo: {ex.Message}", ex);
            }
        }

        public bool RemoveGame(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID deve ser positivo.", nameof(id));
            }         

            try
            {
                return _gameRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover o Jogo com ID {id}: {ex.Message}", ex);
            }
        }

        public List<Game> SearchGames(string searchTerm)
        {            
            return _gameRepository.Search(searchTerm);
        }
        
        public List<Game> GetGamesByGenre(int genreId)
        {
            if (genreId <= 0)
            {
                throw new ArgumentException("O ID do Género deve ser positivo.", nameof(genreId));
            }

            return _gameRepository.GetByGenre(genreId);
        }
        
        public bool IsTitleUnique(string title, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return false; // Um título vazio/nulo não é considerado único/válido.
            }

            return _gameRepository.IsTitleUnique(title, excludeId);
        }
    }
}
