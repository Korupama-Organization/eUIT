// src/mobile/App.js
import React, { useEffect, useState, createContext, useContext } from "react";
import { View, Text, StyleSheet } from "react-native";
import { NavigationContainer } from "@react-navigation/native";
import AppNavigator from "./navigation/AppNavigator";
import { isAuthenticated } from "./features/auth/api/authAPI";
import { darkTheme, lightTheme } from "./theme/theme";

// ----- Tạo Context Theme -----
export const ThemeContext = createContext();
export const useTheme = () => useContext(ThemeContext);

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  // Trạng thái theme
  const [theme, setTheme] = useState(darkTheme);
  const toggleTheme = () => {
    setTheme((prev) => (prev === darkTheme ? lightTheme : darkTheme));
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
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
    return (
      <View style={[styles.center, { backgroundColor: theme.background }]}>
        <Text style={{ fontSize: 18, color: theme.textPrimary }}>
          Đang kiểm tra đăng nhập...
        </Text>
      </View>
    );
  }

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      <NavigationContainer>
        <AppNavigator
          isLoggedIn={isLoggedIn}
          setIsLoggedIn={setIsLoggedIn}
        />
      </NavigationContainer>
    </ThemeContext.Provider>
  );
}

const styles = StyleSheet.create({
  center: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
});
