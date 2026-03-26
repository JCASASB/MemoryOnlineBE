using MemoryOnline.Domain.Entities;
using System;

namespace MemoryOnline.Domain.Entities.UserUseCases
{
    public class CreateUserUseCase
    {
        public Usuario Execute(string name, int age, string password)
        {
            // Aquí podrías agregar validaciones o lógica de negocio
            return new Usuario { Name = name, Age = age, Password = password };
        }
    }
}
