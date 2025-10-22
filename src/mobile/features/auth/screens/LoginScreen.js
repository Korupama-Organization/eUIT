import React, { useState, useRef, useEffect } from "react";
import {
  View,
  Text,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
  Alert,
  StyleSheet,
  SafeAreaView,
  StatusBar,
  TouchableOpacity,
  Animated,
  Easing,
} from "react-native";
import { Picker } from "@react-native-picker/picker";
import { Sun, Moon } from "lucide-react-native";
import InputField from "../components/InputField";
import LoginButton from "../components/LoginButton";
import { login } from "../api/authAPI";
import { USER_ROLES, AUTH_ERRORS } from "../types/auth.types";

const LoginScreen = ({ navigation, setIsLoggedIn }) => {
  const [formData, setFormData] = useState({
    role: USER_ROLES.STUDENT,
    userId: "",
    password: "",
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [isDark, setIsDark] = useState(false);

  // Animation refs
  const fadeAnim = useRef(new Animated.Value(0)).current;
  const slideAnim = useRef(new Animated.Value(30)).current;

  const theme = isDark ? darkTheme : lightTheme;

  useEffect(() => {
    Animated.parallel([
      Animated.timing(fadeAnim, {
        toValue: 1,
        duration: 700,
        easing: Easing.out(Easing.ease),
        useNativeDriver: true,
      }),
      Animated.timing(slideAnim, {
        toValue: 0,
        duration: 700,
        easing: Easing.out(Easing.ease),
        useNativeDriver: true,
      }),
    ]).start();
  }, []);

  const handleInputChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors[field]) setErrors((prev) => ({ ...prev, [field]: "" }));
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.userId.trim()) {
      newErrors.userId = "Vui lòng nhập mã số";
    } else if (
      formData.role === USER_ROLES.STUDENT &&
      (!/^\d+$/.test(formData.userId) || formData.userId.length < 8)
    ) {
      newErrors.userId = "MSSV không hợp lệ (ít nhất 8 số)";
    } else if (
      formData.role === USER_ROLES.LECTURER &&
      formData.userId.length < 3
    ) {
      newErrors.userId = "Mã giảng viên không hợp lệ";
    }

    if (!formData.password) {
      newErrors.password = "Vui lòng nhập mật khẩu";
    } else if (formData.password.length < 6) {
      newErrors.password = "Mật khẩu phải có ít nhất 6 ký tự";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

const handleLogin = async () => {
  if (!validateForm()) return;
  setLoading(true);

  try {
    const result = await login(formData);

    Alert.alert(
      "✅ Đăng nhập thành công!",
      `Chào mừng bạn đăng nhập với vai trò ${getRoleDisplayName(
        formData.role
      )}`,
      [
        {
          text: "OK",
          onPress: () => {
            console.log("Login success:", result);
            setIsLoggedIn(true); // ✅ Cập nhật trạng thái đăng nhập ở App.js
          },
        },
      ]
    );
  } catch (error) {
    let message = "Có lỗi xảy ra, vui lòng thử lại.";
    switch (error.message) {
      case AUTH_ERRORS.INVALID_CREDENTIALS:
        message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
        break;
      case AUTH_ERRORS.NETWORK_ERROR:
        message = "Không thể kết nối đến server.";
        break;
      case AUTH_ERRORS.SERVER_ERROR:
        message = "Lỗi server. Vui lòng thử lại sau.";
        break;
      default:
        message = error.message || message;
    }
    Alert.alert("❌ Đăng nhập thất bại", message);
  } finally {
    setLoading(false);
  }
};
  const getRoleDisplayName = (role) => {
    switch (role) {
      case USER_ROLES.STUDENT:
        return "Sinh viên";
      case USER_ROLES.LECTURER:
        return "Giảng viên";
      case USER_ROLES.ADMIN:
        return "Quản trị viên";
      default:
        return role;
    }
  };

  const getUserIdPlaceholder = () => {
    switch (formData.role) {
      case USER_ROLES.STUDENT:
        return "Nhập MSSV (VD: 23520541)";
      case USER_ROLES.LECTURER:
        return "Nhập mã giảng viên (VD: 80068)";
      case USER_ROLES.ADMIN:
        return "Nhập tài khoản admin";
      default:
        return "Nhập mã số";
    }
  };

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: theme.bg }]}>
      <StatusBar
        barStyle={isDark ? "light-content" : "dark-content"}
        backgroundColor={theme.bg}
      />

      {/* Toggle sáng/tối */}
      <TouchableOpacity
        onPress={() => setIsDark(!isDark)}
        style={styles.toggleBtn}
      >
        {isDark ? (
          <Sun color={theme.primary} size={22} strokeWidth={2} />
        ) : (
          <Moon color={theme.primary} size={22} strokeWidth={2} />
        )}
      </TouchableOpacity>

      <KeyboardAvoidingView
        behavior={Platform.OS === "ios" ? "padding" : "height"}
        style={styles.keyboardAvoidingView}
      >
        <ScrollView
          contentContainerStyle={styles.scrollContainer}
          keyboardShouldPersistTaps="handled"
        >
          <Animated.View
            style={[
              styles.card,
              {
                backgroundColor: theme.card,
                shadowColor: theme.shadow,
                opacity: fadeAnim,
                transform: [{ translateY: slideAnim }],
              },
            ]}
          >
            {/* Header */}
            <View style={styles.header}>
              <Text style={[styles.title, { color: theme.primary }]}>
                Đăng nhập
              </Text>
              <Text style={[styles.subtitle, { color: theme.subtext }]}>
                Vui lòng nhập thông tin để đăng nhập
              </Text>
            </View>

            {/* Role Picker */}
            <View style={styles.roleContainer}>
              <Text style={[styles.roleLabel, { color: theme.text }]}>
                Vai trò
              </Text>
              <View
                style={[
                  styles.pickerContainer,
                  { borderColor: theme.border, backgroundColor: theme.bg },
                ]}
              >
                <Picker
                  selectedValue={formData.role}
                  onValueChange={(value) =>
                    handleInputChange("role", value)
                  }
                  style={[styles.picker, { color: theme.text }]}
                >
                  <Picker.Item
                    label="👨‍🎓 Sinh viên"
                    value={USER_ROLES.STUDENT}
                  />
                  <Picker.Item
                    label="👨‍🏫 Giảng viên"
                    value={USER_ROLES.LECTURER}
                  />
                  <Picker.Item
                    label="👨‍💼 Quản trị viên"
                    value={USER_ROLES.ADMIN}
                  />
                </Picker>
              </View>
            </View>

            {/* Inputs */}
            <InputField
              label="Mã số"
              value={formData.userId}
              onChangeText={(v) => handleInputChange("userId", v)}
              placeholder={getUserIdPlaceholder()}
              keyboardType={
                formData.role === USER_ROLES.STUDENT ? "numeric" : "default"
              }
              error={errors.userId}
              themeColor={theme.primary}
              textColor={theme.text}
              placeholderColor={theme.placeholder}
            />

            <InputField
              label="Mật khẩu"
              value={formData.password}
              onChangeText={(v) => handleInputChange("password", v)}
              placeholder="Nhập mật khẩu"
              secureTextEntry
              showPasswordToggle
              error={errors.password}
              themeColor={theme.primary}
              textColor={theme.text}
              placeholderColor={theme.placeholder}
            />

            <LoginButton
              title={loading ? "Đang đăng nhập..." : "Đăng nhập"}
              onPress={handleLogin}
              loading={loading}
              disabled={loading}
              bgColor={theme.buttonBg}
              textColor={theme.buttonText}
              shadowColor={theme.shadow}
              style={{ marginTop: 12 }}
            />

            {/* Footer */}
            <View style={styles.footer}>
              <Text style={[styles.footerText, { color: theme.subtext }]}>
                Bạn quên mật khẩu?{" "}
                <Text style={[styles.footerLink, { color: theme.link }]}>
                  Khôi phục tại đây
                </Text>
              </Text>
            </View>
          </Animated.View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
};

