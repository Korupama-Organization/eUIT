import React from "react";
import {
  View,
  Text,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
  SafeAreaView,
  StatusBar,
  TouchableOpacity,
  Animated,
  Image,
} from "react-native";
import { Sun, Moon } from "lucide-react-native";

// Imports từ các file đã tách
import { useLogin } from "../hooks/useLogin";
import { lightTheme, darkTheme } from "../constants/authThemes";
import { loginStyles as styles } from "../styles/loginStyles";

// Imports components và constants
import InputField from "../components/InputField";
import LoginButton from "../components/LoginButton";
import { USER_ROLES } from "../types/auth.types";
import uitLogo from "../../../assets/UITLogo.png";

const LoginScreen = ({ navigation, setIsLoggedIn }) => {
  // 💥 CHỈ CẦN GỌI HOOK ĐỂ LẤY LOGIC VÀ STATE
  const {
    formData,
    errors,
    loading,
    isDark,
    fadeAnim,
    slideAnim,
    handleInputChange,
    handleLogin,
    setIsDark,
  } = useLogin(setIsLoggedIn);

  const theme = isDark ? darkTheme : lightTheme;

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
        style={styles.container} // style.container đã có flex: 1
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

export default LoginScreen;
