using AkilliEtkinlikKatilimSistemi.Models;

namespace AkilliEtkinlikKatilimSistemi.Services;

public interface IRepository<T> where T : IEntity
{
    IReadOnlyList<T> GetAll();
    T? GetById(Guid id);
    void Add(T item);
    bool Update(T item);
    bool Delete(Guid id);
}
