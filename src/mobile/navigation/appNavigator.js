import React from "react";
import { createStackNavigator } from "@react-navigation/stack";
import LoginScreen from "../features/auth/screens/LoginScreen";
import MainTabNavigator from "./mainTabNavigator";

const Stack = createStackNavigator();

export default function AppNavigator({ isLoggedIn, setIsLoggedIn }) {
  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      {isLoggedIn ? (
        <Stack.Screen name="MainApp">
          {() => <MainTabNavigator setIsLoggedIn={setIsLoggedIn} />}
        </Stack.Screen>
      ) : (
        <Stack.Screen name="Auth">
          {() => <LoginScreen setIsLoggedIn={setIsLoggedIn} />}
        </Stack.Screen>
      )}
    </Stack.Navigator>
  );
}
