import React, { useState } from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
} from "react-native";
import { Mail, Lock, Eye, EyeOff } from "lucide-react-native";

const InputField = ({
  label,
  value,
  onChangeText,
  placeholder,
  secureTextEntry,
  showPasswordToggle,
  error,
  keyboardType = "default",
  themeColor = "#0032AF",
  textColor = "#1F2937",
  placeholderColor = "#9AA5C4",
}) => {
  const [showPassword, setShowPassword] = useState(false);

  const renderIcon = () => {
    if (label.toLowerCase().includes("mã")) {
      return <Mail color={themeColor} size={20} strokeWidth={2} />;
    } else if (label.toLowerCase().includes("mật")) {
      return <Lock color={themeColor} size={20} strokeWidth={2} />;
    }
    return null;
  };

  return (
    <View style={styles.container}>
      <Text style={[styles.label, { color: textColor }]}>{label}</Text>

      <View
        style={[
          styles.inputWrapper,
          { borderBottomColor: themeColor },
          error && { borderBottomColor: "#EF4444" },
        ]}
      >
        {renderIcon()}
        <TextInput
          style={[styles.input, { color: textColor }]}
          value={value}
          onChangeText={onChangeText}
          placeholder={placeholder}
          placeholderTextColor={placeholderColor}
          secureTextEntry={secureTextEntry && !showPassword}
          keyboardType={keyboardType}
          autoCapitalize="none"
        />
        {showPasswordToggle && (
          <TouchableOpacity onPress={() => setShowPassword(!showPassword)}>
            {showPassword ? (
              <EyeOff color={themeColor} size={20} strokeWidth={2} />
            ) : (
              <Eye color={themeColor} size={20} strokeWidth={2} />
            )}
          </TouchableOpacity>
        )}
      </View>

      {error ? <Text style={styles.errorText}>{error}</Text> : null}
    </View>
  );
};

const styles = StyleSheet.create({
  container: { marginBottom: 20 },
  label: {
    fontSize: 14,
    fontFamily: "Inter",
    fontWeight: "500",
    marginBottom: 6,
  },
  inputWrapper: {
    flexDirection: "row",
    alignItems: "center",
    borderBottomWidth: 1.4,
    paddingBottom: 8,
    paddingHorizontal: 4,
  },
  input: {
    flex: 1,
    fontSize: 14,
    fontFamily: "Inter",
    fontWeight: "500",
    marginLeft: 8,
  },
  errorText: {
    marginTop: 5,
    fontSize: 12,
    color: "#EF4444",
  },
});

export default InputField;
