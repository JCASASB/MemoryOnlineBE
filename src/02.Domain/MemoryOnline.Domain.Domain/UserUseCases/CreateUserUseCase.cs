using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Domain.Domain.UserUseCases
{
    public class CreateUserUseCase
    {
        public Usuario Execute(string name, int age, string password)
        {
            // Aquí podrías agregar validaciones o lógica de negocio
            return new Usuario.Builder()
                .WithName(name)
                .WithAge(age)
                .WithPassword(password)
                .WithUserResults(new List<UserMatchResult>())
                .Build();
        }
    }
}
