import React from "react";
import { View, Text, TouchableOpacity, Switch } from "react-native";
import { Feather } from "@expo/vector-icons";
import { settingsStyles as styles } from "../styles/settingsStyles";

/**
 * Component render một dòng cài đặt (Toggle, Navigate, hoặc Action)
 */
export default function SettingRow({
  item,
  index,
  sectionItems,
  theme,
  isActive,
  onToggle,
  onAction,
}) {
  const isLast = index === sectionItems.length - 1;
  const showDivider = !isLast && item.type !== "description";

  const isLogout = item.key === "logout";

  const renderAction = () => {
    switch (item.type) {
      case "toggle":
        return (
          <Switch
            trackColor={{ false: theme.border, true: theme.primary }}
            thumbColor={theme.card}
            onValueChange={onToggle}
            value={isActive}
          />
        );
      case "navigate":
      case "action":
        return (
          <Feather name="chevron-right" size={20} color={theme.textSecondary} />
        );
      default:
        return null;
    }
  };

  return (
    <View>
      <TouchableOpacity
        style={styles.settingItem}
        onPress={() => onAction(item.key)}
        activeOpacity={item.type !== "toggle" ? 0.7 : 1}
        disabled={item.type === "toggle" && item.key !== "theme"} // Chỉ theme mới có thể nhấn nguyên dòng
      >
        <View style={styles.settingInfo}>
          {item.icon && (
            <Feather
              name={item.icon}
              size={20}
              color={isLogout ? "#EF4444" : theme.textPrimary}
            />
          )}
          <Text
            style={[
              styles.settingTitle,
              { color: isLogout ? "#EF4444" : theme.textPrimary },
              !item.icon && { marginLeft: 0 },
            ]}
          >
            {item.title}
          </Text>
        </View>

        <View style={styles.settingAction}>{renderAction()}</View>
      </TouchableOpacity>

      {showDivider && (
        <View style={[styles.divider, { backgroundColor: theme.border }]} />
      )}
    </View>
  );
}
