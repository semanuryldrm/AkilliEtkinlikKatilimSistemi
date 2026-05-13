using AkilliEtkinlikKatilimSistemi.Enums;

namespace AkilliEtkinlikKatilimSistemi.Models;

public sealed record Participant(
    Guid Id,
    string StudentName,
    string StudentNumber,
    Guid RegisteredEventId,
    AttendanceStatus Status
) : IEntity;
