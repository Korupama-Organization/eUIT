import React, { useState } from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  Switch,
  TouchableOpacity,
  Alert,
} from "react-native";
import { useNavigation } from "@react-navigation/native";
import { useTheme } from "../../../App";

const SettingsScreen = ({ setIsLoggedIn }) => {
  const navigation = useNavigation();
  const { theme, toggleTheme, isDarkMode } = useTheme(); // Sử dụng theme

  // State cho các switch
  const [darkMode, setDarkMode] = useState(false);
  const [notifications, setNotifications] = useState({
    courseProgress: true,
    teachingBreak: true,
    makeUpClass: false,
    schedule: true,
    newNotifications: true,
    administrativeUpdates: false,
  });
  const [emailNotifications, setEmailNotifications] = useState({
    allEmails: false,
    account: true,
  });

  // Hàm xử lý toggle theme
  const handleThemeToggle = () => {
    const newDarkMode = !darkMode;
    setDarkMode(!darkMode);
    toggleTheme(); // Gọi toggleTheme từ context
  };

  // Hàm xử lý đăng xuất
  const handleLogout = () => {
    Alert.alert("Đăng xuất", "Bạn có chắc chắn muốn đăng xuất?", [
      {
        text: "Hủy",
        style: "cancel",
      },
      {
        text: "Đăng xuất",
        style: "destructive",
        onPress: () => {
          setIsLoggedIn(false);
        },
      },
    ]);
  };

  // Hàm toggle notification
  const toggleNotification = (key) => {
    setNotifications((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  // Hàm toggle email notification
  const toggleEmailNotification = (key) => {
    setEmailNotifications((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  return (
    <ScrollView
      style={[styles.container, { backgroundColor: theme.background }]}
      showsVerticalScrollIndicator={false}
    >
      {/* Header */}
      <View style={styles.header}>
        <Text style={[styles.headerTitle, { color: theme.textPrimary }]}>
          Cài đặt
        </Text>
        <Text style={[styles.headerSubtitle, { color: theme.textSecondary }]}>
          Tùy chọn để phù hợp với trải nghiệm người dùng
        </Text>
      </View>

      {/* Màn hình Section */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
          Màn hình
        </Text>
        <Text
          style={[styles.sectionDescription, { color: theme.textSecondary }]}
        >
          Chỉnh chính giao diện để giảm độ chói
        </Text>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />

        {/* Chế độ tối */}
        <View style={styles.settingItem}>
          <View style={styles.settingInfo}>
            <Text style={[styles.settingTitle, { color: theme.textPrimary }]}>
              Chế độ sáng
            </Text>
          </View>
          <Switch
            value={darkMode}
            onValueChange={handleThemeToggle}
            trackColor={{ false: "#E5E7EB", true: "#2F6BFF" }}
            thumbColor={darkMode ? "#FFFFFF" : "#FFFFFF"}
          />
        </View>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />
      </View>

      {/* Thông báo đẩy Section */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
          Thông báo đẩy
        </Text>
        <Text
          style={[styles.sectionDescription, { color: theme.textSecondary }]}
        >
          Tùy chọn những thông báo nào sẽ được đẩy lên
        </Text>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />

        {/* Danh sách thông báo */}
        {[
          { key: "courseProgress", label: "Cập nhật tiến độ khóa học" },
          { key: "teachingBreak", label: "Thông báo nghỉ dạy" },
          { key: "makeUpClass", label: "Thông báo học bù" },
          { key: "schedule", label: "Lịch học" },
          { key: "newNotifications", label: "Thông báo mới" },
          {
            key: "administrativeUpdates",
            label: "Cập nhật trong thái thú tục hành chính",
          },
        ].map((item, index) => (
          <View key={item.key}>
            <View style={styles.notificationItem}>
              <View style={styles.notificationInfo}>
                <Text
                  style={[
                    styles.notificationTitle,
                    { color: theme.textPrimary },
                  ]}
                >
                  {item.label}
                </Text>
              </View>
              <Switch
                value={notifications[item.key]}
                onValueChange={() => toggleNotification(item.key)}
                trackColor={{ false: "#E5E7EB", true: "#2F6BFF" }}
                thumbColor={notifications[item.key] ? "#FFFFFF" : "#FFFFFF"}
              />
            </View>
            {index < 5 && (
              <View
                style={[styles.divider, { backgroundColor: theme.border }]}
              />
            )}
          </View>
        ))}
      </View>

      {/* Thông báo email Section */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
          Thông báo email
        </Text>
        <Text
          style={[styles.sectionDescription, { color: theme.textSecondary }]}
        >
          Tùy chọn các thông báo email
        </Text>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />

        {/* Tất cả thông báo email */}
        <View style={styles.notificationItem}>
          <View style={styles.notificationInfo}>
            <Text
              style={[styles.notificationTitle, { color: theme.textPrimary }]}
            >
              Tất cả thông báo email
            </Text>
          </View>
          <Switch
            value={emailNotifications.allEmails}
            onValueChange={() => toggleEmailNotification("allEmails")}
            trackColor={{ false: "#E5E7EB", true: "#2F6BFF" }}
            thumbColor={emailNotifications.allEmails ? "#FFFFFF" : "#FFFFFF"}
          />
        </View>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />

        {/* Tài khoản */}
        <View style={styles.notificationItem}>
          <View style={styles.notificationInfo}>
            <Text
              style={[styles.notificationTitle, { color: theme.textPrimary }]}
            >
              Tài khoản
            </Text>
            <Text
              style={[
                styles.notificationDescription,
                { color: theme.textSecondary },
              ]}
            >
              Quản lý các chỉ đặt về tài khoản
            </Text>
          </View>
          <Switch
            value={emailNotifications.account}
            onValueChange={() => toggleEmailNotification("account")}
            trackColor={{ false: "#E5E7EB", true: "#2F6BFF" }}
            thumbColor={emailNotifications.account ? "#FFFFFF" : "#FFFFFF"}
          />
        </View>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />
      </View>

      {/* Nút Đăng xuất */}
      <View style={styles.logoutSection}>
        <TouchableOpacity
          style={[styles.logoutButton, { backgroundColor: "#EF4444" }]}
          onPress={handleLogout}
        >
          <Text style={styles.logoutButtonText}>Đăng xuất</Text>
        </TouchableOpacity>
      </View>

      {/* Khoảng trống cuối */}
      <View style={styles.bottomSpace} />
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 10,
  },
  headerTitle: {
    fontSize: 28,
    fontWeight: "bold",
    marginBottom: 4,
  },
  headerSubtitle: {
    fontSize: 16,
    lineHeight: 20,
  },
  section: {
    marginTop: 8,
    paddingVertical: 8,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: "600",
    paddingHorizontal: 20,
    marginBottom: 4,
  },
  sectionDescription: {
    fontSize: 14,
    paddingHorizontal: 20,
    marginBottom: 12,
    lineHeight: 18,
  },
  divider: {
    height: 1,
    marginHorizontal: 20,
  },
  settingItem: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  settingInfo: {
    flex: 1,
  },
  settingTitle: {
    fontSize: 16,
    fontWeight: "500",
  },
  notificationItem: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "flex-start",
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  notificationInfo: {
    flex: 1,
    marginRight: 12,
  },
  notificationTitle: {
    fontSize: 16,
    fontWeight: "500",
    marginBottom: 2,
  },
  notificationDescription: {
    fontSize: 14,
    lineHeight: 18,
  },
  logoutSection: {
    paddingHorizontal: 20,
    paddingTop: 24,
    paddingBottom: 40,
  },
  logoutButton: {
    borderRadius: 10,
    paddingVertical: 16,
    alignItems: "center",
    shadowColor: "#EF4444",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  logoutButtonText: {
    color: "#FFFFFF",
    fontSize: 18,
    fontWeight: "600",
  },
  bottomSpace: {
    height: 20,
  },
});

export default SettingsScreen;
