JWT tokenları ve rol bazlı erişim kontrolü ile kullanıcı kimlik doğrulaması ve yetkilendirmesini uygulamak üzere api içinde kullanılacak geliştirmeleri yap.
Kimlik Doğrulama: JWT (JSON Web Token) tabanlı.
Başarılı girişte bir JWT oluştur.
Korunan endpoint'lerde Microsoft.AspNetCore.Authentication.JwtBearer kullanarak JWT'leri doğrula.
Kullanıcı Yönetimi Özellikleri:
Kullanıcı Kayıt endpoint'i (/api/auth/register) - Username ve Password kabul etsin.
Kullanıcı Giriş endpoint'i (/api/auth/login) - Username ve Password kabul etsin, başarı durumunda bir JWT döndürsün.
Yetkilendirme: Rol Bazlı Erişim Kontrolü (RBAC).
En az iki rol uygula: "Admin" ve "User".
Kullanıcının rolünü JWT taleplerine (claims) ClaimTypes.Role kullanarak ekle.
Endpoint'leri [Authorize] ve [Authorize(Roles = "...")] niteliklerini (attributes) kullanarak koru.
Gerekli DbContext kurulumunu ve migration oluşturma komutlarını dahil et.

Şifre Hashleme: Şifreleri saklamadan önce hashlemek için BCrypt (BCrypt.Net-Next paketi) gibi güçlü bir hash algoritması kullan. Salt oluşturma ve doğrulama mantığını dahil et. Şifreleri asla düz metin olarak saklama.

JWT Gizli Anahtarı: JWT ayarlarını (Issuer, Audience, Secret Key) appsettings.json üzerinden yapılandır. Kod/yapılandırma örneklerinde Gizli Anahtar için bir yer tutucu (placeholder) kullan ve production ortamında mutlaka güvenli bir şekilde saklanması gerektiğini (örn: User Secrets, Ortam Değişkenleri, Azure Key Vault) vurgulayan bir yorum ekle.

Proje Yapısı ve Bileşenler: Aşağıdaki bileşenler için kod oluştur:
User Modeli: Id (int), Username (string), PasswordHash (byte[]), PasswordSalt (byte[]), Role (string) içersin.
DataContext Sınıfı: DbContext'ten miras alsın, DbSet<User> içersin.
Veri Transfer Nesneleri (DTOs):
UserRegisterDto (string Username, string Password)
UserLoginDto (string Username, string Password)
LoginResponseDto (string Token) - İsteğe bağlı, token doğrudan da döndürülebilir
IAuthService Arayüzü & AuthService Sınıfı:
Task<User> Register(UserRegisterDto request) metodu (hashleme, kaydetme işlemlerini yapsın).
Task<string> Login(UserLoginDto request) metodu (kullanıcı bulma, şifre doğrulama, rol claim'i dahil JWT oluşturma işlemlerini yapsın).
DataContext ve IConfiguration inject edilsin.
AuthController:
IAuthService inject edilsin.
[HttpPost("register")] endpoint'i.
[HttpPost("login")] endpoint'i.

Çıktı Formatı: İstenen her bileşen (Model, DTO'lar, Servis Arayüzü/Sınıfı, Controller'lar, Program.cs) için anlaşılır C# kod parçacıkları sağla. Gerekli using ifadelerini ekle. Gerektiğinde önemli kod bölümlerinin amacını kısaca açıkla.

Gerekli NuGet Paketleri: Projenize şu paketleri eklemeniz gerekecek:

Microsoft.AspNetCore.Authentication.JwtBearer: JWT tokenlarını doğrulamak için temel paket.
Microsoft.EntityFrameworkCore.SqlServer (veya seçtiğiniz veritabanı sağlayıcısı, örn: Npgsql.EntityFrameworkCore.PostgreSQL, Microsoft.EntityFrameworkCore.Sqlite): Veritabanı işlemleri için.
Microsoft.EntityFrameworkCore.Tools: Entity Framework Core migration işlemleri için.
System.IdentityModel.Tokens.Jwt: JWT token oluşturmak için (genellikle JwtBearer ile birlikte gelir ama kontrol etmekte fayda var).
Opsiyonel: BCrypt.Net-Next veya benzeri bir kütüphane: Güvenli şifre hashleme için.

