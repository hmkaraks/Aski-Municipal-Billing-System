/* 
 * 1. USING BİLDİRİMLERİ (GEREKLİ KÜTÜPHANELER)
 * AskiFatura.Data: AskiDbContext (veritabanı köprümüz) sınıfına ulaşmak için gereklidir.
 * AskiFatura.Models: Abone modelimizi (şablonumuzu) kullanabilmek için gereklidir.
 * Microsoft.AspNetCore.Mvc: [ApiController], [Route], ControllerBase, IActionResult gibi API altyapısını sağlayan kütüphanedir.
 * Microsoft.EntityFrameworkCore: Veritabanı işlemleri yaparken kullanacağımız .ToListAsync() gibi asenkron (eşzamanlı olmayan) metotları sisteme dahil eder.
 */
using AskiFatura.Data;
using AskiFatura.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskiFatura.Controllers
{
    /* 
     * 2. API YÖNLENDİRME (ROUTING) VE DAVRANIŞ
     * [Route("api/[controller]")]: Tarayıcıdan veya arayüzden bu sınıfa nasıl ulaşılacağını belirler. 
     * [controller] kısmı otomatik olarak sınıfın adını ('Abone'Controller -> 'Abone') alır. 
     * Yani bu sınıftaki işlemlere ulaşmak için adresimiz: http://localhost:5197/api/Abone olur.
     * 
     * [ApiController]: Bu sınıfın sıradan bir C# sınıfı değil, bir RESTful API Controller'ı olduğunu belirtir.
     * En büyük faydası: Abone.cs içinde yazdığımız o [Required], [StringLength(11)] gibi kurallar ihlal edilirse (örn: 10 haneli TC gelirse), 
     * bu etiket o hatayı otomatik yakalar ve metotların içine hiç girmeden anında "400 Bad Request" fırlatır.
     */
    [Route("api/[controller]")]
    [ApiController]
    
    /* 
     * 3. CLASS (SINIF) TANIMI VE MİRAS ALMA
     * AboneController: Dış dünyadan (web sitesi, mobil uygulama, Swagger) gelen istekleri karşılayan sınıfımız.
     * : ControllerBase: MVC mimarisinde, API'ler için kullanılan temel sınıftır. Bu sayede Ok(), NotFound(), BadRequest() gibi hazır HTTP yanıtlarını kullanabiliriz.
     */
    public class AboneController : ControllerBase
    {
        /* 
         * 4. DEPENDENCY INJECTION (BAĞIMLILIK ENJEKSİYONU) VE CONSTRUCTOR
         * private readonly AskiDbContext _context: Veritabanına bağlanmamızı sağlayan aracıdır. Başka bir yerden ezilmesin diye sadece okunabilir (readonly) yapılır.
         * 
         * public AboneController(AskiDbContext context): Sınıfın 'Yapıcı' (Constructor) metodudur. 
         * Sistem (Program.cs) ayağa kalktığında, bize hazır bir veritabanı bağlantısı (context) verir, biz de bunu kendi '_context' değişkenimize eşitleriz.
         * Böylece aşağıdaki metotların içinde "_context.Aboneler..." diyerek PostgreSQL'e doğrudan ulaşabiliriz.
         */
        private readonly AskiDbContext _context;

        public AboneController(AskiDbContext context)
        {
            _context = context;
        }

        /* 
         * 5. GET İSTEĞİ (VERİ OKUMA / LİSTELEME)
         * [HttpGet]: Arayüzden (örneğin fetch ile) sadece 'GET' (veriyi bana getir) isteği atıldığında bu metot tetiklenir.
         * async Task<IActionResult>: İşlemin asenkron (arka planda tıkanmadan) yapılacağını ve sonucunda standart bir HTTP durumu döneceğini belirtir.
         */
        [HttpGet]
        public async Task<IActionResult> GetAboneler()
        {
            /* 
             * await _context.Aboneler.ToListAsync(): Veritabanındaki 'Aboneler' tablosuna git, 
             * tüm kayıtları çekip bir C# Listesine (diziye) çevir. 
             * 'await' komutu, veritabanından cevap gelene kadar sunucunun kilitlenmesini engeller.
             */
            var aboneler = await _context.Aboneler.ToListAsync();
            
            /* 
             * return Ok(aboneler): Çekilen tüm abone listesini JSON formatına çevirip "200 Başarılı" koduyla 
             * dış dünyaya (yani senin tasarladığın o şık HTML tablosuna) gönderir.
             */
            return Ok(aboneler);
        }

        /* 
         * 6. POST İSTEĞİ (YENİ VERİ EKLEME)
         * [HttpPost]: Dışarıdan (form üzerinden veya Swagger'dan) sisteme yeni bir veri (JSON) gönderilerek kayıt işlemi istendiğinde bu metot çalışır.
         */
        [HttpPost]
        public async Task<IActionResult> AboneEkle([FromBody] Abone yeniAbone)
        {
            /* 
             * [FromBody] Abone yeniAbone: Arayüzden gelen JSON verisini otomatik olarak alır ve 
             * bizim Models/Abone.cs şablonumuzla eşleştirip 'yeniAbone' adında dolu bir C# nesnesine dönüştürür.
             */
            
            /* 
             * _context.Aboneler.Add(yeniAbone): Yeni aboneyi veritabanı tablosuna EKLENMEK ÜZERE sıraya alır. 
             * Dikkat et, bu satırda henüz veritabanına yazma işlemi gerçekleşmez, sadece hafızada (RAM) hazırlanır.
             */
            _context.Aboneler.Add(yeniAbone);
            
            /* 
             * await _context.SaveChangesAsync(): İşte asıl mühür burasıdır! 
             * Hafızada sıraya alınan işlemi alır ve PostgreSQL'e gerçek bir "INSERT INTO..." SQL komutu göndererek veriyi kalıcı olarak kaydeder.
             */
            await _context.SaveChangesAsync();
            
            /* 
             * return Ok(yeniAbone): İşlem başarıyla bittiyse, PostgreSQL'in o aboneye otomatik olarak verdiği 'Id' numarası da dahil olmak üzere, 
             * abonenin en güncel halini arayüze JSON olarak geri döndürür.
             */
            return Ok(yeniAbone);
        }
    }
}