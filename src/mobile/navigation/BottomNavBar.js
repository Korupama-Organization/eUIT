import React from "react";
import { View, TouchableOpacity, Text, StyleSheet } from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../App";

export default function BottomNavBar({ state, descriptors, navigation }) {
  const { theme } = useTheme();

  return (
    <View style={[styles.tabBar, { backgroundColor: theme.tabBar }]}>
      {state.routes.map((route, index) => {
        const isFocused = state.index === index;
        const { options } = descriptors[route.key];
        const iconName = {
          Lookup: "search",
          Services: "briefcase",
          Home: "home",
          Schedule: "calendar",
          Settings: "settings",
        }[route.name];

        const onPress = () => {
          const event = navigation.emit({ type: "tabPress", target: route.key });
          if (!isFocused && !event.defaultPrevented) navigation.navigate(route.name);
        };

        if (route.name === "Home") {
          return (
            <TouchableOpacity key={route.name} onPress={onPress} style={styles.centerButton}>
              <View
                style={[
                  styles.homeButton,
                  {
                    backgroundColor: theme.accent,
                    shadowColor: theme.accent,
                    shadowOpacity: 0.6,
                  },
                ]}
              >
                <Feather name="home" size={26} color={theme.background} />
              </View>
            </TouchableOpacity>
          );
        }

        return (
          <TouchableOpacity
            key={route.name}
            onPress={onPress}
            style={styles.tabItem}
          >
            <Feather
              name={iconName}
              size={22}
              color={isFocused ? theme.accent : theme.icon}
            />
            <Text
              style={[
                styles.label,
                {
                  color: isFocused ? theme.accent : theme.textPrimary,
                },
              ]}
            >
              {options.tabBarLabel || route.name}
            </Text>
          </TouchableOpacity>
        );
      })}
    </View>
  );
}

const styles = StyleSheet.create({
  tabBar: {
    position: "absolute",
    bottom: 5,            
    left: 5,
    right: 5,
    height: 80,
    borderRadius: 30,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingHorizontal: 24,
    elevation: 10,
  },
  tabItem: {
    alignItems: "center",
    justifyContent: "center",
    gap: 4,
  },
  label: {
    fontSize: 12,
    fontFamily: "Inter",
    fontWeight: "500",
    lineHeight: 16,
  },
  centerButton: {
    alignItems: "center",
    justifyContent: "center",
  },
  homeButton: {
    width: 64,
    height: 64,
    borderRadius: 32,
    alignItems: "center",
    justifyContent: "center",
    shadowRadius: 15,
    shadowOffset: { width: 0, height: 5 },
  },
});
