# Akıllı Etkinlik Katılım Sistemi

Bu proje, Programlama Dilleri dersi kapsamında modern dil özelliklerini göstermek amacıyla hazırlanmış C# konsol uygulamasıdır. Uygulama, bir üniversitede düzenlenen etkinliklere öğrencilerin kayıt olabildiği mini bir etkinlik katılım sistemi olarak tasarlanmıştır.

## Projenin Amacı

Uygulamanın amacı büyük ölçekli bir otomasyon geliştirmek değil; C# dilinde bulunan modern programlama özelliklerini doğru yerde kullanarak anlaşılır bir sistem oluşturmaktır.

Sistem şu işlemleri yapar:

- Etkinlik ekleme
- Katılımcı ekleme
- Kontenjan kontrolü
- Etkinlik listeleme
- Kategoriye göre filtreleme
- Aktif / pasif etkinlik yönetimi
- Katılım durumu yönetimi
- Recursive etkinlik listeleme

## Çalıştırma

Bilgisayarda .NET SDK yüklü olmalıdır.

```bash
dotnet run
```

Komut proje klasörünün içindeyken çalıştırılmalıdır.

## Proje Klasör Yapısı

```text
AkilliEtkinlikKatilimSistemi
│
├── Program.cs
├── AkilliEtkinlikKatilimSistemi.csproj
│
├── Enums
│   ├── AttendanceStatus.cs
│   └── EventCategory.cs
│
├── Exceptions
│   └── CapacityFullException.cs
│
├── Models
│   ├── BaseEvent.cs
│   ├── IEntity.cs
│   ├── Participant.cs
│   └── UniversityEvent.cs
│
└── Services
    ├── EventService.cs
    ├── GenericRepository.cs
    └── IRepository.cs
```

## Kullanılan Modern Dil Özellikleri

| Özellik | Projede Kullanım Yeri |
|---|---|
| Recursion | `GetRecursiveEventList` metodu ile etkinliklerin recursive listelenmesi |
| Lambda Expression | `Where`, `OrderBy`, `Any`, `Count` gibi LINQ işlemlerinde kullanıldı |
| Generic Type | `GenericRepository<T>` sınıfı etkinlik ve katılımcı için ortak kullanıldı |
| Pattern Matching | `GetCategoryMessage` metodunda etkinlik kategorisine göre açıklama üretildi |
| Exception Handling | Kontenjan dolduğunda `CapacityFullException` fırlatıldı ve yakalandı |
| Immutable Yapı | Etkinlik modeli `record` olarak tanımlandı, güncellemelerde `with` kullanıldı |
| Interface / Abstract Class | `IRepository<T>`, `IEntity` ve `BaseEvent` kullanıldı |
| Higher-Order Function | `FilterEvents(Func<UniversityEvent, bool> condition)` metodu kullanıldı |

## Menü Yapısı

Program açıldığında kullanıcıya şu seçenekler sunulur:

```text
1- Etkinlik Ekle
2- Katılımcı Ekle
3- Etkinlikleri Listele
4- Kategoriye Göre Filtrele
5- Aktif Etkinlikleri Listele
6- Etkinlik Durumunu Değiştir
7- Recursive Etkinlik Listeleme
8- Katılım Durumu Güncelle
9- Katılımcıları Listele
0- Çıkış
```

## Not

Projede örnek veri eklenmiştir. Program ilk açıldığında iki örnek etkinlik ve iki örnek katılımcı otomatik olarak gelir. Bu sayede menüler doğrudan test edilebilir.
