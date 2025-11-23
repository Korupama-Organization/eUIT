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
  Image,
} from "react-native";
import { Sun, Moon } from "lucide-react-native";
import InputField from "../components/InputField";
import LoginButton from "../components/LoginButton";
import { login } from "../api/authAPI";
import { USER_ROLES, AUTH_ERRORS } from "../types/auth.types";

import uitLogo from "../../../assets/UITLogo.png";

const LoginScreen = ({ navigation, setIsLoggedIn }) => {
  const [formData, setFormData] = useState({
    role: USER_ROLES.STUDENT,
    userId: "",
    password: "",
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [isDark, setIsDark] = useState(false);

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
        "Đăng nhập thành công!",
        `Chào mừng bạn đăng nhập với vai trò ${
          formData.role === USER_ROLES.STUDENT ? "Sinh viên" : "Giảng viên"
        }`,
        [
          {
            text: "OK",
            onPress: () => {
              setIsLoggedIn(true);
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
      Alert.alert("Đăng nhập thất bại", message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: theme.bg }]}>
      <StatusBar
        barStyle={isDark ? "light-content" : "dark-content"}
        backgroundColor={theme.bg}
      />

      {/* Nút đổi theme cố định góc phải */}
      <View style={styles.headerContainer}>
        <TouchableOpacity
          onPress={() => setIsDark(!isDark)}
          style={styles.toggleBtn}
        >
          {isDark ? (
            <Sun color={theme.primary} size={22} />
          ) : (
            <Moon color={theme.primary} size={22} />
          )}
        </TouchableOpacity>
      </View>

      {/*Phần nội dung chính */}
      <KeyboardAvoidingView
        behavior={Platform.OS === "ios" ? "padding" : "height"}
        style={styles.keyboardAvoidingView}
      >
        <ScrollView
          contentContainerStyle={styles.scrollContainer}
          keyboardShouldPersistTaps="handled"
        >
          {/* 🔹 Logo UIT giữa phía trên card */}
          <View style={styles.logoTopContainer}>
            <Image
              source={uitLogo}
              style={styles.logoTop}
              resizeMode="contain"
            />
          </View>

          {/* 🔹 Card chính */}
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
            {/* Toggle Vai trò */}
            <View
              style={[
                styles.roleToggleContainer,
                { backgroundColor: isDark ? "#0C1445" : "#E5E7EB" },
              ]}
            >
              {[
                { key: USER_ROLES.STUDENT, label: "Sinh viên" },
                { key: USER_ROLES.LECTURER, label: "Giảng viên" },
              ].map((item) => (
                <TouchableOpacity
                  key={item.key}
                  style={[
                    styles.roleButton,
                    {
                      backgroundColor:
                        formData.role === item.key
                          ? theme.buttonBg
                          : "transparent",
                    },
                  ]}
                  onPress={() => handleInputChange("role", item.key)}
                >
                  <Text
                    style={{
                      color:
                        formData.role === item.key
                          ? theme.buttonText
                          : theme.text,
                      fontWeight: "600",
                    }}
                  >
                    {item.label}
                  </Text>
                </TouchableOpacity>
              ))}
            </View>

            {/* Input fields */}
            <InputField
              label="Mã số"
              value={formData.userId}
              onChangeText={(v) => handleInputChange("userId", v)}
              placeholder={
                formData.role === USER_ROLES.STUDENT
                  ? "Nhập MSSV (VD: 23520541)"
                  : "Nhập mã giảng viên (VD: 80068)"
              }
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

// ================= THEME =================
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

// ================= STYLES =================
const styles = StyleSheet.create({
  container: {
    flex: 1,
  },

  scrollContainer: {
    flexGrow: 1,
    justifyContent: "flex-start",
    alignItems: "center",
    paddingTop: 120,
    paddingBottom: 40,
  },

  logoTopContainer: {
    alignItems: "center",
    marginBottom: 20,
  },
  logoTop: {
    width: 120,
    height: 120,
  },
  card: {
    width: "90%",
    maxWidth: 420,
    borderRadius: 18,
    padding: 28,
    elevation: 6,
    shadowOpacity: 0.3,
    shadowOffset: { width: 0, height: 4 },
    shadowRadius: 12,
  },
  headerContainer: {
    position: "absolute",
    top: 50,
    right: 25,
    zIndex: 20,
  },
  toggleBtn: {
    padding: 6,
  },
  roleToggleContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    borderRadius: 10,
    padding: 3,
    marginBottom: 24,
  },
  roleButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 6,
    alignItems: "center",
    marginHorizontal: 2,
  },
  footer: {
    alignItems: "center",
    marginTop: 24,
  },
  footerText: {
    fontSize: 14,
    textAlign: "center",
  },
  footerLink: {
    fontWeight: "500",
  },
});

export default LoginScreen;
