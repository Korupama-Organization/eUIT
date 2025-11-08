import React, { useRef } from "react";
import { View, Text, TouchableWithoutFeedback, Animated, StyleSheet } from "react-native";
import { FontAwesome5 } from "@expo/vector-icons";
import { useTheme } from "../App";

const quickActions = [
  { id: 1, label: "Kết quả học tập", icon: "graduation-cap" },
  { id: 2, label: "Thời khóa biểu", icon: "calendar" },
  { id: 3, label: "Học phí", icon: "dollar-sign" },
  { id: 4, label: "Gửi xe", icon: "car" },
  { id: 5, label: "Phúc khảo", icon: "file-alt" },
  { id: 6, label: "Đăng ký GXN", icon: "edit" },
  { id: 7, label: "Giấy giới thiệu", icon: "file" },
  { id: 8, label: "Chứng chỉ", icon: "globe" },
];

export default function QuickActions() {
  const { theme } = useTheme();
  return (
    <View style={styles.grid}>
      {quickActions.map((item) => (
        <ActionButton key={item.id} icon={item.icon} label={item.label} theme={theme} />
      ))}
    </View>
  );
}

function ActionButton({ icon, label, theme }) {
  const scale = useRef(new Animated.Value(1)).current;
  const glow = useRef(new Animated.Value(0)).current;

  const onPressIn = () => {
    Animated.parallel([
      Animated.spring(scale, { toValue: 1.1, useNativeDriver: true }),
      Animated.timing(glow, { toValue: 1, duration: 150, useNativeDriver: false }),
    ]).start();
  };

  const onPressOut = () => {
    Animated.parallel([
      Animated.spring(scale, { toValue: 1, useNativeDriver: true }),
      Animated.timing(glow, { toValue: 0, duration: 150, useNativeDriver: false }),
    ]).start();
  };

  const glowStyle = {
    shadowColor: theme.accent,
    shadowOpacity: glow.interpolate({ inputRange: [0, 1], outputRange: [0.15, 0.6] }),
    shadowRadius: glow.interpolate({ inputRange: [0, 1], outputRange: [4, 10] }),
    shadowOffset: { width: 0, height: 2 },
  };

  return (
    <TouchableWithoutFeedback onPressIn={onPressIn} onPressOut={onPressOut}>
      <Animated.View style={[styles.actionItem, { transform: [{ scale }] }]}>
        <Animated.View style={[styles.iconCircle, 
          { backgroundColor: theme.quickIconBg,
            borderColor: theme.name === "light" ? theme.textSecondary : theme.border,
            shadowColor: theme.name === "light" ? theme.textSecondary : "#000",
            shadowOpacity: theme.name === "light" ? 0.1 : 0.3,
            shadowRadius: 4,
            shadowOffset: { width: 0, height: 1 },
          }, 
          glowStyle]}>
          <FontAwesome5 name={icon} size={20} color={theme.textSecondary} />
        </Animated.View>
        <Text style={[styles.label, { color: theme.textSecondary }]}>{label}</Text>
      </Animated.View>
    </TouchableWithoutFeedback>
  );
}

const styles = StyleSheet.create({
  grid: {
    flexDirection: "row",
    flexWrap: "wrap",
    justifyContent: "space-between",
    paddingHorizontal: 6,
    rowGap: 20,
  },
  actionItem: {
    width: "22%",
    alignItems: "center",
  },
  iconCircle: {
    width: 66,
    height: 66,
    borderRadius: 33,
    justifyContent: "center",
    alignItems: "center",
    marginBottom: 8,
    borderWidth: 1,
  },
  label: {
    fontSize: 12,
    fontWeight: "600",
    textAlign: "center",
    lineHeight: 16,
    fontFamily: "Inter",
  },
});
