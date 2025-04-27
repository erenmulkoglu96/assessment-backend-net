# 📖 PhoneBook Microservices Backend Assessment

Bu proje, bir telefon rehberi sistemini mikroservisler mimarisi ile geliştirmek amacıyla .NET Core kullanılarak hazırlanmıştır.


🛠 Kullanılan Teknolojiler:

- .NET Core 9
- PostgreSQL (Docker üzerinden çalıştırıldı)
- Apache Kafka (Docker üzerinden çalıştırıldı)
- Entity Framework Core
- xUnit (Unit Test)
- Git & GitHub


📋 Projenin Özellikleri:

- Rehberde kişi oluşturma
- Rehberden kişi kaldırma
- Rehberdeki kişiye iletişim bilgisi ekleme
- Rehberdeki kişiden iletişim bilgisi kaldırma
- Rehberdeki kişilerin listelenmesi
- Kişi detaylarının iletişim bilgileri ile birlikte getirilmesi
- Konuma göre rapor talep edilmesi
- Oluşturulan raporların listelenmesi
- Bir raporun detay bilgilerinin getirilmesi
- Raporlar hem JSON hem CSV formatında oluşturulabilir ve indirilebilir


### 📚 Veri Yapıları

| Alan          | Açıklama                                    |
| ------------- | ------------------------------------------- |
| **Kişi (Person)**    | Kişiye özel bilgiler                        |
| UUID          | Kişiye özel benzersiz ID                    |
| FirstName     | İsim                                        |
| LastName      | Soyisim                                     |
| Company       | Çalıştığı Şirket                            |
| ContactInfos       | 	İletişim Bilgileri                            |
| **İletişim Bilgisi (ContactInfo)** | İletişim Bilgileri                        |
| Type          | Bilgi Tipi (Telefon Numarası, E-mail, Konum) |
| Content       | Bilgi İçeriği (Numara, E-posta, Şehir)       |
| **Rapor (Report)**    | Rapor Bilgileri                             |
| UUID          | Raporun benzersiz ID'si                     |
| RequestedAt   | Raporun talep edildiği zaman                |
| Status        | Raporun Durumu (Preparing veya Completed)   |
| CompletedAt   | Raporun tamamlanma zamanı                   |
| FilePath      | JSON dosya yolu                             |
| CsvPath       | CSV dosya yolu                              |



🛠 Teknik Gereksinimler ve Durum:

| İstenilen Gereksinim          | Durum                                    |
| ------------- | ------------------------------------------- |
|Projenin sık commitlerle geliştirilmesi |  Yapıldı
|Git üzerinde master, development branch kullanımı |  Yapıldı
|Git üzerinde sürüm taglemesi (v1.0.0) |  Yapıldı
|Minimum %60 unit testing code coverage |  Yapıldı
|Migration yapısının oluşturulması |  Yapıldı
|README.md dökümantasyonu oluşturulması |  Yapıldı
|Servislerin REST API ile iletişim kuruyor, HTTP REST üzerinden haberleşmesi |  Yapıldı
|Rapor kısmında Kafka ile asenkron yapı kullanımı |  Yapıldı


⚙️ Projenin Çalıştırılması:

1. Gerekli bağımlılıkların yüklenmesi
### dotnet restore

3. PostgreSQL ve Kafka'nın Docker üzerinden çalıştırılması
docker-compose -f docker-compose.yml -f docker-compose.kafka.yml up -d

4. Veritabanı Migration işleminin yapılması
dotnet ef database update -p ReportService -s ReportService

5. Uygulamanın Başlatılması
dotnet run --project ReportService

6. Swagger Üzerinden Test Etmek
https://localhost:44393/swagger


🧪 Unit Test Çalıştırmak:

Projede yazılan xUnit testlerini çalıştırmak için:
dotnet test

🚀 API Özellikleri:

POST /api/person : Yeni kişi ekler

DELETE /api/person/{id} : Kişiyi siler

POST /api/person/{personId}/contactinfo : Kişiye iletişim bilgisi ekler

DELETE /api/contactinfo/{id} : İletişim bilgisini siler

GET /api/person : Tüm kişileri listeler

GET /api/person/{id} : Bir kişinin iletişim detaylarını getirir

POST /api/report : Rapor talebi başlatır (asenkron çalışır)

GET /api/report : Tüm raporları listeler

GET /api/report/{id} : Bir raporu ve detaylarını getirir

GET /api/report/{id}/download : JSON rapor dosyasını indirir

GET /api/report/{id}/download/csv : CSV rapor dosyasını indirir

DELETE /api/report/{id} : Raporu ve dosyaları siler


🛡️ Ekstra Özellikler:

- Swagger UI üzerinden test yapılabilir
- JSON ve CSV dosyaları wwwroot/reports klasörüne otomatik kaydedilir
- Arka planda Kafka consumer çalışarak asenkron rapor üretimi sağlar

📢 Not:

Bu proje, teknik değerlendirme süreci için sıfırdan inşa edilerek hazırlanmış bir mimaridir. 
