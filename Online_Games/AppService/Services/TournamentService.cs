using AppService.Interfaces;
using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppService.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserService _userService;
        private readonly IMatchService _matchService;

        // Assumimos que existe uma tabela/entidade para gerir as inscrições (TournamentParticipants)
        // Usaremos o repositório para interagir com essa lógica.

        public TournamentService(ITournamentRepository tournamentRepository, IUserService userService,
            IMatchService matchService)
        {
            _tournamentRepository = tournamentRepository;
            _userService = userService;
            _matchService = matchService;
        }

        public List<Tournament> GetUpcomingTournaments()
        {
            // Delega a consulta complexa (filtro por data e estado) ao Repositório.
            return _tournamentRepository.GetUpcomingTournaments();
        }

        public List<Tournament> GetActiveTournaments()
        {
            // Delega a consulta (filtro por Status = 'InProgress') ao Repositório.
            return _tournamentRepository.GetActiveTournaments();
        }

        public void RegisterPlayer(int tournamentId, int userId)
        {
            if (tournamentId <= 0 || userId <= 0)
            {
                throw new ArgumentException("IDs de torneio e utilizador devem ser positivos.");
            }

            // **Substituição do Retrieve interno por _tournamentRepository.GetById**
            Tournament? tournament = _tournamentRepository.Retrieve(tournamentId);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"Torneio com ID {tournamentId} não encontrado.");
            }

            if(tournament.Status != "Scheduled") // Exemplo de validação: só permite inscrição antes de começar
            {
                throw new InvalidOperationException("Não é possível registar jogadores num torneio que não esteja agendado.");
            }        

            if (_userService.Retrieve(userId) == null) // Valida a existência do utilizador
            {
                throw new KeyNotFoundException($"Utilizador com ID {userId} não encontrado.");
            }

            // Lógica de Inscrição: Delega a criação do registo de participante para o Repositório.
            if (_tournamentRepository.IsPlayerRegistered(tournamentId, userId))
            {
                throw new InvalidOperationException($"O utilizador {userId} já está registado no torneio {tournamentId}.");
            }

            _tournamentRepository.RegisterPlayer(tournamentId, userId);
        }

        public Tournament StartTournament(int tournamentId)
        {
            Tournament? tournament = _tournamentRepository.Retrieve(tournamentId);

            if (tournament == null)
            {
                throw new KeyNotFoundException($"Torneio com ID {tournamentId} não encontrado.");
            }
            // Aqui, o torneio deve estar no estado 'Agendado' e a data ser próxima/agora.
            // Lógica de Negócio e Geração de Partidas:
            // Mudar o estado do Torneio (assumimos que há um campo 'Status')
            tournament.Status = "InProgress"; 

            // Gerar as primeiras partidas (Seeding/Sorteio)
            List<int> players = _tournamentRepository.GetRegisteredPlayerIds(tournamentId);

            if (players.Count < 2)
            {
                throw new InvalidOperationException("Não há jogadores suficientes para iniciar o torneio.");
            }

            // 6. Geração de Partidas (Exemplo: Sorteio Simples)
            for (int i = 0; i < players.Count / 2; i++)
            {
                int player1Id = players[i * 2];
                int player2Id = players[i * 2 + 1];
                // Aqui seria a lógica de criação de Match através do IMatchService
                _matchService.Create(new Match
                {
                    TournamentId = tournamentId,
                    Player1Id = player1Id,
                    Player2Id = player2Id,                    
                });
            }

            //  Atualizar e Persistir o Torneio
            _tournamentRepository.Update(tournament);

            return tournament;
        }

        public Tournament EndTournament(int tournamentId)
        {
            Tournament? tournament = _tournamentRepository.Retrieve(tournamentId);

            if (tournament == null)
            {
                throw new KeyNotFoundException($"Torneio com ID {tournamentId} não encontrado.");
            }            
           
            tournament.Status = "Completed";
            
            tournament.WinnerId = _tournamentRepository.GetFinalWinner(tournamentId);

            _tournamentRepository.Update(tournament);           

            return tournament;
        }
    }
}
