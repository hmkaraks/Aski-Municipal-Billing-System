/* 
 * 1. USING BİLDİRİMLERİ (GEREKLİ KÜTÜPHANELER)
 * AskiFatura.Data: Veritabanı köprümüz olan AskiDbContext'i kullanabilmek için.
 * AskiFatura.Models: Sayac modelimizi tanıyabilmek için.
 * Microsoft.AspNetCore.Mvc: API yönlendirmeleri (Route, ControllerBase) için.
 * Microsoft.EntityFrameworkCore: Asenkron işlemler (.ToListAsync) ve en önemlisi ilişkili tabloları birleştirmemizi (JOIN) sağlayan '.Include()' metodunu kullanabilmek için eklendi.
 */
using AskiFatura.Data;
using AskiFatura.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskiFatura.Controllers
{
    /* 
     * 2. API YÖNLENDİRME (ROUTING) VE KİMLİK
     * [Route("api/[controller]")]: Bu sınıfa dışarıdan 'http://localhost:5197/api/Sayac' adresiyle ulaşılacağını belirtir.
     * [ApiController]: Validasyon hatalarını (örneğin SayacSeriNo boş gelirse) anında yakalayıp 400 Bad Request fırlatan API güvenlik kalkanımızdır.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class SayacController : ControllerBase
    {
        /* 
         * 3. DEPENDENCY INJECTION (BAĞIMLILIK ENJEKSİYONU)
         * _context: PostgreSQL ile konuşmamızı sağlayacak olan elçimizdir.
         * Constructor (SayacController) içinde bu elçiyi sisteme tanımlıyoruz.
         */
        private readonly AskiDbContext _context;

        public SayacController(AskiDbContext context)
        {
            _context = context;
        }

        /* 
         * 4. GET İSTEĞİ (SAYAÇLARI VE SAHİPLERİNİ LİSTELEME)
         * [HttpGet]: Arayüzden '/api/Sayac' adresine bir çağrı geldiğinde çalışır.
         */
        [HttpGet]
        public async Task<IActionResult> GetSayaclar()
        {
            /* 
             * İŞTE BURASI ÇOK KRİTİK! (EAGER LOADING MANTIĞI)
             * _context.Sayaclar: Veritabanındaki 'Sayaclar' tablosuna git.
             * 
             * .Include(s => s.Abone): Eğer bu satırı yazmasaydık, arayüze sadece sayacın seri numarası gider, o sayacın HANGİ ABONEYE (isim, soyisim vb.) ait olduğu bilgisi "null" (boş) olarak dönerdi. 
             * Entity Framework Core bu komutu gördüğü an, arka planda PostgreSQL'e muazzam bir "INNER JOIN" SQL sorgusu gönderir.
             * "Sayaclar tablosunu al, içindeki AboneId'ye bakarak git Aboneler tablosuyla birleştir ve sayaçlarla birlikte sahiplerinin tüm bilgilerini (Ad, Soyad vb.) de getir" der.
             * 
             * .ToListAsync(): Gelen bu birleştirilmiş devasa veriyi alıp bir C# Listesi haline getirir.
             */
            var sayaclar = await _context.Sayaclar.Include(s => s.Abone).ToListAsync();
            
            /* 
             * return Ok(sayaclar): Sayaçları (ve İç içe geçmiş Abone bilgilerini) JSON formatına çevirip "200 Başarılı" koduyla HTML/JS tarafına (Frontend'e) teslim eder.
             */
            return Ok(sayaclar);
        }

        /* 
         * 5. POST İSTEĞİ (YENİ SAYAÇ EKLEME)
         * [HttpPost]: '/api/Sayac' adresine dışarıdan (Swagger veya Frontend formundan) JSON formatında yeni bir sayaç bilgisi geldiğinde tetiklenir.
         */
        [HttpPost]
        public async Task<IActionResult> SayacEkle([FromBody] Sayac yeniSayac)
        {
            /* 
             * [FromBody] Sayac yeniSayac: Gelen JSON'ı okur, içindeki "SayacSeriNo", "SayacTipi" ve "AboneId" değerlerini bizim Sayac şablonumuza doldurur.
             */
            
            /* 
             * _context.Sayaclar.Add(yeniSayac): Veritabanına "Yeni bir sayaç kaydı geldi, bunu Sayaclar tablosuna eklemeye hazırlan" der. (Sadece RAM'de tutar).
             */
            _context.Sayaclar.Add(yeniSayac);
            
            /* 
             * await _context.SaveChangesAsync(): Hafızadaki hazırlığı PostgreSQL'e gerçek bir "INSERT" komutu olarak gönderir. Veri fiziksel olarak diske (veritabanına) yazılır.
             */
            await _context.SaveChangesAsync();
            
            /* 
             * return Ok(yeniSayac): İşlem başarıyla bittiğinde, veritabanının bu yeni sayaca otomatik olarak atadığı "Id" (Primary Key) numarasını da içeren sayaç bilgisini arayüze geri döndürür.
             */
            return Ok(yeniSayac);
        }
    }
}