import React from "react";
import { View, Text, StyleSheet } from "react-native";
import { useTheme } from "../../../App";

export default function ServicesScreen() {
  const { theme } = useTheme();

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      <Text style={[styles.title, { color: theme.textPrimary }]}>Dịch vụ</Text>
      {/* Nội dung khác */}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: "center", alignItems: "center" },
  title: { fontSize: 24, fontWeight: "bold" },
});
