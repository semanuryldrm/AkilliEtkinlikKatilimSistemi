using System.Text;
using AkilliEtkinlikKatilimSistemi.Enums;
using AkilliEtkinlikKatilimSistemi.Exceptions;
using AkilliEtkinlikKatilimSistemi.Models;
using AkilliEtkinlikKatilimSistemi.Services;

Console.OutputEncoding = Encoding.UTF8;

IRepository<UniversityEvent> eventRepository = new GenericRepository<UniversityEvent>();
IRepository<Participant> participantRepository = new GenericRepository<Participant>();
EventService eventService = new(eventRepository, participantRepository);

SeedSampleData(eventService);
RunApplication(eventService);

static void RunApplication(EventService eventService)
{
    bool isRunning = true;

    while (isRunning)
    {
        PrintMenu();
        string choice = ReadRequiredText("Seçiminiz: ");

        try
        {
            switch (choice)
            {
                case "1":
                    AddEventMenu(eventService);
                    break;
                case "2":
                    AddParticipantMenu(eventService);
                    break;
                case "3":
                    ListEvents(eventService.GetAllEvents(), eventService);
                    break;
                case "4":
                    FilterByCategoryMenu(eventService);
                    break;
                case "5":
                    ListActiveEvents(eventService);
                    break;
                case "6":
                    ChangeEventStatusMenu(eventService);
                    break;
                case "7":
                    RecursiveListMenu(eventService);
                    break;
                case "8":
                    ChangeParticipantStatusMenu(eventService);
                    break;
                case "9":
                    ListParticipants(eventService);
                    break;
                case "0":
                    isRunning = false;
                    Console.WriteLine("Programdan çıkılıyor...");
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim. Lütfen menüdeki değerlerden birini giriniz.");
                    break;
            }
        }
        catch (CapacityFullException exception)
        {
            Console.WriteLine($"Kontenjan Hatası: {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Hata: {exception.Message}");
        }

        if (isRunning)
        {
            Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
            Console.ReadKey();
        }
    }
}

static void PrintMenu()
{
    Console.Clear();
    Console.WriteLine("==========================================");
    Console.WriteLine("     Akıllı Etkinlik Katılım Sistemi");
    Console.WriteLine("==========================================");
    Console.WriteLine("1- Etkinlik Ekle");
    Console.WriteLine("2- Katılımcı Ekle");
    Console.WriteLine("3- Etkinlikleri Listele");
    Console.WriteLine("4- Kategoriye Göre Filtrele");
    Console.WriteLine("5- Aktif Etkinlikleri Listele");
    Console.WriteLine("6- Etkinlik Durumunu Değiştir");
    Console.WriteLine("7- Recursive Etkinlik Listeleme");
    Console.WriteLine("8- Katılım Durumu Güncelle");
    Console.WriteLine("9- Katılımcıları Listele");
    Console.WriteLine("0- Çıkış");
    Console.WriteLine("==========================================");
}

static void AddEventMenu(EventService eventService)
{
    Console.WriteLine("\n--- Etkinlik Ekle ---");

    string name = ReadRequiredText("Etkinlik adı: ");
    EventCategory category = ReadCategory();
    int capacity = ReadInt("Kontenjan: ", minValue: 1);
    DateOnly date = ReadDate("Tarih (gg.aa.yyyy): ");

    UniversityEvent addedEvent = eventService.AddEvent(name, category, capacity, date);

    Console.WriteLine("\nEtkinlik başarıyla eklendi.");
    Console.WriteLine(eventService.FormatEvent(addedEvent));
}

static void AddParticipantMenu(EventService eventService)
{
    Console.WriteLine("\n--- Katılımcı Ekle ---");

    Guid? selectedEventId = SelectEvent(eventService);

    if (selectedEventId is null)
    {
        Console.WriteLine("Katılımcı eklemek için önce etkinlik eklenmelidir.");
        return;
    }

    string studentName = ReadRequiredText("Öğrenci adı: ");
    string studentNumber = ReadRequiredText("Öğrenci numarası: ");

    Participant participant = eventService.AddParticipant(selectedEventId.Value, studentName, studentNumber);

    Console.WriteLine("\nKatılımcı başarıyla eklendi.");
    Console.WriteLine(eventService.FormatParticipant(participant));
}

static void ListEvents(IReadOnlyList<UniversityEvent> events, EventService eventService)
{
    Console.WriteLine("\n--- Etkinlik Listesi ---");

    if (events.Count == 0)
    {
        Console.WriteLine("Kayıtlı etkinlik bulunmamaktadır.");
        return;
    }

    for (int i = 0; i < events.Count; i++)
    {
        UniversityEvent eventItem = events[i];
        Console.WriteLine($"{i + 1}. {eventService.FormatEvent(eventItem)}");
        Console.WriteLine($"   Açıklama: {eventService.GetCategoryMessage(eventItem)}");
    }
}

static void FilterByCategoryMenu(EventService eventService)
{
    Console.WriteLine("\n--- Kategoriye Göre Filtreleme ---");
    EventCategory category = ReadCategory();

    // Lambda expression + higher-order function birlikte kullanılmıştır.
    IReadOnlyList<UniversityEvent> filteredEvents = eventService.FilterEvents(eventItem => eventItem.Category == category);
    ListEvents(filteredEvents, eventService);
}

static void ListActiveEvents(EventService eventService)
{
    Console.WriteLine("\n--- Aktif Etkinlikler ---");

    // Higher-order function: Filtreleme koşulu metot dışından gönderilir.
    IReadOnlyList<UniversityEvent> activeEvents = eventService.FilterEvents(eventItem => eventItem.IsActive);
    ListEvents(activeEvents, eventService);
}

