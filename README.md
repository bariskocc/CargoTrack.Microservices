# CargoTrack Mikroservis Projesi

## 🚀 Proje Hakkında
CargoTrack, modern teknolojiler kullanılarak geliştirilmiş, B2B odaklı bir kargo takip sistemidir. Domain-Driven Design (DDD) prensipleri ve mikroservis mimarisi üzerine inşa edilmiştir.

## 🏗️ Sistem Mimarisi

### Mikroservisler
- **Identity.API**: Kimlik doğrulama ve yetkilendirme
- **Tracking.API**: Kargo takip işlemleri
- **Notification.API**: Bildirim yönetimi
- **Reporting.API**: Raporlama servisleri
- **Gateway.API**: API Gateway
- **Integration.API**: Dış sistem entegrasyonları

## 🛠️ Teknolojiler

### Backend
- .NET 8
- Entity Framework Core
- MediatR (CQRS)
- AutoMapper
- FluentValidation

### Veritabanı & Cache
- PostgreSQL
- MongoDB
- Redis
- Elasticsearch

### Message Broker & Real-time
- RabbitMQ
- SignalR

### DevOps & Monitoring
- Docker
- Kubernetes
- Serilog
- Kibana
- Hangfire

### Güvenlik
- IdentityServer4
- JWT Authentication

## 🚀 Başlangıç

### Gereksinimler
- .NET 9 SDK
- Docker Desktop
- PostgreSQL
- MongoDB
- Redis

### Kurulum

1. Repoyu klonlayın
```bash
git clone https://github.com/yourusername/CargoTrack.Microservices.git
```

2. Docker container'ları başlatın
```bash
docker-compose up -d
```

3. Veritabanı migration'ları çalıştırın
```bash
dotnet ef database update
```

4. Projeyi çalıştırın
```bash
dotnet run
```

## 📚 API Dokümantasyonu
Swagger UI üzerinden API dokümantasyonuna erişebilirsiniz:
- Gateway API: `http://localhost:5000/swagger`
- Identity API: `http://localhost:5001/swagger`
- Tracking API: `http://localhost:5002/swagger`

## 🧪 Test

```bash
dotnet test
```

## 📦 Deployment

### Docker ile Deployment
```bash
docker-compose -f docker-compose.prod.yml up -d
```

### Kubernetes ile Deployment
```bash
kubectl apply -f k8s/
```

## 🔒 Güvenlik
- JWT tabanlı kimlik doğrulama
- Role-based access control (RBAC)
- API Gateway güvenlik katmanı
- Rate limiting
- SSL/TLS encryption

## 📈 Monitoring
- Elasticsearch + Kibana entegrasyonu
- Serilog ile yapılandırılabilir loglama
- Health checks
- Prometheus metrics

## 🤝 Katkıda Bulunma
1. Fork'layın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit'leyin (`git commit -m 'feat: Add amazing feature'`)
4. Push'layın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📝 Lisans
Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 📞 İletişim
- Email: your.email@example.com
- Project Link: https://github.com/yourusername/CargoTrack.Microservices 