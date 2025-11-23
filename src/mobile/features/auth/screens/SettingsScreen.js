import React, { useState } from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  Switch,
  TouchableOpacity,
  Alert,
  ActivityIndicator,
} from "react-native";
import { useNavigation } from "@react-navigation/native";
import { useTheme } from "../../../theme/ThemeProvider";
import { logout } from "../api/authAPI";
import AsyncStorage from "@react-native-async-storage/async-storage";
import {
  Moon,
  Bell,
  BookOpen,
  Coffee,
  RefreshCw,
  Calendar,
  ClipboardList,
  Mail,
  LogOut,
  ChevronRight,
} from "lucide-react-native";

const SettingsScreen = ({ setIsLoggedIn }) => {
  const navigation = useNavigation();
  const { theme, toggleTheme, isDarkMode } = useTheme();
  const [loggingOut, setLoggingOut] = useState(false);

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
  });

  const handleThemeToggle = () => {
    toggleTheme();
  };

  const handleLogout = () => {
    Alert.alert(
      "Đăng xuất",
      "Bạn có chắc chắn muốn đăng xuất?",
      [
        {
          text: "Hủy",
          style: "cancel",
        },
        {
          text: "Đăng xuất",
          style: "destructive",
          onPress: async () => {
            setLoggingOut(true);
            try {
              console.log("🔵 [LOGOUT] Đang đăng xuất...");
              await logout();
              await AsyncStorage.clear();
              setIsLoggedIn(false);
              console.log("✅ [LOGOUT] Đăng xuất thành công");
            } catch (error) {
              console.error("❌ [LOGOUT] Error:", error);
              await AsyncStorage.clear();
              setIsLoggedIn(false);
            } finally {
              setLoggingOut(false);
            }
          },
        },
      ],
      { cancelable: true }
    );
  };

  const toggleNotification = (key) => {
    setNotifications((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

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
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Màn hình
          </Text>
          <Text
            style={[styles.sectionDescription, { color: theme.textSecondary }]}
          >
            Chỉnh chính giao diện để giảm độ chói
          </Text>
        </View>

        <View style={styles.settingItem}>
          <View style={styles.settingLeft}>
            <View
              style={[styles.iconContainer, { backgroundColor: theme.iconBg }]}
            >
              <Moon size={20} color={theme.primary} />
            </View>
            <Text style={[styles.settingTitle, { color: theme.textPrimary }]}>
              Chế độ sáng
            </Text>
          </View>
          <Switch
            value={isDarkMode}
            onValueChange={handleThemeToggle}
            trackColor={{ false: "#D1D5DB", true: "#2F6BFF" }}
            thumbColor="#FFFFFF"
            ios_backgroundColor="#D1D5DB"
          />
        </View>
      </View>

      {/* Thông báo đẩy Section */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Thông báo đẩy
          </Text>
          <Text
            style={[styles.sectionDescription, { color: theme.textSecondary }]}
          >
            Tùy chọn những thông báo nào sẽ được đẩy lên
          </Text>
        </View>

        {[
          {
            key: "courseProgress",
            label: "Cập nhật tiến độ khóa học",
            icon: BookOpen,
          },
          { key: "teachingBreak", label: "Thông báo nghỉ dạy", icon: Coffee },
          { key: "makeUpClass", label: "Thông báo học bù", icon: RefreshCw },
          { key: "schedule", label: "Lịch học", icon: Calendar },
          { key: "newNotifications", label: "Thông báo mới", icon: Bell },
          {
            key: "administrativeUpdates",
            label: "Cập nhật trạng thái thủ tục hành chính",
            icon: ClipboardList,
          },
        ].map((item) => {
          const IconComponent = item.icon;
          return (
            <View key={item.key} style={styles.settingItem}>
              <View style={styles.settingLeft}>
                <View
                  style={[
                    styles.iconContainer,
                    { backgroundColor: theme.iconBg },
                  ]}
                >
                  <IconComponent size={20} color={theme.primary} />
                </View>
                <Text
                  style={[styles.settingTitle, { color: theme.textPrimary }]}
                >
                  {item.label}
                </Text>
              </View>
              <Switch
                value={notifications[item.key]}
                onValueChange={() => toggleNotification(item.key)}
                trackColor={{ false: "#D1D5DB", true: "#2F6BFF" }}
                thumbColor="#FFFFFF"
                ios_backgroundColor="#D1D5DB"
              />
            </View>
          );
        })}
      </View>

      {/* Thông báo email Section */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Thông báo email
          </Text>
          <Text
            style={[styles.sectionDescription, { color: theme.textSecondary }]}
          >
            Tùy chọn các thông báo email
          </Text>
        </View>

        <View style={styles.settingItem}>
          <View style={styles.settingLeft}>
            <View
              style={[styles.iconContainer, { backgroundColor: theme.iconBg }]}
            >
              <Mail size={20} color={theme.primary} />
            </View>
            <Text style={[styles.settingTitle, { color: theme.textPrimary }]}>
              Bật thông báo email
            </Text>
          </View>
          <Switch
            value={emailNotifications.allEmails}
            onValueChange={() => toggleEmailNotification("allEmails")}
            trackColor={{ false: "#D1D5DB", true: "#2F6BFF" }}
            thumbColor="#FFFFFF"
            ios_backgroundColor="#D1D5DB"
          />
        </View>
      </View>

      {/* Tài khoản Section - ĐÃ CHỈNH ĐẸP HƠN */}
      <View style={[styles.section, { backgroundColor: theme.card }]}>
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Tài khoản
          </Text>
          <Text
            style={[styles.sectionDescription, { color: theme.textSecondary }]}
          >
            Quản lý các chỉ đặt về tài khoản
          </Text>
        </View>

        <TouchableOpacity
          style={styles.logoutItem}
          onPress={handleLogout}
          activeOpacity={0.7}
          disabled={loggingOut}
        >
          <View style={styles.settingLeft}>
            <View
              style={[styles.iconContainer, { backgroundColor: "#FEE2E2" }]}
            >
              {loggingOut ? (
                <ActivityIndicator size="small" color="#EF4444" />
              ) : (
                <LogOut size={20} color="#EF4444" />
              )}
            </View>
            <Text style={[styles.logoutText]}>
              {loggingOut ? "Đang đăng xuất..." : "Log Out"}
            </Text>
          </View>
          <ChevronRight size={20} color={theme.textSecondary} />
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
    paddingTop: 16,
    paddingBottom: 12,
  },
  headerTitle: {
    fontSize: 26,
    fontWeight: "700",
    marginBottom: 4,
  },
  headerSubtitle: {
    fontSize: 14,
    lineHeight: 20,
  },
  section: {
    marginTop: 12,
    marginHorizontal: 16,
    borderRadius: 12,
    paddingVertical: 12,
    paddingHorizontal: 0,
  },
  sectionHeader: {
    paddingHorizontal: 16,
    paddingBottom: 8,
  },
  sectionTitle: {
    fontSize: 17,
    fontWeight: "600",
    marginBottom: 2,
  },
  sectionDescription: {
    fontSize: 13,
    lineHeight: 18,
    opacity: 0.8,
  },
  settingItem: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingVertical: 10,
    paddingHorizontal: 16,
  },
  settingLeft: {
    flexDirection: "row",
    alignItems: "center",
    flex: 1,
  },
  iconContainer: {
    width: 36,
    height: 36,
    borderRadius: 8,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  settingTitle: {
    fontSize: 15,
    fontWeight: "500",
    flex: 1,
  },
  logoutItem: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingVertical: 10,
    paddingHorizontal: 16,
  },
  logoutText: {
    fontSize: 15,
    fontWeight: "600",
    flex: 1,
    color: "#EF4444",
  },
  bottomSpace: {
    height: 100,
  },
});

export default SettingsScreen;
