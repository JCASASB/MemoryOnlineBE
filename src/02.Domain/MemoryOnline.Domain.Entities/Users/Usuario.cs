using MemoryOnline.Domain.Entities.Stats;

namespace MemoryOnline.Domain.Entities.Users
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Password { get; private set; }

        public required List<UserMatchResult> Results { get; set; } 

        public class Builder
        {
            private readonly Usuario _usuario = new Usuario
            {
                Name = string.Empty,
                Password = string.Empty,
                Results = new List<UserMatchResult>()
            };

            public Builder()
            {
                // Valores por defecto iniciales
                _usuario.Id = Guid.NewGuid();
            }

            public Builder WithId(Guid id)
            {
                _usuario.Id = id;
                return this;
            }

            public Builder WithName(string name)
            {
                _usuario.Name = name;
                return this;
            }

            public Builder WithAge(int age)
            {
                _usuario.Age = age;
                return this;
            }

            public Builder WithPassword(string password)
            {
                _usuario.Password = password;
                return this;
            }

            public Builder WithUserResults(List<UserMatchResult> results)
            {
                _usuario.Results = results;
                return this;
            }

            public Usuario Build()
            {
                return _usuario;
            }
        }
    }
}
