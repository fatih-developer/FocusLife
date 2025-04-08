# FocusLife+

FocusLife+, kullanıcıların günlük yaşamlarını daha verimli ve odaklanmış bir şekilde yönetmelerine yardımcı olan bir uygulamadır.

## Proje Yapısı

Proje, Clean Architecture prensiplerine uygun olarak geliştirilmiştir ve aşağıdaki katmanlardan oluşmaktadır:

- **FocusLifePlus.Domain**: İş mantığı ve temel varlıklar
- **FocusLifePlus.Application**: Uygulama servisleri ve iş akışları
- **FocusLifePlus.Infrastructure**: Veritabanı ve dış servis entegrasyonları
- **FocusLifePlus.API**: RESTful API katmanı
- **FocusLifePlus.Shared**: Tüm katmanlar tarafından kullanılan ortak bileşenler

## Geliştirme Ortamı Gereksinimleri

- .NET 8.0 SDK
- Visual Studio 2022 veya daha yeni bir sürüm
- SQL Server (LocalDB veya tam sürüm)

## Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/[kullanıcı-adınız]/FocusLifePlus.git
```

2. Proje dizinine gidin:
```bash
cd FocusLifePlus
```

3. Projeyi derleyin:
```bash
dotnet build
```

4. Veritabanını oluşturun:
```bash
cd src/FocusLifePlus.API
dotnet ef database update
```

5. API'yi çalıştırın:
```bash
dotnet run
```

## Test

Birim testlerini çalıştırmak için:

```bash
dotnet test
```

## Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Bir Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın. 