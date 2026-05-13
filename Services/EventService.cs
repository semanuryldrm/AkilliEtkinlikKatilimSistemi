using AkilliEtkinlikKatilimSistemi.Enums;
using AkilliEtkinlikKatilimSistemi.Exceptions;
using AkilliEtkinlikKatilimSistemi.Models;

namespace AkilliEtkinlikKatilimSistemi.Services;

public sealed class EventService
{
    private readonly IRepository<UniversityEvent> _eventRepository;
    private readonly IRepository<Participant> _participantRepository;

    public EventService(
        IRepository<UniversityEvent> eventRepository,
        IRepository<Participant> participantRepository)
    {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
    }

    public UniversityEvent AddEvent(string name, EventCategory category, int capacity, DateOnly date)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Etkinlik adı boş bırakılamaz.");
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("Kontenjan 0'dan büyük olmalıdır.");
        }

        UniversityEvent newEvent = new(
            Id: Guid.NewGuid(),
            Name: name.Trim(),
            Category: category,
            Capacity: capacity,
            Date: date,
            IsActive: true,
            CurrentParticipantCount: 0);

        _eventRepository.Add(newEvent);
        return newEvent;
    }

    public Participant AddParticipant(Guid eventId, string studentName, string studentNumber)
    {
        UniversityEvent eventItem = GetEventOrThrow(eventId);

        if (!eventItem.IsActive)
        {
            throw new InvalidOperationException("Pasif durumdaki bir etkinliğe katılımcı eklenemez.");
        }

        bool alreadyRegistered = _participantRepository
            .GetAll()
            .Any(participant => participant.RegisteredEventId == eventId
                                && participant.StudentNumber == studentNumber.Trim()
                                && participant.Status != AttendanceStatus.Cancelled);

        if (alreadyRegistered)
        {
            throw new InvalidOperationException("Bu öğrenci zaten ilgili etkinliğe kayıtlıdır.");
        }

        int activeParticipantCount = CountParticipantsUsingSeat(eventId);

        if (activeParticipantCount >= eventItem.Capacity)
        {
            throw new CapacityFullException(eventItem.Name);
        }

        Participant participant = new(
            Id: Guid.NewGuid(),
            StudentName: studentName.Trim(),
            StudentNumber: studentNumber.Trim(),
            RegisteredEventId: eventId,
            Status: AttendanceStatus.Registered);

        _participantRepository.Add(participant);

        UniversityEvent updatedEvent = eventItem with
        {
            CurrentParticipantCount = activeParticipantCount + 1
        };

        _eventRepository.Update(updatedEvent);
        return participant;
    }

    public IReadOnlyList<UniversityEvent> GetAllEvents()
    {
        // Lambda expression kullanımı: Etkinlikler tarihe göre sıralanır.
        return _eventRepository
            .GetAll()
            .OrderBy(eventItem => eventItem.Date)
            .ThenBy(eventItem => eventItem.Name)
            .ToList();
    }

    public IReadOnlyList<Participant> GetAllParticipants()
    {
        return _participantRepository
            .GetAll()
            .OrderBy(participant => participant.StudentName)
            .ToList();
    }

    public IReadOnlyList<Participant> GetParticipantsByEvent(Guid eventId)
    {
        return _participantRepository
            .GetAll()
            .Where(participant => participant.RegisteredEventId == eventId)
            .OrderBy(participant => participant.StudentName)
            .ToList();
    }

    // Higher-order function: Dışarıdan filtreleme koşulu alan dinamik metot.
    public IReadOnlyList<UniversityEvent> FilterEvents(Func<UniversityEvent, bool> condition)
    {
        return _eventRepository
            .GetAll()
            .Where(condition)
            .OrderBy(eventItem => eventItem.Date)
            .ToList();
    }

    public bool ChangeEventStatus(Guid eventId, bool isActive)
    {
        UniversityEvent eventItem = GetEventOrThrow(eventId);

        // Immutable yapı: Mevcut nesne değiştirilmez, with ile yeni nesne oluşturulur.
        UniversityEvent updatedEvent = eventItem with { IsActive = isActive };
        return _eventRepository.Update(updatedEvent);
    }

    public bool ChangeParticipantStatus(Guid participantId, AttendanceStatus newStatus)
    {
        Participant? participant = _participantRepository.GetById(participantId);

        if (participant is null)
        {
            return false;
        }

        bool oldStatusUsesSeat = participant.Status != AttendanceStatus.Cancelled;
        bool newStatusUsesSeat = newStatus != AttendanceStatus.Cancelled;

        Participant updatedParticipant = participant with { Status = newStatus };
        bool updated = _participantRepository.Update(updatedParticipant);

        if (!updated || oldStatusUsesSeat == newStatusUsesSeat)
        {
            return updated;
        }

        UniversityEvent eventItem = GetEventOrThrow(participant.RegisteredEventId);
        int difference = newStatusUsesSeat ? 1 : -1;

        UniversityEvent updatedEvent = eventItem with
        {
            CurrentParticipantCount = Math.Max(0, eventItem.CurrentParticipantCount + difference)
        };

        _eventRepository.Update(updatedEvent);
        return true;
    }

    // Recursion kullanımı: Etkinlikler döngü yerine recursive metotla liste metnine dönüştürülür.
    public List<string> GetRecursiveEventList(IReadOnlyList<UniversityEvent> events, int index = 0, List<string>? result = null)
    {
        result ??= new List<string>();

        if (index >= events.Count)
        {
            return result;
        }

        result.Add(FormatEvent(events[index]));
        return GetRecursiveEventList(events, index + 1, result);
    }

    // Pattern matching kullanımı: Etkinlik kategorisine ve aktiflik durumuna göre açıklama üretilir.
    public string GetCategoryMessage(UniversityEvent eventItem)
    {
        return eventItem switch
        {
            { IsActive: false } => "Bu etkinlik pasif durumdadır. Yeni kayıt alınmaz.",
            { Category: EventCategory.Technology } => "Teknoloji etkinliği: Yazılım, yapay zekâ veya siber güvenlik alanına yöneliktir.",
            { Category: EventCategory.Academic } => "Akademik etkinlik: Ders, seminer veya bilimsel gelişim amaçlıdır.",
            { Category: EventCategory.Culture } => "Kültür etkinliği: Sosyal ve kültürel katılımı destekler.",
            { Category: EventCategory.Sport } => "Spor etkinliği: Fiziksel aktivite ve takım çalışmasını destekler.",
            { Category: EventCategory.Career } => "Kariyer etkinliği: Mesleki gelişim ve iş hayatına hazırlık sağlar.",
            _ => "Genel etkinlik: Üniversite içi katılımı artırmayı amaçlar."
        };
    }

    public string FormatEvent(UniversityEvent eventItem)
    {
        string activeText = eventItem.IsActive ? "Aktif" : "Pasif";
        return $"{eventItem.Name} | Kategori: {GetCategoryTurkishName(eventItem.Category)} | " +
               $"Tarih: {eventItem.Date:dd.MM.yyyy} | Kontenjan: {eventItem.CurrentParticipantCount}/{eventItem.Capacity} | Durum: {activeText}";
    }

    public string FormatParticipant(Participant participant)
    {
        UniversityEvent? eventItem = _eventRepository.GetById(participant.RegisteredEventId);
        string eventName = eventItem?.Name ?? "Bilinmeyen etkinlik";

        return $"{participant.StudentName} ({participant.StudentNumber}) | Etkinlik: {eventName} | Durum: {GetAttendanceStatusTurkishName(participant.Status)}";
    }

    public static string GetCategoryTurkishName(EventCategory category)
    {
        return category switch
        {
            EventCategory.Academic => "Akademik",
            EventCategory.Technology => "Teknoloji",
            EventCategory.Culture => "Kültür",
            EventCategory.Sport => "Spor",
            EventCategory.Career => "Kariyer",
            _ => "Diğer"
        };
    }

    public static string GetAttendanceStatusTurkishName(AttendanceStatus status)
    {
        return status switch
        {
            AttendanceStatus.Registered => "Kayıtlı",
            AttendanceStatus.Attended => "Katıldı",
            AttendanceStatus.Absent => "Katılmadı",
            AttendanceStatus.Cancelled => "İptal Edildi",
            _ => "Bilinmiyor"
        };
    }

    private int CountParticipantsUsingSeat(Guid eventId)
    {
        return _participantRepository
            .GetAll()
            .Count(participant => participant.RegisteredEventId == eventId
                                  && participant.Status != AttendanceStatus.Cancelled);
    }

    private UniversityEvent GetEventOrThrow(Guid eventId)
    {
        UniversityEvent? eventItem = _eventRepository.GetById(eventId);

        return eventItem ?? throw new KeyNotFoundException("Etkinlik bulunamadı.");
    }
}
