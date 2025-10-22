# Hướng dẫn cài đặt môi trường lập trình Mobile Frontend

## Yêu cầu hệ thống
- Node.js >= 18: https://nodejs.org/
- npm >= 9 (hoặc yarn)
- Git
- Android Studio (để chạy Android Emulator) hoặc Xcode (cho iOS, chỉ trên macOS)
- Visual Studio Code (khuyến nghị) với các extension React Native

## Các bước cài đặt

### 1. Clone repository
```bash
git clone https://github.com/Korupama-Organization/eUIT.git
cd eUIT/src/mobile
```

### 2. Cài đặt dependencies
```bash
npm install
# hoặc
yarn install
```

### 3. Cài đặt các package navigation (nếu chưa có)
```bash
npm install @react-navigation/native @react-navigation/bottom-tabs @react-navigation/stack
npm install react-native-screens react-native-safe-area-context
npm install @expo/vector-icons
```

### 4. Chạy ứng dụng
- **Android:**
	```bash
	npm run android
	```
- **iOS (chỉ trên macOS):**
	```bash
	npm run ios
	```
- **Web (nếu dùng Expo):**
	```bash
	npm run web
	```

## Một số lệnh hữu ích
- Xây dựng lại project native:
	```bash
	npx react-native run-android
	npx react-native run-ios
	```
- Kiểm tra lỗi lint:
	```bash
	npm run lint
	```

## Ghi chú
- Đảm bảo đã cài đặt Android Studio và cấu hình emulator nếu chạy trên Android.
- Nếu gặp lỗi thiếu package, kiểm tra và cài đặt theo thông báo lỗi.
