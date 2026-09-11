/* 
 * 1. GEREKLİ KÜTÜPHANELER (USING BİLDİRİMLERİ)
 * AskiFatura.Data: AskiDbContext (veritabanı köprümüz) sınıfına ulaşmak için.
 * AskiFatura.Models: Tahakkuk modelimizi kullanabilmek için.
 * Microsoft.AspNetCore.Mvc: API yönlendirmeleri ([Route], [ApiController], IActionResult vb.) için.
 * Microsoft.EntityFrameworkCore: .ToListAsync() ve .Include() gibi veritabanı operasyonları için.
 */
using AskiFatura.Data;
using AskiFatura.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskiFatura.Controllers
{
    /* 
     * 2. API YÖNLENDİRME (ROUTING) VE KİMLİK
     * [Route("api/[controller]")]: Bu sınıfa 'http://localhost:5197/api/Tahakkuk' adresiyle ulaşılır.
     * [ApiController]: Gelen verilerdeki kuralları otomatik kontrol eden güvenlik kalkanımızdır.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class TahakkukController : ControllerBase
    {
        /* 
         * 3. DEPENDENCY INJECTION (VERİTABANI BAĞLANTISI)
         * _context: PostgreSQL ile konuşmamızı sağlayan tünelimiz.
         */
        private readonly AskiDbContext _context;

        public TahakkukController(AskiDbContext context)
        {
            _context = context;
        }

        /* 
         * ==========================================================
         * 4. GET ENDPOINT (TÜM FATURALARI LİSTELEME)
         * ==========================================================
         * [HttpGet]: GET isteklerini karşılar.
         * İlişkili tabloları (.Include ve .ThenInclude) zincirleyerek Abone ve Sayaç 
         * bilgilerini tek seferde eksiksiz olarak çeker.
         */
        [HttpGet]
        public async Task<IActionResult> GetTahakkuklar()
        {
            var tahakkuklar = await _context.Tahakkuklar
                .Include(t => t.Sayac)
                .ThenInclude(s => s.Abone)
                .ToListAsync();
            
            return Ok(tahakkuklar);
        }

        /* 
         * ==========================================================
         * 5. POST ENDPOINT (YENİ FATURA KESME VE HESAPLAMA)
         * ==========================================================
         * [HttpPost]: Yeni fatura ekleme isteklerini karşılar.
         * Business Logic (İş Mantığı): Tüketim miktarını ve fatura tutarını 
         * güvenli bir şekilde sunucu tarafında (Backend'de) hesaplar.
         */
        [HttpPost]
        public async Task<IActionResult> TahakkukEkle([FromBody] Tahakkuk yeniTahakkuk)
        {
            // Güvenlik ve Mantık Kontrolü: Son okuma ilk okumadan küçük olamaz!
            if (yeniTahakkuk.SonOkumaDegeri < yeniTahakkuk.IlkOkumaDegeri)
            {
                return BadRequest("Son okuma değeri, ilk okuma değerinden küçük olamaz.");
            }

            // Tüketim Miktarı Hesabı (m3): Son Okuma - İlk Okuma
            yeniTahakkuk.TuketimMiktari = yeniTahakkuk.SonOkumaDegeri - yeniTahakkuk.IlkOkumaDegeri;
            
            // Fatura Tutarı Hesabı: Tüketim miktarını sabit birim fiyatla (Örn: 25 TL) çarpıyoruz
            yeniTahakkuk.FaturaTutari = yeniTahakkuk.TuketimMiktari * 25;

            // Veritabanı ekleme sırasına al ve PostgreSQL'e kaydet
            _context.Tahakkuklar.Add(yeniTahakkuk);
            await _context.SaveChangesAsync();
            
            return Ok(yeniTahakkuk);
        }

        /* 
         * ==========================================================
         * 6. PUT ENDPOINT (FATURAYI ÖDENDİ OLARAK İŞARETLEME)
         * ==========================================================
         * [HttpPut("{id}/ode")]: Vatandaş ödeme ekranından kartla ödeme onay verdiğinde 
         * bu uç tetiklenir ve ilgili faturanın durumunu 'true' (Ödendi) yapar.
         */
        [HttpPut("{id}/ode")]
        public async Task<IActionResult> FaturaOde(int id)
        {
            // Veritabanından faturayı ID ile bul
            var tahakkuk = await _context.Tahakkuklar.FindAsync(id);
            
            if (tahakkuk == null)
            {
                return NotFound("Belirtilen ID numarasına ait fatura bulunamadı.");
            }

            // Durumu 'Ödendi' olarak değiştir
            tahakkuk.OdendiMi = true;
            
            // Değişikliği PostgreSQL'e kalıcı olarak kaydet
            await _context.SaveChangesAsync();

            return Ok(new { mesaj = "Fatura başarıyla ödendi.", faturaId = tahakkuk.Id });
        }
    }
}