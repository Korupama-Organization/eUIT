import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
  Alert,
  StyleSheet,
  SafeAreaView,
  StatusBar
} from 'react-native';
import { Picker } from '@react-native-picker/picker';

import InputField from '../components/InputField';
import LoginButton from '../components/LoginButton';
import { login } from '../api/authAPI';
import { USER_ROLES, AUTH_ERRORS } from '../types/auth.types';

/**
 * LoginScreen Component - Màn hình đăng nhập
 */
const LoginScreen = ({ navigation }) => {
  // States
  const [formData, setFormData] = useState({
    role: USER_ROLES.STUDENT,
    userId: '',
    password: ''
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  // Handlers
  const handleInputChange = (field, value) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
    
    // Xóa lỗi khi user nhập lại
    if (errors[field]) {
      setErrors(prev => ({
        ...prev,
        [field]: ''
      }));
    }
  };

  const validateForm = () => {
    const newErrors = {};

    if (!formData.userId.trim()) {
      newErrors.userId = 'Vui lòng nhập mã số';
    } else {
      // Validate theo role
      if (formData.role === USER_ROLES.STUDENT) {
        // MSSV phải là số và có độ dài phù hợp
        if (!/^\d+$/.test(formData.userId) || formData.userId.length < 8) {
          newErrors.userId = 'MSSV không hợp lệ (ít nhất 8 số)';
        }
      } else if (formData.role === USER_ROLES.LECTURER) {
        // Mã giảng viên có thể chứa chữ và số
        if (formData.userId.length < 3) {
          newErrors.userId = 'Mã giảng viên không hợp lệ';
        }
      }
    }

    if (!formData.password) {
      newErrors.password = 'Vui lòng nhập mật khẩu';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Mật khẩu phải có ít nhất 6 ký tự';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleLogin = async () => {
    if (!validateForm()) {
      return;
    }

    setLoading(true);

    try {
      const result = await login(formData);
      
      // Đăng nhập thành công
      Alert.alert(
        '✅ Đăng nhập thành công!',
        `Chào mừng bạn đã đăng nhập với vai trò ${getRoleDisplayName(formData.role)}`,
        [
          {
            text: 'OK',
            onPress: () => {
              // TODO: Navigate to main app screen
              console.log('Login success:', result);
            }
          }
        ]
      );

    } catch (error) {
      console.error('Login error:', error);
      
      let errorMessage = 'Có lỗi xảy ra, vui lòng thử lại.';
      
      switch (error.message) {
        case AUTH_ERRORS.INVALID_CREDENTIALS:
          errorMessage = 'Tên đăng nhập hoặc mật khẩu không chính xác.';
          break;
        case AUTH_ERRORS.NETWORK_ERROR:
          errorMessage = 'Không thể kết nối đến server. Vui lòng kiểm tra mạng.';
          break;
        case AUTH_ERRORS.SERVER_ERROR:
          errorMessage = 'Lỗi server. Vui lòng thử lại sau.';
          break;
        default:
          errorMessage = error.message || errorMessage;
      }

      Alert.alert('❌ Đăng nhập thất bại', errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const getRoleDisplayName = (role) => {
    switch (role) {
      case USER_ROLES.STUDENT:
        return 'Sinh viên';
      case USER_ROLES.LECTURER:
        return 'Giảng viên';
      case USER_ROLES.ADMIN:
        return 'Quản trị viên';
      default:
        return role;
    }
  };

  const getUserIdPlaceholder = () => {
    switch (formData.role) {
      case USER_ROLES.STUDENT:
        return 'Nhập MSSV (ví dụ: 23520541)';
      case USER_ROLES.LECTURER:
        return 'Nhập mã giảng viên (ví dụ: 80068)';
      case USER_ROLES.ADMIN:
        return 'Nhập tài khoản admin';
      default:
        return 'Nhập mã số';
    }
  };

  const getUserIdKeyboardType = () => {
    return formData.role === USER_ROLES.STUDENT ? 'numeric' : 'default';
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" backgroundColor="#FFFFFF" />
      
      <KeyboardAvoidingView
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.keyboardAvoidingView}
      >
        <ScrollView 
          contentContainerStyle={styles.scrollContainer}
          keyboardShouldPersistTaps="handled"
        >
          {/* Header */}
          <View style={styles.header}>
            <Text style={styles.title}>Đăng nhập</Text>
            <Text style={styles.subtitle}>Vui lòng nhập thông tin để đăng nhập</Text>
          </View>

          {/* Form */}
          <View style={styles.form}>
            {/* Role Selector */}
            <View style={styles.roleContainer}>
              <Text style={styles.roleLabel}>Vai trò</Text>
              <View style={styles.pickerContainer}>
                <Picker
                  selectedValue={formData.role}
                  onValueChange={(value) => handleInputChange('role', value)}
                  style={styles.picker}
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

            {/* User ID Input */}
            <InputField
              label="Mã số"
              value={formData.userId}
              onChangeText={(value) => handleInputChange('userId', value)}
              placeholder={getUserIdPlaceholder()}
              keyboardType={getUserIdKeyboardType()}
              error={errors.userId}
            />

            {/* Password Input */}
            <InputField
              label="Mật khẩu"
              value={formData.password}
              onChangeText={(value) => handleInputChange('password', value)}
              placeholder="Nhập mật khẩu"
              secureTextEntry={true}
              showPasswordToggle={true}
              error={errors.password}
            />

            {/* Login Button */}
            <LoginButton
              title={loading ? 'Đang đăng nhập...' : 'Đăng nhập'}
              onPress={handleLogin}
              loading={loading}
              disabled={loading}
              style={styles.loginButton}
            />
          </View>

          {/* Footer */}
          <View style={styles.footer}>
            <Text style={styles.footerText}>
              Bạn quên mật khẩu? {' '}
              <Text style={styles.footerLink}>Khôi phục tại đây</Text>
            </Text>
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  keyboardAvoidingView: {
    flex: 1,
  },
  scrollContainer: {
    flexGrow: 1,
    padding: 24,
    justifyContent: 'center',
  },
  header: {
    alignItems: 'center',
    marginBottom: 40,
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#111827',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
    color: '#6B7280',
    textAlign: 'center',
  },
  form: {
    width: '100%',
    marginBottom: 32,
  },
  roleContainer: {
    marginBottom: 16,
  },
  roleLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#374151',
    marginBottom: 6,
  },
  pickerContainer: {
    borderWidth: 1,
    borderColor: '#D1D5DB',
    borderRadius: 8,
    backgroundColor: '#FFFFFF',
  },
  picker: {
    height: 48,
  },
  loginButton: {
    marginTop: 8,
  },
  footer: {
    alignItems: 'center',
  },
  footerText: {
    fontSize: 14,
    color: '#6B7280',
    textAlign: 'center',
  },
  footerLink: {
    color: '#3B82F6',
    fontWeight: '500',
  },
});

export default LoginScreen;