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

// Import Stack Navigator cho phần Tra Cứu (Lookup)
import LookupStack from "./features/students/screens/LookupStack";

// IMPORT MÀN HÌNH LỊCH HỌC HOÀN CHỈNH
import ScheduleScreen from "./features/students/screens/ScheduleScreen";

// Các màn hình giả (placeholder)
function ServicesScreen() {
  return (
    <View style={styles.center}>
      <Text>Services Screen</Text>
    </View>
  );
}

function SettingsScreen() {
  return (
    <View style={styles.center}>
      <Text>Settings Screen</Text>
    </View>
  );
}

// Tạo navigator
const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

// Main App với bottom tabs
function MainApp({ setIsLoggedIn }) {
  return (
    <Tab.Navigator
      screenOptions={{
        headerShown: false,
        tabBarShowLabel: false,
        tabBarStyle: {
          position: "absolute",
          bottom: 25,
          left: 20,
          right: 20,
          elevation: 0,
          backgroundColor: "#1E1E3D",
          borderRadius: 15,
          height: 70,
          borderTopWidth: 0,
        },
        tabBarActiveTintColor: "#FFFFFF",
        tabBarInactiveTintColor: "#A0A0A0",
      }}
    >
      <Tab.Screen
        name="Lookup"
        // Sử dụng LookupStack để quản lý các màn hình tra cứu
        component={LookupStack} 
        options={{
          tabBarIcon: ({ color }) => (
            <Feather name="search" color={color} size={24} />
          ),
        }}
      />
      <Tab.Screen
        name="Services"
        component={ServicesScreen}
        options={{
          tabBarIcon: ({ color }) => (
            <Feather name="briefcase" color={color} size={24} />
          ),
        }}
      />
      <Tab.Screen
        name="Home"
        options={{
          tabBarIcon: ({ color }) => (
            <Feather name="home" size={24} color={color} />
          ),
        }}
      >
        {() => <HomeScreen setIsLoggedIn={setIsLoggedIn} />}
      </Tab.Screen>

      <Tab.Screen
        name="Schedule"
        component={ScheduleScreen} // <-- SỬ DỤNG SCHEDULE SCREEN THỰC TẾ
        options={{
          tabBarIcon: ({ color }) => (
            <Feather name="calendar" color={color} size={24} />
          ),
        }}
      />
      <Tab.Screen
        name="Settings"
        component={SettingsScreen}
        options={{
          tabBarIcon: ({ color }) => (
            <Feather name="settings" color={color} size={24} />
          ),
        }}
      />
    </Tab.Navigator>
  );
}

// Auth Stack Navigator
function AuthStack({ setIsLoggedIn }) {
  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      <Stack.Screen name="Login">
        {(props) => <LoginScreen {...props} setIsLoggedIn={setIsLoggedIn} />}
      </Stack.Screen>
    </Stack.Navigator>
  );
}

// Root App Component
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
    return (
      <View style={styles.center}>
        <Text style={{ fontSize: 18, color: "#6B7280" }}>
          Đang kiểm tra đăng nhập...
        </Text>
      </View>
    );
  }

  return (
    <NavigationContainer>
      {isLoggedIn ? (
        <MainApp setIsLoggedIn={setIsLoggedIn} />
      ) : (
        <AuthStack setIsLoggedIn={setIsLoggedIn} />
      )}
    </NavigationContainer>
  );
}

// Định nghĩa Styles
const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: "center", alignItems: "center" },
});
