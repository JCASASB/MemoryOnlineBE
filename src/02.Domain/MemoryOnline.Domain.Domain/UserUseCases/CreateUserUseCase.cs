using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.UserUseCases
{
    public class CreateUserUseCase
    {
        public User Execute(string name, int age, int password)
        {
            // Aquí podrías agregar validaciones o lógica de negocio
            return new User { Name = name, Age = age, Password = password };
        }
    }
}
