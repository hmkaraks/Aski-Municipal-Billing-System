/* 
 * 1. USING BİLDİRİMLERİ
 * using: C# kütüphanelerini bu dosyaya dahil eder.
 * System.Collections.Generic: En alttaki 'ICollection' (liste/koleksiyon) yapısını kullanabilmemiz için gereklidir.
 * System.ComponentModel.DataAnnotations: [Key], [Required], [MaxLength] gibi kısıtlama (constraint) ve doğrulama (validation) etiketlerini kullanmamızı sağlar.
 */
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/* 
 * 2. NAMESPACE (İSİM UZAYI)
 * Bu sınıfın projedeki adresidir. Başka bir dosyada bu sınıfı kullanmak istersek
 * 'using AskiFatura.Models;' yazarak burayı işaret ederiz. Bir nevi sanal klasörlemedir.
 */
namespace AskiFatura.Models
{
    /* 
     * 3. CLASS (SINIF) TANIMI
     * public: Bu sınıfın projenin her yerinden erişilebilir olduğunu belirtir.
     * class Abone: PostgreSQL'deki 'Aboneler' tablosunun C# tarafındaki şablonudur (Entity).
     */
    public class Abone
    {
        /* 
         * 4. ID ALANI (BİRİNCİL ANAHTAR)
         * [Key]: Entity Framework Core (EF Core) bu etiketi gördüğünde veritabanında bu kolonu "Primary Key" (PK) yapar.
         * int: Veri tipinin tam sayı olduğunu belirtir. EF Core '[Key]' ve 'int' ikilisini yan yana görünce, 
         * PostgreSQL'de bu kolonu otomatik artan (Identity / Serial) olarak ayarlar. Sen veri eklerken Id göndermezsin, veritabanı kendi verir.
         * get; set;: Kapsülleme (Encapsulation). Veriyi okumaya (get) ve içine veri yazmaya (set) izin verir.
         */
        [Key] 
        public int Id { get; set; } 

        /* 
         * 5. TC KİMLİK ALANI
         * [Required]: Bu alanın veritabanında "NOT NULL" olmasını sağlar. API'ye bu alan boş gelirse 400 Bad Request fırlatır.
         * ErrorMessage: Kural ihlal edildiğinde Frontend'e dönülecek spesifik hata mesajıdır.
         * 
         * [StringLength(11, MinimumLength = 11)]: Veritabanında kolonu 'varchar(11)' yapar. 
         * Hem alt hem üst sınırı 11 belirleyerek 10 veya 12 karakter girilmesini API seviyesinde engeller.
         * 
         * [RegularExpression]: Girilen metnin sadece belirli bir düzende olmasını zorunlu kılar.
         * "^[0-9]*$": Regex (Düzenli İfade) dilinde "^" başlangıç, "$" bitiş demektir. "[0-9]*" ise baştan sona sadece rakam olabilir demektir. Harf girişini bloklar.
         */
        [Required(ErrorMessage = "TC Kimlik alanı zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası tam 11 haneli olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "TC Kimlik sadece rakamlardan oluşmalıdır.")]
        public string TcKimlik { get; set; }

        /* 
         * 6. AD VE SOYAD ALANLARI
         * [MaxLength(50)]: Veritabanında kolonu 'varchar(50)' olarak oluşturur. 
         * Sadece üst sınır belirler. 50 karakteri geçen bir isim gelirse veritabanına gitmeden API'de işlem reddedilir.
         */
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [MaxLength(50, ErrorMessage = "Ad alanı en fazla 50 karakter olabilir.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [MaxLength(50, ErrorMessage = "Soyad alanı en fazla 50 karakter olabilir.")]
        public string Soyad { get; set; }

        /* 
         * 7. ADRES VE TELEFON ALANLARI
         * Üzerlerinde [Required] veya başka bir kısıtlama olmadığı için veritabanında 'NULL' (boş bırakılabilir) olarak ayarlanırlar.
         * Telefon numarası matematikte kullanılmayan bir veri olduğu ve başında "0" barındırdığı için int değil 'string' olarak tanımlanır.
         */
        public string Adres { get; set; }
        public string Telefon { get; set; }

        /* 
         * 8. NAVIGATION PROPERTY (İLİŞKİSEL YÖNLENDİRME)
         * ICollection<Sayac>: Bu satır veritabanında "Sayaclar" adında bir kolon OLUŞTURMAZ.
         * Bu yapı EF Core'a özgüdür. Veritabanındaki 'One-to-Many' (Bire-Çok) ilişkisini C# tarafında temsil eder.
         * Anlamı: "Bir Abone nesnesini veritabanından çektiğimde (Include komutu ile), o aboneye ait tüm sayaçları da PostgreSQL'den JOIN işlemiyle çek ve bu listenin içine doldur."
         */
        public ICollection<Sayac> Sayaclar { get; set; } 
    }
}