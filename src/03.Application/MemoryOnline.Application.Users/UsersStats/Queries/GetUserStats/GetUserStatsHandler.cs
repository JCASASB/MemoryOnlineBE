
using MediatR;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Domain.UserStatsUseCases;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public class GetUserStatsHandler : IRequestHandler<GetUserStatsQuery, UserStats>
    {
        private readonly IApplicationUOW _appRepository;

        private readonly IGetUserStatsByIdUserUseCase _getUserStatsByIdUserUseCase;
        
        public GetUserStatsHandler(IApplicationUOW appRepository
            , IGetUserStatsByIdUserUseCase getUserStatsByIdUserUseCase)
        {
            _appRepository = appRepository;
            _getUserStatsByIdUserUseCase = getUserStatsByIdUserUseCase;
        }

        public async Task<UserStats> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _appRepository.GetUserResultsWithFilterAsync(new UserResultsFilterByUserIdSpec(request.id));
                

                if(results.Count() > 0)
                {
                    var userStats = _getUserStatsByIdUserUseCase.Execute(results.First().UsuarioId, results);
                    return userStats;
                }

                return new UserStats.Builder().Build();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
