import React from "react";
import { View, Text, Switch, StyleSheet } from "react-native";
import { useTheme } from "../App";
import { Feather } from "@expo/vector-icons";

export default function SettingsScreen() {
  const { theme, toggleTheme } = useTheme();
  const isDark = theme.background === "#09092A";

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      <Text style={[styles.title, { color: theme.textPrimary }]}>
        Cài đặt hiển thị
      </Text>
      <View style={styles.row}>
        <Feather name="sun" size={20} color={theme.textSecondary} />
        <Text style={[styles.label, { color: theme.textPrimary }]}>Chế độ sáng / tối</Text>
        <Switch
          trackColor={{ false: "#ccc", true: "#7AF8FF" }}
          thumbColor={isDark ? "#2F6BFF" : "#0032AF"}
          onValueChange={toggleTheme}
          value={!isDark}
        />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 24 },
  title: { fontSize: 18, fontWeight: "700", marginBottom: 20 },
  row: { flexDirection: "row", alignItems: "center", justifyContent: "space-between" },
  label: { fontSize: 16, fontWeight: "500" },
});
