import { useState, useEffect, useRef, useMemo } from "react";
import { Animated, Easing, Alert } from "react-native";
import { login } from "../api/authAPI"; // Import từ API layer
import { USER_ROLES, AUTH_ERRORS } from "../types/auth.types";

/**
 * Custom hook xử lý tất cả logic của màn hình đăng nhập
 * @param {function} setIsLoggedIn - Hàm để cập nhật trạng thái đăng nhập
 * @returns {object} Các state và hàm cần thiết cho component UI
 */
export const useLogin = (setIsLoggedIn) => {
  // 1. State
  const [formData, setFormData] = useState({
    role: USER_ROLES.STUDENT,
    userId: "",
    password: "",
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [isDark, setIsDark] = useState(false);

  // 2. Animation
  const fadeAnim = useRef(new Animated.Value(0)).current;
  const slideAnim = useRef(new Animated.Value(30)).current;

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
  }, [fadeAnim, slideAnim]);

  // 3. Handlers & Validation
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
      await login(formData);
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

  return {
    formData,
    errors,
    loading,
    isDark,
    fadeAnim,
    slideAnim,
    handleInputChange,
    handleLogin,
    setIsDark,
  };
};
