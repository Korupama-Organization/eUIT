import React from "react";
import { View, Text, StyleSheet } from "react-native";

export default function ScheduleScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.text}>📅 Màn hình Lịch trình</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "#09092A",
  },
  text: {
    color: "#7AF8FF",
    fontSize: 18,
    fontWeight: "600",
  },
});
