
Refresh Token'ın geçerlilik süresini (TTL - Time To Live) belirlemek için appsettings.json dosyasına bir ayar ekle (örneğin, gün cinsinden): Jwt:RefreshTokenTTL (örn: 3).
Login İşlemi Güncellemesi (AuthService ve AuthController):

AuthService içindeki Login metodunu güncelle:
Başarılı kullanıcı doğrulamasından sonra, Access Token (JWT) oluşturmaya ek olarak:
Güvenli, rastgele bir string olarak yeni bir Refresh Token değeri oluştur (bunun için bir yardımcı private metot oluşturulabilir: GenerateRefreshToken()).
Yeni RefreshToken entity'sini oluştur (Token değeri, UserId, yapılandırmadan okunan TTL'e göre Expires tarihi, Created tarihi ile).
Bu yeni RefreshToken entity'sini veritabanına (RefreshTokens tablosuna) kaydet.
Login metodu artık hem AccessToken'ı hem de oluşturulan RefreshToken'ın string değerini döndürmeli.
Gerekirse LoginResponseDto gibi bir DTO'yu bu iki değeri içerecek şekilde güncelle (string AccessToken, string RefreshToken).
AuthController'daki /login endpoint'i bu güncellenmiş yanıtı döndürsün.
Yeni Refresh Endpoint'i (AuthController ve AuthService):

AuthController'a yeni bir endpoint ekle: [HttpPost("refresh")].
Bu endpoint girdi olarak bir DTO (RefreshTokenRequestDto) alsın. Bu DTO sadece string RefreshToken içersin.
AuthService'e yeni bir metot ekle: Task<LoginResponseDto> RefreshToken(RefreshTokenRequestDto request) (veya benzeri bir dönüş tipi). Bu metodun mantığı:
İstekten gelen RefreshToken string'ini al.
Bu token string'ine sahip RefreshToken entity'sini veritabanında bul (User bilgisini de include et).
Doğrulama:
Token bulundu mu?
Süresi dolmuş mu (Expires < DateTime.UtcNow)?
İptal edilmiş mi (Revoked != null)?
Eğer token geçersizse, uygun bir hata fırlat (örn: UnauthorizedAccessException veya özel bir exception) ve Controller bunu 401 Unauthorized veya 400 Bad Request olarak yakalasın.
Eğer token geçerliyse:
İlişkili User nesnesini kullanarak yeni bir Access Token (JWT) oluştur.
Refresh Token Rotation (Önemli Güvenlik Adımı):
Yeni, güvenli, rastgele bir Refresh Token string'i oluştur (GenerateRefreshToken() kullanarak).
Veritabanındaki kullanılan (eski) RefreshToken entity'sini bulup Revoked alanını şu anki zamanla güncelle (iptal et).
Yeni Refresh Token string'i, yeni Expires tarihi ve aynı UserId ile yeni bir RefreshToken entity'si oluştur ve veritabanına kaydet.
Yeni oluşturulan AccessToken ve yeni RefreshToken string'ini içeren bir yanıt nesnesi (örn: LoginResponseDto) döndür.
Yeni Revoke Endpoint'i (AuthController ve AuthService) (İsteğe Bağlı Ama Önerilir):

AuthController'a yeni bir endpoint ekle: [HttpPost("revoke")] (veya /logout). Bu endpoint [Authorize] niteliği ile korunabilir.
Bu endpoint girdi olarak string RefreshToken içeren bir DTO alsın (veya mevcut refresh token cookie'den okunabilir).
AuthService'e yeni bir metot ekle: Task RevokeToken(string token).
Mantık: Verilen token string'ine sahip RefreshToken'ı veritabanında bul ve Revoked alanını şu anki zamanla güncelle. Eğer token bulunamazsa hata verme (opsiyonel).
Gerekli DTO'lar:

RefreshTokenRequestDto (string RefreshToken)
Gerekirse LoginResponseDto güncellemesi (string AccessToken, string RefreshToken)
Güvenlik Notları:

Refresh Token'ların istemci tarafında güvenli saklanması gerektiğini vurgula (Web için HttpOnly, Secure, SameSite=Strict cookie; Mobil için Encrypted Storage).
Refresh Token Rotation'ın önemini belirt.
Çıktı Formatı: İstenen yeni/güncellenmiş bileşenler (Entity, DbContext kısmı, Servis metotları, Controller endpoint'leri, DTO'lar) için anlaşılır C# kod parçacıkları sağla. Gerekli using ifadelerini ekle. Önemli mantık adımlarını kod içinde yorumlarla açıkla.