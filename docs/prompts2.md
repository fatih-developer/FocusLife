# Kişisel Yönetim Sistemi Projesi için Yapay Zeka ile Kod Yazma Odaklı Adım Adım Geliştirme Prompt Listesi (Kesin İstekler)

**Aşama 1: Proje Temelleri ve Çekirdek Modüller**

1.  **Prompt 1 (Proje Yapısı Oluştur):** "Yüklediğim 'Temiz Mimari ve CQRS Kullanarak Kişisel Yönetim Uygulaması için Mimari Plan' belgesine dayanarak, backend (.NET 9.0, C#) uygulaması için temel proje yapısını (klasörler ve temel dosyalar) oluştur. Temiz Mimari katmanları ve CQRS için gerekli klasörleri dahil et."
2.  **Prompt 2 (Görev Takibi - Veri Modeli Geliştir):** "Görev takibi modülü için C# kullanarak temel veri modelini (entity) geliştir. `Başlık`, `Açıklama`, `BitişTarihi`, `Öncelik`, `Durum` ve ilgili diğer özellikleri içeren bir `Gorev` sınıfı yaz."
3.  **Prompt 3 (Görev Takibi - CQRS DTO'larını Tanımla):** "CQRS prensiplerine uygun olarak, görev takibi modülü için C# kullanarak komut (oluşturma, güncelleme, silme) ve sorgu (listeleme, ID'ye göre getirme) DTO'larını (Data Transfer Objects) tanımla."

**Halüsinasyon Önleme ve Doğrulama (Aşama 1)**

4.  **Prompt 4:** "Oluşturulan proje yapısının 'Temiz Mimari ve CQRS Kullanarak Kişisel Yönetim Uygulaması için Mimari Plan' belgesindeki önerilerle ne kadar uyumlu olduğunu değerlendir. Eksik veya yanlış bir şey var mı, kontrol et."
5.  **Prompt 5:** "Tanımlanan C# veri modeli ve DTO'ların, görev takibi gereksinimlerini eksiksiz karşılayıp karşılamadığını kontrol et. Daha iyi bir modelleme için önerilerin varsa sun."

**Aşama 2: Özelliklerin Uygulanması ve İş Mantığı (Backend)**

**(Her bir özellik için (not alma, takvim, bütçe vb.) benzer promptlar oluşturulacaktır.)**

6.  **Prompt 6 (Görev Takibi - Komut İşleyicileri Geliştir):** "C# kullanarak görev takibi modülü için komut işleyicilerini (örneğin, `YeniGorevOlusturKomutuHandler`, `GorevGuncelleKomutuHandler`, `GorevSilKomutuHandler`) geliştir. Bu işleyicilerin domain entity'leri ve potansiyel bir repository interface'i ile nasıl etkileşimde bulunacağını gösteren kodu yaz."
7.  **Prompt 7 (Görev Takibi - Repository Interface Tanımla):** "Görev takibi modülü için C# kullanarak (örneğin :`IGorevRepository`) adında bir repository interface'i tanımla. Görevleri kaydetme, getirme ve silme metotlarını ekle."
8.  **Prompt 8 (Görev Takibi - Repository Uygulaması Yap):** "Test amaçlı basit bir in-memory veya SQLite tabanlı `IGorevRepository` implementasyonunu C# kullanarak yap."
9.  **Prompt 9 (Görev Takibi - Sorgu İşleyicileri Uygula):** "C# kullanarak farklı kriterlere göre (örneğin, tüm görevleri getirme, ID'ye göre görev getirme) görevleri getirecek sorgu işleyicilerini (örneğin, `TumGorevleriGetirSorguHandler`, `IdyeGorevGetirSorguHandler`) uygula."
10. **Prompt 10 (Görev Takibi - API Entegrasyonu (Backend)):** "Backend API'sine (RESTful varsayılır) görev oluşturma, güncelleme, silme ve getirme isteklerini işleyecek ASP.NET Core controller kodunu C# kullanarak yaz."

**Halüsinasyon Önleme ve Doğrulama (Aşama 2)**

11. **Prompt 11:** "Oluşturulan C# komut ve sorgu işleyicilerinin, Temiz Mimari prensiplerine ve CQRS desenine uygun olarak iş mantığını doğru bir şekilde uygulayıp uygulamadığını incele. Herhangi bir potansiyel sorun veya eksiklik var mı, kontrol et."
12. **Prompt 12:** "Tanımlanan repository interface'i ve implementasyonunun, veri erişimini doğru şekilde soyutlayıp soyutlamadığını kontrol et. Veritabanı işlemlerini nasıl ele aldığını incele."
13. **Prompt 13:** "Yazılan ASP.NET Core controller'ının doğru HTTP metotlarını kullandığını ve istekleri doğru şekilde işlediğini doğrula."

**Aşama 3: Gelişmiş Özellikler ve Entegrasyonlar (Backend)**