static void ChangeEventStatusMenu(EventService eventService)
{
    Console.WriteLine("\n--- Etkinlik Durumu Değiştir ---");

    Guid? selectedEventId = SelectEvent(eventService);

    if (selectedEventId is null)
    {
        Console.WriteLine("Durumu değiştirilecek etkinlik bulunmamaktadır.");
        return;
    }

    Console.WriteLine("1- Aktif yap");
    Console.WriteLine("2- Pasif yap");
    int choice = ReadInt("Seçiminiz: ", minValue: 1, maxValue: 2);

    bool newStatus = choice == 1;
    eventService.ChangeEventStatus(selectedEventId.Value, newStatus);

    Console.WriteLine("Etkinlik durumu başarıyla güncellendi.");
}

static void RecursiveListMenu(EventService eventService)
{
    Console.WriteLine("\n--- Recursive Etkinlik Listeleme ---");

    IReadOnlyList<UniversityEvent> events = eventService.GetAllEvents();
    List<string> recursiveList = eventService.GetRecursiveEventList(events);

    if (recursiveList.Count == 0)
    {
        Console.WriteLine("Kayıtlı etkinlik bulunmamaktadır.");
        return;
    }

    for (int i = 0; i < recursiveList.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {recursiveList[i]}");
    }
}

static void ChangeParticipantStatusMenu(EventService eventService)
{
    Console.WriteLine("\n--- Katılım Durumu Güncelle ---");

    IReadOnlyList<Participant> participants = eventService.GetAllParticipants();

    if (participants.Count == 0)
    {
        Console.WriteLine("Kayıtlı katılımcı bulunmamaktadır.");
        return;
    }

    for (int i = 0; i < participants.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {eventService.FormatParticipant(participants[i])}");
    }

    int selectedParticipantIndex = ReadInt("Katılımcı seçiniz: ", 1, participants.Count) - 1;
    AttendanceStatus newStatus = ReadAttendanceStatus();

    bool result = eventService.ChangeParticipantStatus(participants[selectedParticipantIndex].Id, newStatus);

    Console.WriteLine(result
        ? "Katılım durumu başarıyla güncellendi."
        : "Katılımcı durumu güncellenemedi.");
}

static void ListParticipants(EventService eventService)
{
    Console.WriteLine("\n--- Katılımcı Listesi ---");

    IReadOnlyList<Participant> participants = eventService.GetAllParticipants();

    if (participants.Count == 0)
    {
        Console.WriteLine("Kayıtlı katılımcı bulunmamaktadır.");
        return;
    }

    foreach (Participant participant in participants)
    {
        Console.WriteLine(eventService.FormatParticipant(participant));
    }
}

static Guid? SelectEvent(EventService eventService)
{
    IReadOnlyList<UniversityEvent> events = eventService.GetAllEvents();

    if (events.Count == 0)
    {
        return null;
    }

    for (int i = 0; i < events.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {eventService.FormatEvent(events[i])}");
    }

    int selectedIndex = ReadInt("Etkinlik seçiniz: ", 1, events.Count) - 1;
    return events[selectedIndex].Id;
}

static EventCategory ReadCategory()
{
    Console.WriteLine("Kategori seçiniz:");
    Console.WriteLine("1- Akademik");
    Console.WriteLine("2- Teknoloji");
    Console.WriteLine("3- Kültür");
    Console.WriteLine("4- Spor");
    Console.WriteLine("5- Kariyer");
    Console.WriteLine("6- Diğer");

    int choice = ReadInt("Kategori: ", minValue: 1, maxValue: 6);
    return (EventCategory)choice;
}

static AttendanceStatus ReadAttendanceStatus()
{
    Console.WriteLine("Katılım durumu seçiniz:");
    Console.WriteLine("1- Kayıtlı");
    Console.WriteLine("2- Katıldı");
    Console.WriteLine("3- Katılmadı");
    Console.WriteLine("4- İptal Edildi");

    int choice = ReadInt("Durum: ", minValue: 1, maxValue: 4);
    return (AttendanceStatus)choice;
}

static string ReadRequiredText(string message)
{
    while (true)
    {
        Console.Write(message);
        string? value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        Console.WriteLine("Bu alan boş bırakılamaz.");
    }
}

static int ReadInt(string message, int minValue = int.MinValue, int maxValue = int.MaxValue)
{
    while (true)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number) && number >= minValue && number <= maxValue)
        {
            return number;
        }

        Console.WriteLine($"Lütfen {minValue} ile {maxValue} arasında geçerli bir sayı giriniz.");
    }
}

static DateOnly ReadDate(string message)
{
    while (true)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (DateOnly.TryParseExact(input, "dd.MM.yyyy", out DateOnly date))
        {
            return date;
        }

        Console.WriteLine("Lütfen tarihi gg.aa.yyyy formatında giriniz. Örnek: 20.05.2026");
    }
}

static void SeedSampleData(EventService eventService)
{
    UniversityEvent aiEvent = eventService.AddEvent(
        "Yapay Zekâ ve Kariyer Söyleşisi",
        EventCategory.Technology,
        2,
        new DateOnly(2026, 5, 20));

    UniversityEvent cultureEvent = eventService.AddEvent(
        "Bahar Dönemi Kültür Buluşması",
        EventCategory.Culture,
        3,
        new DateOnly(2026, 5, 25));

    eventService.AddParticipant(aiEvent.Id, "Ayşe Demir", "20230001");
    eventService.AddParticipant(cultureEvent.Id, "Mehmet Yılmaz", "20230002");
}
