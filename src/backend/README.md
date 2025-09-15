 

# Hướng dẫn cài đặt môi trường lập trình Backend

## Yêu cầu hệ thống
- .NET 9 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
- PostgreSQL (khuyến nghị >= 14): https://www.postgresql.org/download/
- Visual Studio 2022 (hoặc VS Code) với các extension hỗ trợ C#
- Git
 
 ## Các bước cài đặt
 
 ### 1. Clone repository
 ```bash
 git clone https://github.com/Korupama-Organization/eUIT.git
 cd eUIT/src/backend
 ```
 
 ### 2. Cấu hình database
 - Tạo một database PostgreSQL mới (ví dụ: `euitdb`).
 - Cập nhật chuỗi kết nối trong file `appsettings.json` (tạo từ `appsettings.Example.json`).
 
 Ví dụ:
 ```json
 "ConnectionStrings": {
	 "DefaultConnection": "Host=localhost;Port=5432;Database=eUIT;Username=postgres;Password=yourpassword"
 }
 ```
 
 ### 3. Chạy migration để tạo bảng
 ```bash
 dotnet tool install --global dotnet-ef # Nếu chưa cài
 cd src/backend
 # Tạo file appsettings.json nếu chưa có
 cp appsettings.Example.json appsettings.json # hoặc tự tạo thủ công
 # Chạy migration
	dotnet ef database update
 ```
 
 ### 4. Chạy ứng dụng
 ```bash
 dotnet run --project eUIT.API.csproj
 ```
 
 API sẽ chạy mặc định ở `https://localhost:5001` hoặc `http://localhost:5000`.
 
 ## Một số lệnh hữu ích
 - Tạo migration mới:
	 ```bash
	 dotnet ef migrations add <MigrationName>
	 ```
 - Xem/chạy các lệnh HTTP mẫu: sử dụng file `eUIT.API.http` với extension REST Client trên VS Code.
 
 ## Ghi chú
 - Luôn tuân thủ clean architecture, sử dụng DI, async/await, logging, validation.
