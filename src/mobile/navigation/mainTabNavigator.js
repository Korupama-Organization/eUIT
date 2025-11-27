import React from "react";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { View, TouchableOpacity } from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../App";
// Import màn hình
import HomeScreen from "../features/users/screens/HomeScreen";
import LookupScreen from "../features/lookup/screens/LookupScreen";
import ServicesScreen from "../features/services/screens/ServicesScreen";
import ScheduleScreen from "../features/schedule/screens/ScheduleScreen";
import SettingsScreen from "../features/settings/screens/SettingsScreen";
// Import styles
import styles from "./tabNavigatorStyles";

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

export default MainTabNavigator;
