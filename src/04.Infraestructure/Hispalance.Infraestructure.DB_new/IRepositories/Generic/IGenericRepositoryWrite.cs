namespace Hispalance.Infraestructure.DB.IRepositories.Generic
{
    public interface IGenericRepositoryWrite<TEntity> : IGenericBaseRepository<TEntity>
    {
        void DeleteById(object id);

        void Delete(TEntity entityToDelete);

        void Add(TEntity entityToAdd);

        void Attach(TEntity entityToAttach);

        void Update(TEntity entityToUpdate);

        void SaveChanges();
    }
}
