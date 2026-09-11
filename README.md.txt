# ASKİ Yönetim ve Vatandaş Portalı 💧

Modern web mimarileri (Thin Client & RESTful API) kullanılarak geliştirilmiş, tam kapsamlı Belediye Altyapı ve Su Faturalandırma Otomasyonu. 

Bu proje, bir kurumun hem iç yönetim (Personel/Admin) süreçlerini hem de dış (Vatandaş) işlemlerini tek bir merkezden, yüksek güvenlik ve performans standartlarıyla yönetebilmesi için tasarlanmıştır.

## 🚀 Kullanılan Teknolojiler
* **Backend:** C# .NET Core, Entity Framework Core
* **Veritabanı:** PostgreSQL
* **Frontend:** HTML5, Vanilla JavaScript (Fetch API), Bootstrap 5, CSS3 (Glassmorphism UI)
* **Mimari Yaklaşım:** İnce İstemci (Thin Client), Zero-Trust Backend Hesaplamaları

## 💡 Temel Özellikler
* **Rol Bazlı SPA (Single Page Application) Mimarisi:** Personel ve Vatandaş için ayrıştırılmış özel arayüzler.
* **Güvenli Tahakkuk Mantığı:** Fatura tutarı hesaplamaları frontend'de (JS) değil, manipülasyonu engellemek adına tamamen backend'de (C#) yapılır.
* **Dinamik Veri Yönetimi:** Sayfa yenilenmeden asenkron API istekleriyle Abone, Sayaç ve Fatura CRUD işlemleri.
* **Oturum Yönetimi:** LocalStorage tabanlı frontend kimlik doğrulama simülasyonu.
* **Responsive ve Kurumsal UI:** Tüm cihazlara uyumlu, kurumsal filigran (watermark) destekli modern tasarım.

## 📸 Ekran Görüntüleri
*(Buraya Admin paneli, Fatura kesme modalı ve Vatandaş ekranının 3-4 tane güzel ekran görüntüsünü ekle)*