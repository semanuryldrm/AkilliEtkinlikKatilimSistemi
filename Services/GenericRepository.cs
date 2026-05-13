using AkilliEtkinlikKatilimSistemi.Models;

namespace AkilliEtkinlikKatilimSistemi.Services;

// Generic type kullanımı: Aynı repository yapısı hem etkinlik hem katılımcı için kullanılabilir.
public sealed class GenericRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

    public T? GetById(Guid id) => _items.FirstOrDefault(item => item.Id == id);

    public void Add(T item) => _items.Add(item);

    public bool Update(T item)
    {
        int index = _items.FindIndex(existingItem => existingItem.Id == item.Id);

        if (index == -1)
        {
            return false;
        }

        _items[index] = item;
        return true;
    }

    public bool Delete(Guid id)
    {
        T? item = GetById(id);

        if (item is null)
        {
            return false;
        }

        return _items.Remove(item);
    }
}
