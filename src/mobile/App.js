import React, { useEffect, useState } from 'react';
import { View, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import { Feather } from '@expo/vector-icons';

// Import màn hình và nút tùy chỉnh
import HomeScreen from './screens/HomeScreen';
import TabBarCustomButton from './components/TabBarCustomButton';

// Import Auth screens
import LoginScreen from './features/auth/screens/LoginScreen';
import { isAuthenticated } from './features/auth/api/authAPI';

// Các màn hình giả
function ServicesScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Services Screen</Text></View>; }
function ScheduleScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Schedule Screen</Text></View>; }
function LookupScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Lookup Screen</Text></View>; }
function SettingsScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Settings Screen</Text></View>; }

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

// Main App với bottom tabs
function MainApp() {
  return (
    <Tab.Navigator
      screenOptions={{
        headerShown: false,
        tabBarShowLabel: false, 
        tabBarStyle: {
          position: 'absolute',
          bottom: 25,
          left: 20,
          right: 20,
          elevation: 0,
          backgroundColor: '#1E1E3D',
          borderRadius: 15,
          height: 70,
          borderTopWidth: 0,
        },
        tabBarActiveTintColor: '#FFFFFF',
        tabBarInactiveTintColor: '#A0A0A0',
      }}
    >
      <Tab.Screen name="Lookup" component={LookupScreen} options={{ tabBarIcon: ({ color }) => <Feather name="search" color={color} size={24} /> }} />
      <Tab.Screen name="Services" component={ServicesScreen} options={{ tabBarIcon: ({ color }) => <Feather name="briefcase" color={color} size={24} /> }} />
      <Tab.Screen 
        name="Home" 
        component={HomeScreen} 
        options={{
          tabBarIcon: () => null, 
          tabBarButton: (props) => (
            <TabBarCustomButton {...props} />
          ),
        }}
      />
      <Tab.Screen name="Schedule" component={ScheduleScreen} options={{ tabBarIcon: ({ color }) => <Feather name="calendar" color={color} size={24} /> }} />
      <Tab.Screen name="Settings" component={SettingsScreen} options={{ tabBarIcon: ({ color }) => <Feather name="settings" color={color} size={24} /> }} />
    </Tab.Navigator>
  );
}

// Auth Stack Navigator
function AuthStack() {
  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      <Stack.Screen name="Login" component={LoginScreen} />
    </Stack.Navigator>
  );
}

// Root App Component với auth check
export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
      const authenticated = await isAuthenticated();
      setIsLoggedIn(authenticated);
    } catch (error) {
      console.error('Auth check error:', error);
      setIsLoggedIn(false);
    } finally {
      setIsLoading(false);
    }
  };

  // Loading screen
  if (isLoading) {
    return (
      <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: '#FFFFFF' }}>
        <Text style={{ fontSize: 18, color: '#6B7280' }}>Đang kiểm tra đăng nhập...</Text>
      </View>
    );
  }

  return (
    <NavigationContainer>
      {isLoggedIn ? <MainApp /> : <AuthStack />}
    </NavigationContainer>
  );
}