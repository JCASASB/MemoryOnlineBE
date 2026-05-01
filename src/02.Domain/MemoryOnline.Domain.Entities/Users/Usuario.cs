using MemoryOnline.Domain.Entities.Stats;

namespace MemoryOnline.Domain.Entities.Users
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public string ConnectionIdHub { get; set; }

        public List<UserResults> Results { get; set; } 

        public class Builder
        {
            private readonly Usuario _usuario = new Usuario();

            public Builder()
            {
                // Valores por defecto iniciales
                _usuario.Id = Guid.NewGuid();
                _usuario.ConnectionIdHub = "";
                _usuario.Results = new List<UserResults>();
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

            public Builder WithConnectionIdHub(string connectionIdHub)
            {
                _usuario.ConnectionIdHub = connectionIdHub;
                return this;
            }

            public Builder WithUserResults(List<UserResults> results)
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
