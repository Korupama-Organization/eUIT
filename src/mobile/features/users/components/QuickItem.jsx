import React from "react";
import { TouchableOpacity, Text, StyleSheet } from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { homeStyles as styles } from "../styles/homeStyles";

/**
 * Component hiển thị mục truy cập nhanh
 */
export default function QuickItem({ icon, label, theme }) {
  return (
    <TouchableOpacity style={styles.quickItem}>
      <Ionicons name={icon} size={26} color={theme.primary} />
      <Text style={[styles.quickText, { color: theme.textSecondary }]}>
        {label}
      </Text>
    </TouchableOpacity>
  );
}
