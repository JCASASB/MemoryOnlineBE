
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
        private readonly IApplicationRepository _appRepository;

        private readonly IGetUserStatsByIdUserUseCase _getUserStatsByIdUserUseCase;
        
        public GetUserStatsHandler(IApplicationRepository appRepository
            , IGetUserStatsByIdUserUseCase getUserStatsByIdUserUseCase)
        {
            _appRepository = appRepository;
            _getUserStatsByIdUserUseCase = getUserStatsByIdUserUseCase;
        }

        public async Task<UserStats> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _appRepository.GetUserWithFilter(new UserResultsByUserIdSpec(request.id));
                
                if(user.Count() > 0)
                {
                    var userStats = _getUserStatsByIdUserUseCase.Execute(user.First().Id, user.First().Results);
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
