
import React from 'react';
import { 
  TouchableOpacity, 
  Text, 
  ActivityIndicator, 
  StyleSheet 
} from 'react-native';

/**
 * LoginButton Component - Nút đăng nhập tùy chỉnh
 * @param {Object} props
 * @param {string} props.title - Text hiển thị trên nút
 * @param {function} props.onPress - Callback khi nhấn nút
 * @param {boolean} props.loading - Trạng thái loading
 * @param {boolean} props.disabled - Trạng thái disable
 * @param {'primary'|'secondary'|'outline'} props.variant - Kiểu nút
 * @param {Object} props.style - Custom style
 */
const LoginButton = ({
  title = 'Đăng nhập',
  onPress,
  loading = false,
  disabled = false,
  variant = 'primary',
  style,
  ...props
}) => {
  const isDisabled = disabled || loading;

  const getButtonStyle = () => {
    let baseStyle = [styles.button];
    
    switch (variant) {
      case 'primary':
        baseStyle.push(styles.primaryButton);
        if (isDisabled) baseStyle.push(styles.disabledButton);
        break;
      case 'secondary':
        baseStyle.push(styles.secondaryButton);
        if (isDisabled) baseStyle.push(styles.disabledSecondaryButton);
        break;
      case 'outline':
        baseStyle.push(styles.outlineButton);
        if (isDisabled) baseStyle.push(styles.disabledOutlineButton);
        break;
    }
    
    return baseStyle;
  };

  const getTextStyle = () => {
    let baseStyle = [styles.buttonText];
    
    switch (variant) {
      case 'primary':
        baseStyle.push(styles.primaryButtonText);
        if (isDisabled) baseStyle.push(styles.disabledButtonText);
        break;
      case 'secondary':
        baseStyle.push(styles.secondaryButtonText);
        if (isDisabled) baseStyle.push(styles.disabledSecondaryButtonText);
        break;
      case 'outline':
        baseStyle.push(styles.outlineButtonText);
        if (isDisabled) baseStyle.push(styles.disabledOutlineButtonText);
        break;
    }
    
    return baseStyle;
  };

  return (
    <TouchableOpacity
      style={[...getButtonStyle(), style]}
      onPress={onPress}
      disabled={isDisabled}
      activeOpacity={0.8}
      {...props}
    >
      {loading ? (
        <ActivityIndicator 
          size="small" 
          color={variant === 'primary' ? '#FFFFFF' : '#3B82F6'} 
        />
      ) : (
        <Text style={getTextStyle()}>{title}</Text>
      )}
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  button: {

    borderRadius: 8,
    paddingVertical: 14,
    paddingHorizontal: 24,
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: 50,
  },
  
  // Primary Button (Blue)
  primaryButton: {
    backgroundColor: '#3B82F6',
    shadowColor: '#3B82F6',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 3,
  },
  disabledButton: {
    backgroundColor: '#9CA3AF',
    shadowOpacity: 0,
    elevation: 0,
  },
  
  // Secondary Button (Gray)
  secondaryButton: {
    backgroundColor: '#6B7280',
  },
  disabledSecondaryButton: {
    backgroundColor: '#D1D5DB',
  },
  
  // Outline Button
  outlineButton: {
    backgroundColor: 'transparent',
    borderWidth: 1,
    borderColor: '#3B82F6',
  },
  disabledOutlineButton: {
    borderColor: '#D1D5DB',
  },
  
  // Text Styles
  buttonText: {
    fontSize: 16,
    fontWeight: '600',
    textAlign: 'center',
  },
  
  primaryButtonText: {
    color: '#FFFFFF',
  },
  disabledButtonText: {
    color: '#D1D5DB',
  },
  
  secondaryButtonText: {
    color: '#FFFFFF',
  },
  disabledSecondaryButtonText: {
    color: '#9CA3AF',
  },
  
  outlineButtonText: {
    color: '#3B82F6',
  },
  disabledOutlineButtonText: {
    color: '#D1D5DB',
  },
});

export default LoginButton;

