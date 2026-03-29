
namespace MemoryOnline.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public string ConnectionIdHub { get; set; }

        public class Builder
        {
            private readonly Usuario _usuario = new Usuario();

            public Builder()
            {
                // Valores por defecto iniciales
                _usuario.Id = Guid.NewGuid();
                _usuario.ConnectionIdHub = "";
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

            public Usuario Build()
            {
                return _usuario;
            }
        }
    }
}
