import React, { useState } from 'react';
import { 
  View, 
  TextInput, 
  Text, 
  TouchableOpacity, 
  StyleSheet 
} from 'react-native';

/**
 * InputField Component - Input field đơn giản cho form đăng nhập
 */
const InputField = ({
  label,
  value,
  onChangeText,
  placeholder,
  secureTextEntry = false,
  keyboardType = 'default',
  showPasswordToggle = false,
  error,
  style,
  ...props
}) => {
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  const togglePasswordVisibility = () => {
    setIsPasswordVisible(!isPasswordVisible);
  };

  return (
    <View style={[styles.container, style]}>
      {/* Label */}
      {label && (
        <Text style={styles.label}>{label}</Text>
      )}
      
      {/* Input Container */}
      <View style={styles.inputContainer}>
        <TextInput
          style={styles.textInput}
          value={value}
          onChangeText={onChangeText}
          placeholder={placeholder}
          placeholderTextColor="#999"
          secureTextEntry={secureTextEntry && !isPasswordVisible}
          keyboardType={keyboardType}
          autoCapitalize="none"
          autoCorrect={false}
          returnKeyType="done"
          blurOnSubmit={true}
          {...props}
        />
        
        {/* Password Toggle */}
        {showPasswordToggle && (
          <TouchableOpacity
            onPress={togglePasswordVisibility}
            style={styles.eyeButton}
          >
            <Text style={styles.eyeIcon}>
              {isPasswordVisible ? '�️' : '👁️‍🗨️'}
            </Text>
          </TouchableOpacity>
        )}
      </View>
      
      {/* Error Message */}
      {error && (
        <Text style={styles.errorText}>{error}</Text>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    marginBottom: 15,
  },
  label: {
    fontSize: 16,
    fontWeight: '500',
    color: '#333',
    marginBottom: 8,
  },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    backgroundColor: '#fff',
    paddingHorizontal: 15,
    height: 50,
  },
  textInput: {
    flex: 1,
    fontSize: 16,
    color: '#333',
    height: '100%',
  },
  eyeButton: {
    padding: 5,
    marginLeft: 10,
  },
  eyeIcon: {
    fontSize: 20,
  },
  errorText: {
    fontSize: 14,
    color: '#ff4444',
    marginTop: 5,
  },
});

export default InputField;