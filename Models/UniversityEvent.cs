using AkilliEtkinlikKatilimSistemi.Enums;

namespace AkilliEtkinlikKatilimSistemi.Models;

// Immutable yapı: Record kullanıldığı için etkinlik nesnesi doğrudan değiştirilmez.
// Güncelleme gerektiğinde "with" ifadesiyle yeni bir kopya üretilir.
public sealed record UniversityEvent(
    Guid Id,
    string Name,
    EventCategory Category,
    int Capacity,
    DateOnly Date,
    bool IsActive,
    int CurrentParticipantCount
) : BaseEvent(Id, Name, Category, Capacity, Date, IsActive, CurrentParticipantCount);
