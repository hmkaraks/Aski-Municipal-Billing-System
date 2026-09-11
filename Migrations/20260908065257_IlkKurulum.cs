/* 
 * 1. USING BİLDİRİMLERİ
 * Microsoft.EntityFrameworkCore.Migrations: Bu dosyanın bir Migration (veritabanı sürüm) dosyası olmasını sağlayan temel kütüphanedir.
 * Npgsql.EntityFrameworkCore.PostgreSQL.Metadata: PostgreSQL'e özel olan (örneğin otomatik artan ID'ler gibi) özellikleri C# tarafında kullanmamızı sağlar.
 */
using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

/* 
 * 2. NAMESPACE (MIGRATIONS KLASÖRÜ)
 * EF Core, oluşturduğu tüm sürüm dosyalarını projedeki 'Migrations' klasörü altında toplar ki 
 * geçmişteki tüm değişiklikleri adım adım takip edebilelim.
 */
namespace AskiFatura.Migrations
{
    /* 
     * 3. MIGRATION SINIFI (İLK KURULUM)
     * public partial class IlkKurulum : Migration
     * Terminalde "Add-Migration IlkKurulum" yazdığın için EF Core bu isimde bir sınıf oluşturdu.
     * ': Migration' diyerek bu sınıfın bir veritabanı inşaat planı olduğunu belirtiyor.
     */
    public partial class IlkKurulum : Migration
    {
        /* 
         * 4. UP METODU (İLERİ SARMA / VERİTABANINA UYGULAMA)
         * Terminalde "Update-Database" yazdığında ÇALIŞACAK OLAN metottur.
         * Görevi: C# modellerine bakarak PostgreSQL'de boş olan veritabanına tabloları sıfırdan inşa etmektir.
         */
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* 
             * 5. ABONELER TABLOSUNUN OLUŞTURULMASI
             * migrationBuilder.CreateTable(...): PostgreSQL'e "CREATE TABLE Aboneler (...)" komutunu gönderir.
             */
            migrationBuilder.CreateTable(
                name: "Aboneler",
                columns: table => new
                {
                    /* 
                     * Id Kolonu:
                     * type: "integer" -> Sayısal tip.
                     * NpgsqlValueGenerationStrategy.IdentityByDefaultColumn: İşte bu komut PostgreSQL'e özeldir! 
                     * "Bu kolonu SERIAL/IDENTITY (otomatik artan) yap, her kayıtta numarayı veritabanı kendisi versin" der.
                     */
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    
                    /* 
                     * TcKimlik Kolonu:
                     * Hatırlarsan [StringLength(11)] yazmıştık. EF Core bunu "character varying(11)" yani VARCHAR(11) olarak çevirdi.
                     * nullable: false -> [Required] yazdığımız için "NOT NULL" yaptı. Boş geçilemez!
                     */
                    TcKimlik = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Ad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    
                    /* 
                     * Adres ve Telefon Kolonları:
                     * Üzerlerinde kural olmadığı için sınırsız metin ("text") ve "nullable: true" (boş geçilebilir) oldular.
                     */
                    Adres = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    /* table.PrimaryKey: Yukarıdaki Id kolonunu bu tablonun "Anahtarı" (PK) yapar. */
                    table.PrimaryKey("PK_Aboneler", x => x.Id);
                });

            /* 
             * 6. SAYAÇLAR TABLOSUNUN OLUŞTURULMASI
             */
            migrationBuilder.CreateTable(
                name: "Sayaclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SayacSeriNo = table.Column<string>(type: "text", nullable: false),
                    SayacTipi = table.Column<string>(type: "text", nullable: true),
                    KurulumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AboneId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayaclar", x => x.Id);
                    
                    /* 
                     * YABANCI ANAHTAR (FOREIGN KEY) VE CASCADE SİLME MANTIĞI
                     * table.ForeignKey: Sayaclar tablosundaki 'AboneId'yi, Aboneler tablosundaki 'Id' ile bağlar.
                     * onDelete: ReferentialAction.Cascade: BU ÇOK ÖNEMLİDİR! 
                     * Anlamı: "Eğer Aboneler tablosundan bir adamı silersen (Örn: 5 nolu abone), git Sayaclar tablosunda 
                     * o adamın üzerine kayıtlı olan tüm sayaçları da OTOMATİK OLARAK SİL!" (Buna Cascade Delete denir).
                     */
                    table.ForeignKey(
                        name: "FK_Sayaclar_Aboneler_AboneId",
                        column: x => x.AboneId,
                        principalTable: "Aboneler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            /* 
             * 7. TAHAKKUKLAR TABLOSUNUN OLUŞTURULMASI
             */
            migrationBuilder.CreateTable(
                name: "Tahakkuklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SayacId = table.Column<int>(type: "integer", nullable: false),
                    FaturaDonemi = table.Column<string>(type: "text", nullable: true),
                    
                    /* 
                     * C# tarafındaki 'decimal' tipi, PostgreSQL'de 'numeric' (hassas ondalıklı sayı) kolonuna dönüşür. 
                     * Su tüketimi ve para hesaplamalarında küsurat kaybı olmaması için en güvenli yoldur.
                     */
                    IlkOkumaDegeri = table.Column<decimal>(type: "numeric", nullable: false),
                    SonOkumaDegeri = table.Column<decimal>(type: "numeric", nullable: false),
                    TuketimMiktari = table.Column<decimal>(type: "numeric", nullable: false),
                    FaturaTutari = table.Column<decimal>(type: "numeric", nullable: false),
                    
                    SonOdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OdendiMi = table.Column<bool>(type: "boolean", nullable: false),
                    SayacGorselYolu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tahakkuklar", x => x.Id);
                    
                    /* Aynı şekilde sayacı silinen faturanın da otomatik silinmesi için Cascade uygulanır. */
                    table.ForeignKey(
                        name: "FK_Tahakkuklar_Sayaclar_SayacId",
                        column: x => x.SayacId,
                        principalTable: "Sayaclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            /* 
             * 8. İNDEKSLERİN (INDEXING) OLUŞTURULMASI
             * CreateIndex: EF Core, ilişkisel bağ kurduğumuz yabancı anahtar (Foreign Key) kolonlarına otomatik indeks atar.
             * Neden? Çünkü veritabanında "Bana 5 nolu abonenin sayaçlarını getir" diye arama yaptığımızda, 
             * PostgreSQL tüm tabloyu baştan sona taramak yerine bu indeks sayesinde saniyeler içinde o kaydı bulur. Performans artar.
             */
            migrationBuilder.CreateIndex(
                name: "IX_Sayaclar_AboneId",
                table: "Sayaclar",
                column: "AboneId");

            migrationBuilder.CreateIndex(
                name: "IX_Tahakkuklar_SayacId",
                table: "Tahakkuklar",
                column: "SayacId");
        }

        /* 
         * 9. DOWN METODU (GERİ SARMA / GERİ ALMA)
         * Eğer bir hata yaparsan ve terminalde eski bir sürüme dönmek istersen çalışacak metottur.
         * Up metodunun tam tersini yapar: Tabloları veritabanından tamamen siler (DropTable).
         * Dikkat et, en son oluşturulan 'Tahakkuklar'ı ilk başta siler ki ForeignKey hataları patlamasın.
         */
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tahakkuklar");

            migrationBuilder.DropTable(
                name: "Sayaclar");

            migrationBuilder.DropTable(
                name: "Aboneler");
        }
    }
}