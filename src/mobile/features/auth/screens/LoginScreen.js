import React, { useState } from "react";
import { View, Text, StyleSheet, Alert } from "react-native";
import { AuthApi } from "../api/authAPI";
import LoginButton from "../components/LoginButton";
import InputField from "../components/InputField";

const LoginScreen = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    if (!email || !password) {
      Alert.alert("Lỗi", "Vui lòng nhập đầy đủ thông tin");
      return;
    }

    setLoading(true);
    const result = await AuthApi.login(email, password);
    setLoading(false);

    if (result.success) {
      Alert.alert("Thành công", "Đăng nhập thành công!");
      console.log("Login data:", result.data);
      // TODO: Navigate or save token
    } else {
      Alert.alert("Lỗi", result.error);
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.content}>
        <Text style={styles.title}>Đăng Nhập</Text>

        <InputField
          label="Nhập mail"
          value={email}
          onChangeText={setEmail}
          placeholder="example@uit.edu.vn"
          icon="✉"
        />

        <InputField
          label="Mật khẩu"
          value={password}
          onChangeText={setPassword}
          placeholder="Nhập mật khẩu"
          secureTextEntry={true}
          showPassword={showPassword}
          onTogglePassword={() => setShowPassword(!showPassword)}
          icon="🔒"
        />

        <LoginButton onPress={handleLogin} loading={loading} />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
    justifyContent: "center",
    padding: 24,
  },
  content: {
    width: "100%",
    maxWidth: 400,
    alignSelf: "center",
  },
  title: {
    fontSize: 28,
    fontWeight: "bold",
    color: "#0066FF",
    marginBottom: 40,
  },
});

export default LoginScreen;
