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


📚 Veri Yapıları
Kişi (Person)

Alan	- Açıklama

UUID	Kişiye özel benzersiz ID

FirstName	İsim

LastName	Soyisim

Company	Çalıştığı şirket

ContactInfos	İletişim Bilgileri

İletişim Bilgisi (ContactInfo)

Alan	Açıklama

Type	Telefon Numarası, E-mail veya Konum

Content	Bilgi içeriği (ör. numara, email, şehir)

Rapor (Report)

Alan	Açıklama

UUID	Raporun ID'si

RequestedAt	Raporun talep edildiği zaman

Status	Hazırlanıyor veya Tamamlandı

CompletedAt	Rapor tamamlanma tarihi

FilePath	JSON dosya yolu

CsvPath	CSV dosya yolu



🛠 Teknik Gereksinimler ve Durum:

İstenilen Gereksinim									Durum

Projenin sık commitlerle geliştirilmesi					Yapıldı

Git üzerinde master, development branch yapısı			Yapıldı

Git üzerinde sürüm taglemesi (v1.0.0)					Yapıldı

Minimum %60 unit testing coverage						Yapıldı

Veritabanı Migration yapısı oluşturuldu					Yapıldı

README.md hazırlanması									Yapıldı

Servisler REST API ile iletişim kuruyor					Yapıldı

Raporlar Kafka ile asenkron hazırlanıyor				Yapıldı



⚙️ Projenin Çalıştırılması:

1. Gerekli bağımlılıkların yüklenmesi
dotnet restore

2. PostgreSQL ve Kafka'nın Docker üzerinden çalıştırılması
docker-compose -f docker-compose.yml -f docker-compose.kafka.yml up -d

3. Veritabanı Migration işleminin yapılması
dotnet ef database update -p ReportService -s ReportService

4. Uygulamanın Başlatılması
dotnet run --project ReportService

5. Swagger Üzerinden Test Etmek
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
