import React from "react";
import { View, Text, StyleSheet } from "react-native";

export default function LookupScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.text}>🔍 Màn hình Tra cứu</Text>
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


