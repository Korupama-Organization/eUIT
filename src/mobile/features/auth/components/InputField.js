import React from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
} from "react-native";

const InputField = ({
  label,
  value,
  onChangeText,
  placeholder,
  secureTextEntry,
  showPassword,
  onTogglePassword,
  icon,
}) => {
  return (
    <View style={styles.inputContainer}>
      <Text style={styles.label}>{label}</Text>
      <View style={styles.inputWrapper}>
        <Text style={styles.icon}>{icon}</Text>
        <TextInput
          style={styles.input}
          value={value}
          onChangeText={onChangeText}
          placeholder={placeholder}
          secureTextEntry={secureTextEntry && !showPassword}
          autoCapitalize="none"
          keyboardType={icon === "✉" ? "email-address" : "default"}
        />
        {secureTextEntry && (
          <TouchableOpacity onPress={onTogglePassword} style={styles.eyeIcon}>
            <Text>{showPassword ? "👁" : "👁‍🗨"}</Text>
          </TouchableOpacity>
        )}
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  inputContainer: {
    marginBottom: 20,
  },
  label: {
    fontSize: 14,
    color: "#666",
    marginBottom: 8,
  },
  inputWrapper: {
    flexDirection: "row",
    alignItems: "center",
    borderBottomWidth: 1,
    borderBottomColor: "#0066FF",
    paddingBottom: 8,
  },
  icon: {
    fontSize: 18,
    marginRight: 8,
    color: "#666",
  },
  input: {
    flex: 1,
    fontSize: 16,
    color: "#333",
    padding: 0,
  },
  eyeIcon: {
    padding: 4,
  },
});

export default InputField;
