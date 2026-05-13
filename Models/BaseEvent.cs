using AkilliEtkinlikKatilimSistemi.Enums;

namespace AkilliEtkinlikKatilimSistemi.Models;

// Abstract record + init-only özellikler: Etkinlik modelinin temel ve değişmez yapısını temsil eder.
public abstract record BaseEvent(
    Guid Id,
    string Name,
    EventCategory Category,
    int Capacity,
    DateOnly Date,
    bool IsActive,
    int CurrentParticipantCount
) : IEntity;
