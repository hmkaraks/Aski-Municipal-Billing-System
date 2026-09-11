/* 
 * 1. USING BİLDİRİMLERİ
 * System.ComponentModel.DataAnnotations: [Key] ve [Required] gibi veri doğrulama ve kimlik etiketlerini sağlar.
 * System.ComponentModel.DataAnnotations.Schema: [ForeignKey] gibi veritabanı tabloları arası ilişki (Foreign Key) kurmamızı sağlayan özel etiketleri getirir.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskiFatura.Models
{
    /* 
     * 2. CLASS (SINIF) TANIMI
     * Sayac sınıfı, PostgreSQL veritabanında 'Sayaclar' adında bir tabloya dönüşecektir.
     */
    public class Sayac
    {
        /* 
         * 3. ID ALANI (BİRİNCİL ANAHTAR - PRIMARY KEY)
         * [Key]: Bu tablonun ana kilididir. 
         * int olduğu için EF Core bunu PostgreSQL'de otomatik artan (Identity) bir kolon yapar. 
         * Her yeni sayaç eklendiğinde Id numarası (1, 2, 3...) sistem tarafından otomatik verilir.
         */
        [Key]
        public int Id { get; set; }

        /* 
         * 4. SAYAÇ SERİ NO
         * [Required]: Veritabanında bu alanı "NOT NULL" yapar. Yani seri numarası girilmeden sayaç kaydedilemez.
         * Herhangi bir MaxLength koymadığımız için PostgreSQL'de varsayılan metin uzunluğuyla (text/varchar) tutulur.
         */
        [Required]
        public string SayacSeriNo { get; set; }

        /* 
         * 5. SAYAÇ TİPİ
         * Üzerinde [Required] olmadığı için veritabanında "NULL" (boş geçilebilir) bir kolon olur. 
         * Mekanik, Akıllı, Kartlı gibi sayaç türlerini metin olarak tutar.
         */
        public string SayacTipi { get; set; }
        
        /* 
         * 6. KURULUM TARİHİ
         * DateTime tipi, PostgreSQL veritabanında 'timestamp' kolonuna dönüştürülür. 
         * Sayacın sisteme ne zaman kaydedildiğini veya bağlandığını saniyesine kadar tutmamızı sağlar.
         */
        public DateTime KurulumTarihi { get; set; }

        /* 
         * 7. FOREIGN KEY (YABANCI ANAHTAR) KOLONU
         * AboneId: Veritabanında fiziksel olarak tutulan, sayacın kime ait olduğunu gösteren sayıdır (Örn: 5 numaralı abone).
         * Bu sadece basit bir int (sayı) kolonudur, tablodaki gerçek eşleşmeyi sağlar.
         */
        public int AboneId { get; set; }
        
        /* 
         * 8. NAVIGATION PROPERTY (BİRE-BİR YÖNLENDİRME)
         * [ForeignKey("AboneId")]: EF Core'a "Üstteki AboneId sayısı aslında Abone tablosunun anahtarıdır" der.
         * public Abone Abone { get; set; }: Veritabanında KOLON OLUŞTURMAZ. 
         * C# kodunda çalışırken 'sayac.Abone.Ad' diyerek doğrudan abonenin bilgilerine JOIN (birleştirme) yaparak ulaşmamızı sağlar. 
         */
        [ForeignKey("AboneId")]
        public Abone Abone { get; set; }

        /* 
         * 9. BİRE-ÇOK İLİŞKİ (ONE-TO-MANY)
         * ICollection<Tahakkuk>: Bu da veritabanında kolon oluşturmaz.
         * Bir sayacın zaman içinde kesilmiş birden fazla faturası (tahakkuku) olacağını EF Core'a bildirir.
         * C# tarafında o sayaca ait tüm faturaları bir liste (koleksiyon) olarak çekmemize olanak tanır.
         */
        public ICollection<Tahakkuk> Tahakkuklar { get; set; }
    }
}