import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';

// --- 1. Import các Màn hình (Screens) ---
// Màn hình gốc của luồng Lookup (Màn hình Tra cứu tổng hợp)
import LookupScreen from './LookupScreen'; 
// Màn hình chi tiết kết quả học tập
import AcademicResultScreen from './AcademicResultScreen'; 

const Stack = createStackNavigator();

/**
 * LookupStack: Quản lý các màn hình liên quan đến tra cứu
 * - LookupHome (Màn hình gốc, nơi hiển thị các tùy chọn tra cứu - Không có Header)
 * - AcademicResultScreen (Màn hình chi tiết, có Header)
 */
function LookupStack() {
  return (
    <Stack.Navigator 
        initialRouteName="LookupHome"
        screenOptions={{
            // Các tùy chọn Header mặc định cho tất cả các màn hình (trừ khi bị ghi đè)
            headerStyle: {
                backgroundColor: '#7AF8FF', // Màu nền header theo UI của bạn
            },
            headerTintColor: '#171736', // Màu chữ/icon
            headerTitleStyle: {
                fontWeight: 'bold',
            },
        }}
    >
      {/* Màn hình chính (Root) của Tab Lookup */}
      <Stack.Screen 
        name="LookupHome" 
        component={LookupScreen} 
        options={{ 
            title: 'Tra cứu', 
            // Ẩn Header của Stack Navigator cho màn hình này
            headerShown: false,
        }} 
      />
      
      {/* Màn hình chi tiết - Header được hiển thị theo screenOptions hoặc được ghi đè */}
      <Stack.Screen 
        name="AcademicResult" 
        component={AcademicResultScreen} 
        options={{ 
            title: 'Kết quả học tập',
            headerShown: true, // Đảm bảo header được hiển thị cho màn hình chi tiết
        }} 
      />
    </Stack.Navigator>
  );
}

export default LookupStack;