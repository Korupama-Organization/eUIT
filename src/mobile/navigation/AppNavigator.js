// src/mobile/navigation/AppNavigator.js
import React from "react";
import { createNativeStackNavigator } from "@react-navigation/native-stack";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { Feather } from "@expo/vector-icons";
import LoginScreen from "../features/auth/screens/LoginScreen";
import HomeScreen from "../screens/HomeScreen";
import BottomNavBar from "./BottomNavBar";

// Giả lập thêm vài màn hình khác
import ScheduleScreen from "../screens/ScheduleScreen";
import LookupScreen from "../screens/LookupScreen";
import SettingsScreen from "../screens/SettingsScreen";
import ServicesScreen from "../screens/ServicesScreen";

const Stack = createNativeStackNavigator();
const Tab = createBottomTabNavigator();

// -------- Bottom Tabs --------
function MainTabs({ setIsLoggedIn }) {
  return (
    <Tab.Navigator
      screenOptions={{ headerShown: false }}
      tabBar={(props) => <BottomNavBar {...props} />} // ✅ dùng thanh nav tùy chỉnh
    >
      <Tab.Screen name="Lookup" component={LookupScreen} options={{ tabBarLabel: "Tra cứu" }} />
      <Tab.Screen name="Services" component={ServicesScreen} options={{ tabBarLabel: "Dịch vụ" }} />
      <Tab.Screen name="Home">
        {() => <HomeScreen setIsLoggedIn={setIsLoggedIn} />}
      </Tab.Screen>
      <Tab.Screen name="Schedule" component={ScheduleScreen} options={{ tabBarLabel: "Lịch trình" }} />
      <Tab.Screen name="Settings" component={SettingsScreen} options={{ tabBarLabel: "Cài đặt" }} />
    </Tab.Navigator>
  );
}

// -------- Stack điều hướng chính --------
export default function AppNavigator({ isLoggedIn, setIsLoggedIn }) {
  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      {isLoggedIn ? (
        <Stack.Screen name="MainTabs">
          {() => <MainTabs setIsLoggedIn={setIsLoggedIn} />}
        </Stack.Screen>
      ) : (
        <Stack.Screen name="Login">
          {(props) => <LoginScreen {...props} setIsLoggedIn={setIsLoggedIn} />}
        </Stack.Screen>
      )}
    </Stack.Navigator>
  );
}
