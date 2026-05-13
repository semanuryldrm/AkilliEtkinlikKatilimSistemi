[README (4).md](https://github.com/user-attachments/files/27714631/README.4.md)# Akıllı Etkinlik Katılım Sistemi

**Akıllı Etkinlik Katılım Sistemi**, Programlama Dilleri dersi kapsamında C# programlama dili ile geliştirilmiş konsol tabanlı bir mini etkinlik katılım uygulamasıdır. Proje, bir üniversitede düzenlenen etkinliklere öğrencilerin kayıt olabildiği, etkinliklerin listelenebildiği, kontenjan kontrolünün yapılabildiği ve katılım durumlarının yönetilebildiği sade bir sistem olarak hazırlanmıştır.

Bu projenin temel amacı büyük ölçekli bir otomasyon geliştirmek değil; C# dilinde yer alan modern programlama dili özelliklerini doğru yerde, anlaşılır ve açıklanabilir şekilde kullanmaktır.

---

## İçindekiler

- [Projenin Amacı](#projenin-amacı)
- [Uygulama Özellikleri](#uygulama-özellikleri)
- [Kullanılan Teknolojiler](#kullanılan-teknolojiler)
- [Kullanılan Modern Dil Özellikleri](#kullanılan-modern-dil-özellikleri)
- [Proje Klasör Yapısı](#proje-klasör-yapısı)
- [Menü Yapısı](#menü-yapısı)
- [Kurulum ve Çalıştırma](#kurulum-ve-çalıştırma)
- [Uygulama Mantığı](#uygulama-mantığı)
- [Örnek Kullanım Senaryosu](#örnek-kullanım-senaryosu)
- [Ders Kapsamındaki Önemi](#ders-kapsamındaki-önemi)
- [Geliştirilebilir Yönler](#geliştirilebilir-yönler)
- [Lisans](#lisans)
- [Geliştirici](#geliştirici)

---

## Projenin Amacı

Bu projenin amacı, üniversite içinde düzenlenen etkinliklere öğrencilerin kayıt olabildiği küçük ölçekli bir etkinlik katılım sistemi geliştirmektir. Sistem üzerinden etkinlik ekleme, katılımcı kaydı oluşturma, kontenjan kontrolü yapma, etkinlikleri listeleme, filtreleme ve katılım durumu yönetimi gibi temel işlemler gerçekleştirilebilmektedir.

Proje geliştirilirken yalnızca çalışan bir konsol uygulaması oluşturmak hedeflenmemiştir. Aynı zamanda Programlama Dilleri dersi kapsamında istenen modern dil özelliklerinin uygulama içinde doğru yerlerde kullanılması amaçlanmıştır. Bu nedenle proje içinde `interface`, `abstract record`, `generic type`, `lambda expression`, `higher-order function`, `exception handling`, `pattern matching`, `recursion` ve `immutable yapı` gibi özelliklere yer verilmiştir.

---

## Uygulama Özellikleri

Uygulama aşağıdaki temel işlemleri desteklemektedir:

- Etkinlik ekleme
- Katılımcı ekleme
- Kontenjan kontrolü
- Etkinlikleri listeleme
- Kategoriye göre filtreleme
- Aktif etkinlikleri listeleme
- Etkinlik durumunu aktif veya pasif olarak değiştirme
- Recursive etkinlik listeleme
- Katılım durumu güncelleme
- Katılımcıları listeleme
- Hatalı girişleri kontrol etme
- Kontenjan dolu olduğunda özel hata mesajı gösterme

---

## Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|---|---|
| C# | Uygulamanın geliştirildiği programlama dilidir. |
| .NET 8 | Projenin çalıştırıldığı platformdur. |
| Console Application | Kullanıcı işlemleri konsol menüsü üzerinden yapılmaktadır. |
| Nesne Yönelimli Programlama | Model, servis, repository ve exception yapıları ayrı sorumluluklarla düzenlenmiştir. |

---

## Kullanılan Modern Dil Özellikleri

Projede Programlama Dilleri dersi kapsamında beklenen modern dil özellikleri uygulama içinde kullanılmıştır.

| Modern Dil Özelliği | Projede Kullanım Yeri |
|---|---|
| Interface | `IEntity` ve `IRepository<T>` yapıları ile ortak davranışlar tanımlanmıştır. |
| Abstract Class / Abstract Record | `BaseEvent` yapısı etkinlikler için temel model olarak kullanılmıştır. |
| Generic Type | `GenericRepository<T>` ile etkinlik ve katılımcı verileri ortak bir yapı üzerinden yönetilmiştir. |
| Lambda Expression | Filtreleme, sıralama ve kontrol işlemlerinde kullanılmıştır. |
| Higher-Order Function | `FilterEvents(Func<UniversityEvent, bool> condition)` metodu ile dışarıdan koşul alan dinamik filtreleme yapılmıştır. |
| Exception Handling | Kontenjan dolduğunda `CapacityFullException` ile özel hata yönetimi sağlanmıştır. |
| Pattern Matching | Etkinlik kategorisine ve aktiflik durumuna göre açıklama üretmek için kullanılmıştır. |
| Recursion | `GetRecursiveEventList` metodu ile etkinlikler recursive olarak listelenmiştir. |
| Immutable Yapı | `record` ve `with` kullanımıyla veriler kontrollü şekilde güncellenmiştir. |

---

## Proje Klasör Yapısı

```text
AkilliEtkinlikKatilimSistemi
│
├── AkilliEtkinlikKatilimSistemi.csproj
├── AkilliEtkinlikKatilimSistemi.sln
├── Program.cs
├── README.md
├── LICENSE
│
├── Enums
│   ├── AttendanceStatus.cs
│   └── EventCategory.cs
│
├── Exceptions
│   └── CapacityFullException.cs
│
├── Models
│   ├── IEntity.cs
│   ├── BaseEvent.cs
│   ├── UniversityEvent.cs
│   └── Participant.cs
│
└── Services
    ├── IRepository.cs
    ├── GenericRepository.cs
    └── EventService.cs
```

### Klasörlerin Görevleri

| Dosya / Klasör | Görevi |
|---|---|
| `Program.cs` | Konsol menüsünü, kullanıcı seçimlerini ve uygulamanın genel akışını yönetir. |
| `Enums` | Etkinlik kategorileri ve katılım durumları gibi sabit seçenekleri içerir. |
| `Exceptions` | Uygulamadaki özel hata sınıflarını içerir. |
| `Models` | Etkinlik ve katılımcı gibi temel veri modellerini içerir. |
| `Services` | Repository yapısı ve etkinlik işlemlerini yöneten servis sınıflarını içerir. |
| `README.md` | Proje hakkında açıklama, kurulum ve kullanım bilgilerini içerir. |
| `LICENSE` | Projenin lisans bilgisini içerir. |

---

## Menü Yapısı

Program çalıştırıldığında kullanıcıya aşağıdaki menü seçenekleri sunulur:

```text
==========================================
      Akıllı Etkinlik Katılım Sistemi
==========================================
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
==========================================
```

---

## Kurulum ve Çalıştırma

Projeyi çalıştırmak için bilgisayarda **.NET SDK** yüklü olmalıdır.

### 1. Repoyu klonlama

```bash
git clone https://github.com/semanuryldrm/AkilliEtkinlikKatilimSistemi.git
```

### 2. Proje klasörüne girme

```bash
cd AkilliEtkinlikKatilimSistemi
```

### 3. Projeyi çalıştırma

```bash
dotnet run
```

Alternatif olarak çözüm dosyası Visual Studio ile açılarak da proje çalıştırılabilir.

---

## Uygulama Mantığı

Uygulama konsol üzerinden çalışan sade bir menü sistemine sahiptir. Kullanıcı menüden yapmak istediği işlemi seçer ve seçime göre ilgili servis metotları çalıştırılır.

Etkinlik ekleme işleminde kullanıcıdan etkinlik adı, kategori, kontenjan ve tarih bilgileri alınır. Girilen bilgiler uygun ise yeni etkinlik sisteme eklenir. Etkinlik adı boş bırakılamaz ve kontenjan değeri sıfırdan büyük olmalıdır.

Katılımcı ekleme işleminde kullanıcı önce kayıt yapılacak etkinliği seçer. Daha sonra öğrenci adı ve öğrenci numarası girilir. Sistem, seçilen etkinliğin aktif olup olmadığını ve kontenjan durumunu kontrol eder. Eğer etkinlik pasifse veya kontenjan doluysa kayıt işlemi yapılmaz.

Kategoriye göre filtreleme işleminde kullanıcı bir kategori seçer ve sistem yalnızca o kategoriye ait etkinlikleri listeler. Aktif etkinlikleri listeleme işleminde ise sadece kayıt almaya uygun olan etkinlikler görüntülenir.

Katılım durumu güncelleme seçeneği ile bir katılımcının durumu `Kayıtlı`, `Katıldı`, `Katılmadı` veya `İptal Edildi` olarak değiştirilebilir. Katılımcı iptal edildiğinde kontenjan yönetimi de buna göre güncellenir.

---

## Örnek Kullanım Senaryosu

1. Kullanıcı programı başlatır.
2. Menüden `1- Etkinlik Ekle` seçeneğini seçer.
3. Etkinlik adı, kategori, kontenjan ve tarih bilgilerini girer.
4. Menüden `2- Katılımcı Ekle` seçeneği ile etkinliğe öğrenci kaydı yapar.
5. Sistem etkinliğin aktifliğini ve kontenjan durumunu kontrol eder.
6. Kontenjan uygunsa katılımcı sisteme eklenir.
7. Kullanıcı etkinlikleri listeleyebilir veya kategoriye göre filtreleyebilir.
8. Gerekirse katılım durumunu güncelleyebilir.

---

## Örnek Veriler

Program ilk çalıştırıldığında test amacıyla örnek etkinlik ve katılımcılar otomatik olarak sisteme eklenmektedir. Bu sayede uygulama menüleri doğrudan denenebilir.

Örnek etkinlikler:

- Yapay Zekâ ve Kariyer Söyleşisi
- Bahar Dönemi Kültür Buluşması

Örnek katılımcılar:

- Ayşe Demir
- Mehmet Yılmaz

---

## Ders Kapsamındaki Önemi

Bu proje, Programlama Dilleri dersinde öğrenilen modern programlama özelliklerinin küçük bir uygulama üzerinde nasıl kullanılabileceğini göstermek amacıyla hazırlanmıştır. Projede kullanılan yapılar yalnızca teorik olarak bırakılmamış, etkinlik kayıt sistemi içinde gerçek kullanım alanlarıyla uygulanmıştır.

Proje kapsamında özellikle aşağıdaki kazanımlar hedeflenmiştir:

- Modern C# özelliklerini uygulama içinde kullanmak
- Nesne yönelimli programlama mantığını pekiştirmek
- Kod tekrarını azaltan generic yapıları kullanmak
- Hata yönetimini kontrollü şekilde gerçekleştirmek
- Filtreleme işlemlerini lambda expression ve higher-order function ile yapmak
- Immutable veri yaklaşımını örneklemek
- Recursive metot kullanımını uygulama içinde göstermek

---

## Geliştirilebilir Yönler

Bu proje ders kapsamında mini bir konsol uygulaması olarak hazırlanmıştır. İlerleyen süreçte aşağıdaki geliştirmeler yapılabilir:

- Verilerin dosyaya kaydedilmesi
- Veritabanı bağlantısı eklenmesi
- Kullanıcı giriş sistemi oluşturulması
- Yönetici ve öğrenci rolleri eklenmesi
- Grafik arayüz geliştirilmesi
- Etkinlik raporlama ekranı eklenmesi
- Katılım listelerinin dosya olarak dışa aktarılması

---

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Ayrıntılar için `LICENSE` dosyasına bakabilirsiniz.

---

## Geliştirici

**Semanur Yıldırım**

Programlama Dilleri Dersi Projesi
