/* 
 * 1. USING BİLDİRİMLERİ
 * System.ComponentModel.DataAnnotations: [Key] gibi temel kimlik belirleyicilerini kullanmamızı sağlar.
 * System.ComponentModel.DataAnnotations.Schema: [ForeignKey] etiketiyle iki tabloyu birbirine bağlamak için gereklidir.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskiFatura.Models
{
    /* 
     * 2. CLASS (SINIF) TANIMI
     * Tahakkuk sınıfı, fatura kesim işlemlerini temsil eder ve PostgreSQL'de 'Tahakkuklar' tablosuna dönüşür.
     */
    public class Tahakkuk
    {
        /* 
         * 3. ID ALANI (BİRİNCİL ANAHTAR - PRIMARY KEY)
         * [Key]: Bu faturanın (tahakkukun) benzersiz işlem numarasıdır (Örn: Fatura No).
         * int: Otomatik artan (Identity) yapıdadır. Yeni bir fatura kesildiğinde 
         * veritabanı sıradaki numarayı otomatik olarak atar.
         */
        [Key]
        public int Id { get; set; }

        /* 
         * 4. FOREIGN KEY (YABANCI ANAHTAR - SAYAÇ BAĞLANTISI)
         * SayacId: Veritabanında fiziksel olarak tutulan kolondur. Bu faturanın hangi sayaca kesildiğini 
         * sayacın Id'si (örn: 105 numaralı sayaç) üzerinden veritabanına kaydeder.
         */
        public int SayacId { get; set; }
        
        /* 
         * 5. NAVIGATION PROPERTY (BİRE-BİR YÖNLENDİRME)
         * [ForeignKey("SayacId")]: EF Core'a "Üstteki SayacId sayısını kullanarak git Sayac tablosundaki doğru sayacı bul" der.
         * public Sayac Sayac { get; set; }: Veritabanında KOLON OLUŞTURMAZ. C# tarafında 'tahakkuk.Sayac.SayacSeriNo' diyerek 
         * faturanın bağlı olduğu sayacın bilgilerine JOIN işlemi ile hızlıca ulaşmamızı sağlar.
         */
        [ForeignKey("SayacId")]
        public Sayac Sayac { get; set; }

        /* 
         * 6. FATURA DÖNEMİ
         * Bu faturanın hangi aya/yıla ait olduğunu tutar (Örn: "2026-08"). 
         * string olarak tutulması, arayüzde filtreleme yaparken işimizi çok kolaylaştırır.
         */
        public string FaturaDonemi { get; set; } 

        /* 
         * 7. ONDALIKLI SAYILAR (DECIMAL KULLANIMI)
         * Neden double veya float değil de decimal? Çünkü C#'ta parasal değerler ve hassas ölçümler 
         * (su tüketimi gibi) küsurat kayıpları yaşanmaması için daima 'decimal' ile tutulur.
         * IlkOkumaDegeri: Geçen ayki sayaç endeksi.
         * SonOkumaDegeri: Bu ay okunan sayaç endeksi.
         */
        public decimal IlkOkumaDegeri { get; set; }
        public decimal SonOkumaDegeri { get; set; }
        
        /* 
         * 8. HESAPLANAN DEĞERLER
         * TuketimMiktari: (Son Okuma - İlk Okuma) sonucu çıkan, abonenin harcadığı net su (m³).
         * FaturaTutari: Tüketim miktarının güncel su birim fiyatı (ve varsa kademe algoritması) ile 
         * çarpılarak hesaplanan TL cinsinden ödenecek net tutardır.
         */
        public decimal TuketimMiktari { get; set; } 
        public decimal FaturaTutari { get; set; } 
        
        /* 
         * 9. ZAMAN DAMGASI (TIMESTAMP)
         * DateTime: Faturanın son ödeme tarihini gün, ay, yıl ve saat olarak veritabanında tutar.
         * Eğer bu tarih geçerse, sisteme "Gecikme Zammı" veya "Kesme İşlemi" uyarıları eklemek için kullanılabilir.
         */
        public DateTime SonOdemeTarihi { get; set; }
        
        /* 
         * 10. DURUM BİLDİRİCİSİ (BOOLEAN)
         * bool: Veritabanında (PostgreSQL) sadece TRUE veya FALSE (1 veya 0) olarak tutulur.
         * Fatura ilk kesildiğinde 'false' (Ödenmedi) olur. Vezneden para yatınca sistem bunu 'true' (Ödendi) yapar.
         */
        public bool OdendiMi { get; set; }
        
        /* 
         * 11. GÖRSEL/DOSYA YOLU
         * Eğer ileride bir mobil uygulama yazıp sayacın fotoğrafını çekerek OCR (Yapay Zeka ile okuma) 
         * yaparsak, o çekilen fotoğrafın sunucudaki yerini (Örn: "/images/sayaclar/foto1.jpg") tutmak için ayırdığımız metin alanıdır.
         */
        public string SayacGorselYolu { get; set; } 
    }
}