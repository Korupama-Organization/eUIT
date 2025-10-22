import React from 'react';
import { View, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Feather } from '@expo/vector-icons';

// Import màn hình và nút tùy chỉnh
import HomeScreen from './screens/HomeScreen';
import TabBarCustomButton from './components/TabBarCustomButton';

// Các màn hình giả
function ServicesScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Services Screen</Text></View>; }
function ScheduleScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Schedule Screen</Text></View>; }
import LookUpScreen from './screens/LookUpScreen';
function SettingsScreen() { return <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}><Text>Settings Screen</Text></View>; }

const Tab = createBottomTabNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <View style={{ flex: 1, backgroundColor: '#030E2C' }}>
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
          <Tab.Screen
            name="Lookup"
            component={LookUpScreen}
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
            component={HomeScreen}
            options={{
              tabBarIcon: () => null,
              tabBarButton: (props) => <TabBarCustomButton {...props} />,
            }}
          />
          <Tab.Screen
            name="Schedule"
            component={ScheduleScreen}
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
      </View>
    </NavigationContainer>
  );
}
