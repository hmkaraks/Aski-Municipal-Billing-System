/* 
 * 1. USING BİLDİRİMLERİ
 * AskiFatura.Models: Abone, Sayac ve Tahakkuk gibi veri şablonlarımızı (modellerimizi) 
 * bu dosyada tabloya dönüştürebilmek için projeye dahil ediyoruz.
 * Microsoft.EntityFrameworkCore: Bu dosyanın bir "DbContext" (Veritabanı Bağlamı) olmasını 
 * sağlayan, C# dünyasının en güçlü ORM (Object-Relational Mapping) kütüphanesidir.
 */
using AskiFatura.Models;
using Microsoft.EntityFrameworkCore;

namespace AskiFatura.Data
{
    /* 
     * 2. CLASS TANIMI VE MİRAS ALMA (INHERITANCE)
     * AskiDbContext: Bizim veritabanı işlemlerimizi yönetecek olan özel sınıfımız.
     * : DbContext: EF Core'un çekirdek sınıfıdır. Sınıfımızın sonuna ': DbContext' ekleyerek 
     * aslında şu mesajı veriyoruz: "Bu sıradan bir sınıf değil, bu sınıf benim veritabanı köprümdür. 
     * Veritabanına bağlanma, sorgu atma ve tablo oluşturma yeteneklerini DbContext'ten miras al."
     */
    public class AskiDbContext : DbContext
    {
        /* 
         * 3. CONSTRUCTOR (YAPICI METOT) VE AYARLARIN ALINMASI
         * Bu metot, uygulama (Program.cs) ayağa kalktığında otomatik olarak tetiklenir.
         * DbContextOptions<AskiDbContext> options: Program.cs dosyasında yazdığımız o 
         * "PostgreSQL kullanacağım, şifrem şu, portum 5432" gibi bağlantı ayarlarını (Connection String) 
         * içine alan pakettir.
         * 
         * : base(options): Bize gelen bu ayar paketini, miras aldığımız ana 'DbContext' sınıfının 
         * motoruna (base) iletiyoruz ki EF Core PostgreSQL'e nasıl bağlanacağını bilsin.
         */
        public AskiDbContext(DbContextOptions<AskiDbContext> options) : base(options)
        {
        }

        /* 
         * 4. DBSET TANIMLAMALARI (TABLOLARIN OLUŞTURULMASI)
         * İşte C# kodlarını veritabanındaki (PostgreSQL) fiziksel tablolara bağladığımız asıl sihir burası!
         * 
         * DbSet<Abone>: "Benim Models klasöründeki Abone.cs şablonumu al."
         * Aboneler: "Bunu PostgreSQL'de 'Aboneler' adında bir tablo olarak oluştur ve yönet."
         * 
         * get; set;: Controller (API) dosyalarında '_context.Aboneler.ToList()' yazarak 
         * bu tablo içindeki verileri okumamıza (get) ve tabloya yeni veri yazmamıza (set) olanak tanır.
         */
        public DbSet<Abone> Aboneler { get; set; }
        
        /* 
         * DbSet<Sayac> Sayaclar: Aynı şekilde Sayac modelini baz alarak veritabanında 'Sayaclar' tablosunu oluşturur.
         */
        public DbSet<Sayac> Sayaclar { get; set; }
        
        /* 
         * DbSet<Tahakkuk> Tahakkuklar: Tahakkuk modelini baz alarak veritabanında fatura işlemlerinin 
         * tutulacağı 'Tahakkuklar' tablosunu inşa eder.
         */
        public DbSet<Tahakkuk> Tahakkuklar { get; set; }
        
        /*
         * ÖNEMLİ NOT: 
         * EF Core 'Migrations' (Göçler) komutunu (Add-Migration) çalıştırdığında, sadece ve sadece 
         * burada DbSet<> olarak tanımladığın sınıfları veritabanı tablosuna dönüştürür. 
         * Eğer bir modeli oluşturup buraya yazmayı unutursan, PostgreSQL tarafında o tablo asla oluşmaz!
         */
    }
}