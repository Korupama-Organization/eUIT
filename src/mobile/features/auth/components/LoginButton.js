import React from "react";
import { TouchableOpacity, Text, StyleSheet } from "react-native";

const LoginButton = ({ onPress, loading }) => {
  return (
    <TouchableOpacity
      style={[styles.button, loading && styles.buttonDisabled]}
      onPress={onPress}
      disabled={loading}
    >
      <Text style={styles.buttonText}>
        {loading ? "Đang đăng nhập..." : "Đăng nhập"}
      </Text>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  button: {
    backgroundColor: "#0066FF",
    borderRadius: 8,
    padding: 16,
    alignItems: "center",
    marginTop: 20,
  },
  buttonDisabled: {
    backgroundColor: "#99C2FF",
  },
  buttonText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "600",
  },
});

export default LoginButton;
