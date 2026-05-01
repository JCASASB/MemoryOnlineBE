
using MediatR;
using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public class GetUserStatsHandler : IRequestHandler<GetUserStatsQuery, List<UserResults>>
    {
        private readonly IApplicationRepository _appRepository;

        public GetUserStatsHandler(IApplicationRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<List<UserResults>> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _appRepository.GetUserWithFilter(new UserResultsByUserIdSpec(request.id));
                
                return user.First().Results;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