14. **Prompt 14 (Belge Analizi - Kütüphane Öner ve Fonksiyon Yaz):** "Yüklenen belgelerden temel metin çıkarmak için kullanılabilecek bir Python kütüphanesi öner ve bir dosya yolunu girdi olarak alıp çıkarılan metni döndüren basit bir Python fonksiyonu yaz."
15. **Prompt 15 (Belge Analizi - Bilgi Çıkarma Uygula):** "Çıkarılan metinden potansiyel görevleri, notları veya takvim etkinliklerini belirlemek için basit bir anahtar kelime tabanlı yaklaşımı Python kullanarak uygula. Örnek kod sağla."
16. **Prompt 16 (Şablonlar - Yöntem Öner ve Örnek Yapı Sağla):** "Özelleştirilebilir şablonları (örneğin, JSON formatında) depolamak ve yönetmek için bir yöntem öner. Basit bir not şablonu için örnek bir JSON yapısı sağla."

**Halüsinasyon Önleme ve Doğrulama (Aşama 3)**

17. **Prompt 17:** "Önerilen Python kütüphanesinin belge analizi için uygun ve güvenilir olduğunu doğrula. Alternatifler var mı, araştır ve hangisinin projenin ihtiyaçlarına daha uygun olabileceğini değerlendir."
18. **Prompt 18:** "Şablon yönetimi için önerilen yaklaşımın esnek ve kullanıcı dostu olup olmadığını değerlendir. JSON formatı şablonlar için en uygun seçenek mi, alternatifleri araştır."

**Aşama 4: Raporlama ve Veri Görselleştirme (Backend)**

19. **Prompt 19 (Raporlama Yap):** "Python kullanarak, son bir haftada tamamlanan görev sayısını gösteren basit bir raporu (örneğin, düz metin veya CSV formatında) yap."

**Halüsinasyon Önleme ve Doğrulama (Aşama 4)**

20. **Prompt 20:** "Oluşturulan raporlama yönteminin temel gereksinimleri karşılayıp karşılamadığını değerlendir. Daha gelişmiş raporlama seçenekleri neler olabilir, araştır."

**Aşama 5: API Tasarımı**

21. **Prompt 21 (API Tasarla):** "ASP.NET Core kullanarak görev takibi modülü için temel REST API endpoint'lerini (oluşturma, güncelleme, silme, getirme) detaylı olarak tasarla. İstek ve yanıt formatlarını (JSON) belirt."

**Halüsinasyon Önleme ve Doğrulama (Aşama 5)**

22. **Prompt 22:** "Tasarlanan API endpoint'lerinin RESTful prensiplerine uygun olup olmadığını ve beklenen işlevselliği sağlayıp sağlamadığını kontrol et."

**Aşama 6: Flutter Uygulamasını Geliştirme**

23. **Prompt 23 (Flutter - Proje Yapısı Oluştur):** "Flutter uygulaması için temel proje yapısını (klasörler ve temel dosyalar) oluştur."
24. **Prompt 24 (Flutter - Görev Takibi UI Taslağı Oluştur):** "Flutter kullanarak, görev listesini görüntüleyecek ve yeni görev oluşturma/düzenleme ekranlarını içerecek temel bir kullanıcı arayüzü (UI) taslağı için Flutter widget kod örnekleri sağla."
25. **Prompt 25 (Flutter - API Entegrasyonu Yap):** "Backend API'sine (önceki aşamalarda tasarlanan) görev oluşturma, güncelleme, silme ve getirme isteklerini göndermek için Flutter'da örnek kod yaz."
26. **Prompt 26 (Flutter - Tema İşlevselliği Uygula):** "Flutter'da temel tema değiştirme işlevselliğini (örneğin, açık ve koyu temalar) uygula. Örnek Flutter kodu yaz."
27. **Prompt 27 (Flutter - Veri Görselleştirme Uygula):** "Flutter'da temel grafikler (örneğin, bütçe dağılımı için pasta grafik) görüntülemek için kullanılabilecek bir kütüphane kullanarak örnek bir görselleştirme uygula."

**Halüsinasyon Önleme ve Doğrulama (Aşama 6)**

28. **Prompt 28:** "Oluşturulan Flutter proje yapısının standart Flutter geliştirme prensiplerine uygun olup olmadığını kontrol et."
29. **Prompt 29:** "Flutter UI taslağının kullanıcı dostu olup olmadığını ve temel görev takibi işlevlerini destekleyip desteklemediğini değerlendir."
30. **Prompt 30:** "Flutter API entegrasyon kodunun doğru HTTP metotlarını kullandığını ve backend API'si ile doğru şekilde iletişim kurduğunu doğrula."
31. **Prompt 31:** "Uygulanan tema değiştirme ve veri görselleştirme işlevselliklerinin beklendiği gibi çalışıp çalışmadığını test et."