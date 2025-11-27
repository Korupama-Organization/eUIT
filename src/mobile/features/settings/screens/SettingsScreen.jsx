import React from "react";
import { View, Text, ScrollView } from "react-native";

// Imports từ các file đã tách
import { useSettingsLogic } from "../hooks/useSettingsLogic";
import SettingRow from "../components/SettingRow";
import { settingsMenu } from "../constants/settingsData";
import { settingsStyles as styles } from "../styles/settingsStyles";

const SettingsScreen = ({ setIsLoggedIn }) => {
  // 💥 CHỈ CẦN GỌI HOOK ĐỂ LẤY LOGIC VÀ STATE
  const { theme, isDarkMode, toggles, handleToggle, handleAction } =
    useSettingsLogic(setIsLoggedIn);

  const renderSection = (section) => (
    <View key={section.id} style={styles.section}>
      <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
        {section.title}
      </Text>
      <Text style={[styles.sectionDescription, { color: theme.textSecondary }]}>
        {section.description}
      </Text>
      <View style={{ backgroundColor: theme.card }}>
        {section.items.map((item, index) => {
          const isDarkModeSetting = item.key === "theme";
          const isActive = isDarkModeSetting ? isDarkMode : toggles[item.key];

          return (
            <SettingRow
              key={item.key}
              item={item}
              index={index}
              sectionItems={section.items}
              theme={theme}
              isActive={isActive}
              onToggle={() =>
                isDarkModeSetting
                  ? handleAction("theme") // Dùng handleAction cho theme
                  : handleToggle(item.key)
              }
              onAction={handleAction}
            />
          );
        })}
      </View>
    </View>
  );

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      {/* Header */}
      <View style={styles.header}>
        <Text style={[styles.headerTitle, { color: theme.textPrimary }]}>
          Cài đặt
        </Text>
      </View>

      <ScrollView
        style={styles.container}
        contentContainerStyle={styles.scrollViewContent}
        showsVerticalScrollIndicator={false}
      >
        {settingsMenu.map(renderSection)}
      </ScrollView>
    </View>
  );
};

export default SettingsScreen;
