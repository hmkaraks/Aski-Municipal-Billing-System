/* 
 * 1. USING BİLDİRİMLERİ
 * Microsoft.EntityFrameworkCore: PostgreSQL veritabanı bağlantımızı kurmak için gerekli kütüphane.
 * AskiFatura.Data: 'AskiDbContext' köprümüzü bu dosyaya tanıtabilmek için.
 * System.Text.Json.Serialization: JSON içindeki sonsuz döngü (Circular Reference) hatasını kırmak için eklediğimiz yeni kütüphane!
 */
using Microsoft.EntityFrameworkCore;
using AskiFatura.Data;
using System.Text.Json.Serialization;

/* 
 * 2. WEB UYGULAMASI İNŞA EDİCİSİ (BUILDER)
 * Uygulamanın temel iskeletini oluşturur. 'appsettings.json' gibi ayar dosyalarını 
 * otomatik olarak okuyarak sunucuyu hazırlamaya başlar.
 */
var builder = WebApplication.CreateBuilder(args);

/* 
 * ==========================================================
 * 3. DEPENDENCY INJECTION (SERVİS KAYITLARI)
 * ==========================================================
 */

// 3.1. Veritabanı Bağlantısını (DbContext) Sisteme Tanıtma
/* 
 * options.UseNpgsql: "Bu sistem PostgreSQL kullanacak."
 * builder.Configuration.GetConnectionString: "Şifreyi ve adresi 'appsettings.json'dan al."
 */
builder.Services.AddDbContext<AskiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3.2. Projeye Controller Desteği ve "JSON DÖNGÜ KIRICI" Ekleme (EN KRİTİK YER!)
/* 
 * Normalde sadece "AddControllers()" yazıyorduk. Ama Abone ve Sayaç tabloları birbirine 
 * bağlandığında (Include) sistem sonsuz döngüye girip HTTP 500 hatası veriyordu.
 * 
 * AddJsonOptions -> ReferenceHandler.IgnoreCycles: Bu sihirli komut .NET'e şunu söyler: 
 * "Eğer JSON oluştururken Abone'den Sayaca, oradan tekrar Abone'ye döndüğünü fark edersen 
 * panikleme, döngüyü kopart ve veriyi ekrana bas!"
 * İşte Yönetim Panelindeki tabloların artık ŞAK diye yüklenmesini sağlayan kod budur.
 */
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// 3.3. Swagger (Görsel API Test Arayüzü) Desteği Ekleme
/* 
 * Yazdığımız tüm Controller'ları tarar ve test edebilmemiz için görsel bir arayüz hazırlar.
 */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* 
 * ==========================================================
 * 4. UYGULAMANIN İNŞA EDİLMESİ (BUILD)
 * ==========================================================
 * Yukarıda kaydettiğimiz tüm servisleri alır ve çalışmaya hazır bir 'app' nesnesi üretir.
 */
var app = builder.Build();

/* 
 * ==========================================================
 * 5. HTTP REQUEST PIPELINE (ARA KATMANLAR - MIDDLEWARES)
 * ==========================================================
 */

/* 
 * app.UseDefaultFiles(): Tarayıcıya 'localhost' yazıldığında otomatik olarak 
 * 'wwwroot' içindeki 'index.html' (Bizim yeni giriş ekranımız) dosyasını çalıştırır.
 */
app.UseDefaultFiles(); 

/* 
 * app.UseStaticFiles(): 'wwwroot' klasörünün dış dünyadan (tarayıcıdan) 
 * erişilebilir olmasını sağlar. (Bunu yazmazsak Bootstrap, CSS, JS çalışmaz).
 */
app.UseStaticFiles();  

/* 
 * 5.1. Swagger Arayüzünü Yayına Alma
 * Test için tarayıcıya 'http://localhost:XXXX/swagger' yazıldığında dashboard'u açar.
 */
app.UseSwagger();
app.UseSwaggerUI();

/* 
 * 5.2. Yönlendirme (Routing) İşlemi
 * Dışarıdan gelen '/api/Tahakkuk' gibi istekleri alır ve doğru Controller'a yönlendirir.
 */
app.MapControllers();

/* 
 * 6. MOTORU ÇALIŞTIRMA!
 * Bu komut sunucuyu ayağa kaldırır, gelen istekleri dinlemeye başlar.
 */
app.Run();