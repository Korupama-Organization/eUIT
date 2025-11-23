// src/mobile/App.js
import React, { useEffect, useState } from "react";
import { View, Text, StyleSheet } from "react-native";
import { NavigationContainer } from "@react-navigation/native";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { createStackNavigator } from "@react-navigation/stack";
import { Feather } from "@expo/vector-icons";

// Import screens
import LoginScreen from "./features/auth/screens/LoginScreen";
import HomeScreen from "./features/auth/screens/HomeScreen";
import { isAuthenticated } from "./features/auth/api/authAPI";
import { ThemeProvider } from "./theme/ThemeProvider";
export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
      // Hàm kiểm tra trạng thái đăng nhập từ authAPI
      const authenticated = await isAuthenticated();
      setIsLoggedIn(authenticated);
    } catch (error) {
      console.error("Auth check error:", error);
      setIsLoggedIn(false);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    // Đoạn này có thể dùng màu cứng hoặc import theme từ ThemeProvider
    return (
      <View style={[styles.center, { backgroundColor: "#fff" }]}>
        <Text style={{ fontSize: 18, color: "#000" }}>
          Đang kiểm tra đăng nhập...
        </Text>
      </View>
    );
  }

  return (
    <ThemeProvider>
      <NavigationContainer>
        <AppNavigator isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} />
      </NavigationContainer>
    </ThemeProvider>
  );
}

// Định nghĩa Styles
const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: "center", alignItems: "center" },
});
