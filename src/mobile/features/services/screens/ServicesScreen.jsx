import React from "react";
import { View, Text } from "react-native";
import { useTheme } from "../../../App";

// Import styles đã tách
import { servicesStyles as styles } from "../styles/servicesStyles";

export default function ServicesScreen() {
  const { theme } = useTheme();

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      <Text style={[styles.title, { color: theme.textPrimary }]}>Dịch vụ</Text>
      {/* Nội dung khác */}
    </View>
  );
}