const lightTheme = {
  bg: "#FFFFFF",
  card: "#F9FAFB",
  primary: "#0032AF",
  text: "#1F2937",
  subtext: "#6B7280",
  placeholder: "#9AA5C4",
  border: "#D1D5DB",
  buttonBg: "#2F6BFF",
  buttonText: "#FFFFFF",
  link: "#3B82F6",
  shadow: "#0032AF",
};

const darkTheme = {
  bg: "#09092A",
  card: "#121232",
  primary: "#FFFFFF",
  text: "#E5E7EB",
  subtext: "#9CA3AF",
  placeholder: "#9CA3AF",
  border: "#1F2937",
  buttonBg: "#7AF8FF",
  buttonText: "#000000",
  link: "#7AF8FF",
  shadow: "#7AF8FF",
};

const styles = StyleSheet.create({
  container: { flex: 1 },
  toggleBtn: { position: "absolute", top: 55, right: 25, zIndex: 10 },
  keyboardAvoidingView: { flex: 1 },
  scrollContainer: { flexGrow: 1, padding: 24, justifyContent: "center" },
  card: {
    borderRadius: 18,
    padding: 28,
    elevation: 6,
    shadowOpacity: 0.3,
    shadowOffset: { width: 0, height: 4 },
    shadowRadius: 12,
  },
  header: { alignItems: "center", marginBottom: 32 },
  title: { fontSize: 30, fontWeight: "700", fontFamily: "Inter" },
  subtitle: { fontSize: 16, textAlign: "center" },
  roleContainer: { marginBottom: 18 },
  roleLabel: { fontSize: 14, fontWeight: "500", marginBottom: 6 },
  pickerContainer: { borderWidth: 1, borderRadius: 10 },
  picker: { height: 48 },
  footer: { alignItems: "center", marginTop: 24 },
  footerText: { fontSize: 14, textAlign: "center" },
  footerLink: { fontWeight: "500" },
});

export default LoginScreen;
