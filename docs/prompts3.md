# JWT Token ve Rol Bazlı Erişim Kontrolü İmplementasyonu Adımları

## 1. Temel Altyapı Kurulumu (Öncelik: Yüksek)
- [x] Gerekli NuGet paketlerinin yüklenmesi:
  - Microsoft.AspNetCore.Authentication.JwtBearer
  - System.IdentityModel.Tokens.Jwt
  - BCrypt.Net-Next

## 2. Role ve User Model Güncellemesi (Öncelik: Yüksek)
- [x] Role entity'sinin oluşturulması:
  - Id (Guid)
  - Name (string)
  - NormalizedName (string)
  - Description (string)
  - Permissions (string) - JSON formatında saklanacak (opsiyonel)
- [x] UserRole entity'sinin oluşturulması (many-to-many ilişki):
  - UserId (Guid)
  - RoleId (Guid)
- [x] Mevcut User modeline yeni alanların eklenmesi:
  - RefreshToken (string) - Opsiyonel
  - RefreshTokenExpiryTime (DateTime?) - Opsiyonel
- [x] Entity konfigürasyonlarının oluşturulması:
  - RoleConfiguration
  - UserRoleConfiguration
  - User konfigürasyonunun güncellenmesi
- [x] Seed data hazırlanması:
  - Admin rolü
  - User rolü
- [x] Migration oluşturulması ve uygulanması

## 2.1. Refresh Token Altyapısı (Öncelik: Yüksek)
- [x] RefreshToken entity'sinin oluşturulması:
  - Id (Guid)
  - Token (string)
  - UserId (Guid)
  - Created (DateTime)
  - Expires (DateTime)
  - Revoked (DateTime?)
  - ReplacedByToken (string?)
- [x] Entity konfigürasyonlarının oluşturulması:
  - RefreshTokenConfiguration
- [x] ApplicationDbContext güncellenmesi:
  - DbSet<RefreshToken> RefreshTokens
- [x] appsettings.json güncellenmesi:
  - Jwt:RefreshTokenTTL (gün cinsinden)
- [x] Migration oluşturulması ve uygulanması

## 3. DTO'ların Oluşturulması (Öncelik: Yüksek)
- [x] Application katmanında Contracts/Authentication klasörü oluşturulması
- [x] UserRegisterDto
  - Username (string)
  - Email (string)
  - Password (string)
  - FirstName (string)
  - LastName (string)
  - Roles (List<string>) - Varsayılan "User" rolü
- [x] UserLoginDto
  - Username (string)
  - Password (string)
- [x] AuthResponseDto
  - AccessToken (string)
  - RefreshToken (string)
  - Username (string)
  - Email (string)
  - Roles (List<string>)
- [x] RefreshTokenRequestDto
  - RefreshToken (string)

## 4. JWT Konfigürasyonu (Öncelik: Yüksek)
- [x] appsettings.json'da JWT ayarlarının yapılandırılması:
  - Issuer
  - Audience
  - Secret Key
  - TokenExpirationInMinutes
  - RefreshTokenTTL
- [x] Infrastructure katmanında JWT Options sınıfının oluşturulması
- [x] InfrastructureServiceRegistration.cs'de JWT servislerinin eklenmesi

## 5. Repository Pattern Güncellemesi (Öncelik: Orta)
- [x] IUserRepository arayüzünün oluşturulması:
  - Task<User> GetByRefreshTokenAsync(string refreshToken)
- [x] IRoleRepository arayüzünün oluşturulması
- [x] IRefreshTokenRepository arayüzünün oluşturulması:
  - Task<RefreshToken> GetByTokenAsync(string token)
  - Task RevokeAsync(string token, string replacedByToken = null)
  - Task AddAsync(RefreshToken refreshToken)
- [x] UserRepository sınıfının implementasyonu
- [x] RoleRepository sınıfının implementasyonu
- [x] RefreshTokenRepository sınıfının implementasyonu
- [x] InfrastructureServiceRegistration.cs'de repository kayıtlarının yapılması

## 6. Auth Servisi İmplementasyonu (Öncelik: Orta)
- [x] Application katmanında IAuthService arayüzünün oluşturulması:
  - Task<AuthResponseDto> RegisterAsync(UserRegisterDto request)
  - Task<AuthResponseDto> LoginAsync(UserLoginDto request)
  - Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
  - Task RevokeTokenAsync(string token)
  - Task<string> CreateAccessTokenAsync(User user, IList<Role> roles)
  - Task<string> GenerateRefreshTokenAsync()
- [x] Infrastructure katmanında AuthService implementasyonu:
  - BCrypt ile şifre hashleme
  - JWT token oluşturma (rol bazlı)
  - Kullanıcı doğrulama
  - Refresh token yönetimi:
    - Token üretimi
    - Token rotasyonu
    - Token iptali

## 7. Controller Katmanı (Öncelik: Orta)
- [x] API katmanında AuthController'ın oluşturulması:
  - [x] [AllowAnonymous] Register endpoint'i (/api/auth/register)
  - [x] [AllowAnonymous] Login endpoint'i (/api/auth/login)
  - [x] [AllowAnonymous] Refresh endpoint'i (/api/auth/refresh)
  - [x] [Authorize] Revoke endpoint'i (/api/auth/revoke)
  - [x] [Authorize] GetCurrentUser endpoint'i (/api/auth/me)
  - [x] [Authorize(Roles = "Admin")] AssignRole endpoint'i (/api/auth/assign-role).

## 8. Rol Bazlı Erişim Kontrolü (Öncelik: Düşük)
- [x] JWT token'lara rol claim'lerinin eklenmesi
- [x] Endpoint'lere [Authorize(Roles = "Admin,User")] attribute'larının eklenmesi
- [x] Role bazlı policy'lerin eklenmesi

## 9. Güvenlik İyileştirmeleri (Öncelik: Düşük)
- [x] Application katmanında password validation service'in oluşturulması
- [x] API katmanında rate limiting middleware'inin eklenmesi
- [x] Refresh token güvenlik önlemleri:
  - HttpOnly cookie kullanımı
  - Secure flag ayarı
  - SameSite=Strict ayarı
  - Token rotasyonu
  - Absolute ve sliding expiration
- [x] User işlemleri için logging mekanizmasının eklenmesi

## 10. Test ve Dokümantasyon (Öncelik: Düşük)
- [ ] Unit testlerin yazılması:
  - AuthService testleri
  - UserRepository testleri
  - RoleRepository testleri
  - RefreshTokenRepository testleri
  - Password validation testleri
- [ ] Integration testlerin yazılması
- [x] API dokümantasyonunun hazırlanması
- [x] Swagger/OpenAPI açıklamalarının eklenmesi
- [x] Postman koleksiyonunun hazırlanması

## Notlar
- Mevcut User modelinde zaten email, username ve passwordHash alanları bulunmakta
- ApplicationDbContext'te Users DbSet'i mevcut
- InfrastructureServiceRegistration.cs'de dependency injection yapısı mevcut
- BaseEntity'den gelen özellikler (Id, CreatedAt, UpdatedAt, IsDeleted) kullanılabilir
- Tüm hata durumları için exception handling mekanizması kurulmalı
- JWT Secret Key'in production ortamında güvenli şekilde saklanması gerekli
- Soft delete özelliği tüm entity'ler için aktif olacak
- Refresh Token'lar istemci tarafında güvenli şekilde saklanmalı
- Token rotasyonu güvenlik için önemli bir önlem .