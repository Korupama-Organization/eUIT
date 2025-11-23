// src/mobile/navigation/MainTabNavigator.js
import React from "react";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { View, StyleSheet, TouchableOpacity } from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../App";

// Import màn hình
import HomeScreen from "../../features/auth/screens/HomeScreen";
import LookupScreen from "../../features/auth/screens/LookupScreen";
import ServicesScreen from "../../features/auth/screens/ServicesScreen";
import ScheduleScreen from "../../features/auth/screens/ScheduleScreen";
import SettingsScreen from "../../features/auth/screens/SettingsScreen";

const Tab = createBottomTabNavigator();

function MainTabNavigator({ setIsLoggedIn }) {
  const { theme } = useTheme();

  // Custom Tab Bar Button cho Home
  const CustomHomeTabButton = (props) => (
    <TouchableOpacity
      style={styles.centerButtonContainer}
      onPress={props.onPress}
      activeOpacity={0.8}
    >
      <View style={styles.centerButtonWrapper}>
        <View
          style={[
            styles.centerButton,
            styles.glowEffect,
            { backgroundColor: theme.primary },
          ]}
        >
          <Feather name="home" color={theme.accent} size={28} />
        </View>
      </View>
    </TouchableOpacity>
  );

  return (
    <Tab.Navigator
      screenOptions={{
        headerShown: false,
        tabBarShowLabel: true,
        tabBarStyle: [
          styles.tabBar,
          {
            backgroundColor: theme.tabBar || theme.card,
            borderTopColor: theme.border,
          },
        ],
        tabBarActiveTintColor: theme.tabActive,
        tabBarInactiveTintColor: theme.tabInactive,
        tabBarLabelStyle: styles.tabBarLabel,
      }}
    >
      <Tab.Screen
        name="Tra cứu"
        component={LookupScreen}
        options={{
          tabBarIcon: ({ color, focused }) => (
            <View
              style={[
                styles.iconContainer,
                focused && styles.iconContainerActive,
              ]}
            >
              <Feather name="search" color={color} size={24} />
            </View>
          ),
        }}
      />

      <Tab.Screen
        name="Dịch vụ"
        component={ServicesScreen}
        options={{
          tabBarIcon: ({ color, focused }) => (
            <View
              style={[
                styles.iconContainer,
                focused && styles.iconContainerActive,
              ]}
            >
              <Feather name="briefcase" color={color} size={24} />
            </View>
          ),
        }}
      />

      <Tab.Screen
        name="Trang chủ"
        options={{
          tabBarButton: (props) => <CustomHomeTabButton {...props} />,
        }}
      >
        {() => <HomeScreen setIsLoggedIn={setIsLoggedIn} />}
      </Tab.Screen>

      <Tab.Screen
        name="Lịch trình"
        component={ScheduleScreen}
        options={{
          tabBarIcon: ({ color, focused }) => (
            <View
              style={[
                styles.iconContainer,
                focused && styles.iconContainerActive,
              ]}
            >
              <Feather name="calendar" color={color} size={24} />
            </View>
          ),
        }}
      />

      <Tab.Screen
        name="Cài đặt"
        component={SettingsScreen}
        options={{
          tabBarIcon: ({ color, focused }) => (
            <View
              style={[
                styles.iconContainer,
                focused && styles.iconContainerActive,
              ]}
            >
              <Feather name="settings" color={color} size={24} />
            </View>
          ),
        }}
      />
    </Tab.Navigator>
  );
}

const styles = StyleSheet.create({
  tabBar: {
    position: "absolute",
    bottom: 0,
    left: 0,
    right: 0,
    elevation: 8,
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
    height: 80,
    paddingBottom: 10,
    paddingTop: 10,
    borderTopWidth: 1,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: -2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 8,
  },
  tabBarLabel: {
    fontSize: 11,
    fontWeight: "600",
    marginTop: 4,
  },
  iconContainer: {
    justifyContent: "center",
    alignItems: "center",
    width: 40,
    height: 40,
    borderRadius: 20,
  },
  iconContainerActive: {
    backgroundColor: "rgba(92, 225, 230, 0.1)",
  },
  centerButtonContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: -30,
  },
  centerButtonWrapper: {
    width: 70,
    height: 70,
    justifyContent: "center",
    alignItems: "center",
  },
  centerButton: {
    width: 60,
    height: 60,
    borderRadius: 30,
    justifyContent: "center",
    alignItems: "center",
    elevation: 8,
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 8,
  },
  glowEffect: {
    shadowColor: "#5ce1e6",
  },
});

export default MainTabNavigator;
