import React from "react";
import { View, Text, TouchableOpacity, StyleSheet, useColorScheme } from "react-native";

export default function HomeScreen({ setIsLoggedIn }) {
  const scheme = useColorScheme();
  const isDark = scheme === "dark";

  const theme = isDark ? darkTheme : lightTheme;

  return (
    <View style={[styles.container, { backgroundColor: theme.bg }]}>
      <Text style={[styles.title, { color: theme.primary }]}>🏠 Trang chính eUIT</Text>
      <Text style={[styles.subtitle, { color: theme.text }]}>
        Chào mừng bạn đến với ứng dụng sinh viên UIT
      </Text>

      {/* Nút đăng xuất */}
      <TouchableOpacity
        style={[styles.logoutBtn, { backgroundColor: theme.primary }]}
        onPress={() => setIsLoggedIn(false)} // 🔥 đăng xuất
      >
        <Text style={[styles.logoutText, { color: theme.buttonText }]}>
          Đăng xuất
        </Text>
      </TouchableOpacity>
    </View>
  );
}

const lightTheme = {
  bg: "#FFFFFF",
  primary: "#0032AF",
  text: "#374151",
  buttonText: "#FFFFFF",
};

const darkTheme = {
  bg: "#09092A",
  primary: "#7AF8FF",
  text: "#E5E7EB",
  buttonText: "#000000",
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  title: {
    fontSize: 24,
    fontWeight: "700",
    fontFamily: "Inter",
    marginBottom: 10,
  },
  subtitle: {
    fontSize: 16,
    fontWeight: "400",
    marginBottom: 40,
  },
  logoutBtn: {
    paddingVertical: 12,
    paddingHorizontal: 30,
    borderRadius: 10,
    elevation: 4,
  },
  logoutText: {
    fontSize: 16,
    fontWeight: "600",
  },
});
