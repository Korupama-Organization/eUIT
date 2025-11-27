import { useState } from "react";
import { Alert } from "react-native";
import { useNavigation } from "@react-navigation/native";
import { useTheme } from "../../../App";
import { logout } from "../../auth/api/authAPI";

/**
 * Custom hook xử lý tất cả logic nghiệp vụ và state của SettingsScreen
 * @param {function} setIsLoggedIn - Hàm để cập nhật trạng thái đăng nhập
 */
export const useSettingsLogic = (setIsLoggedIn) => {
  const navigation = useNavigation();
  const { theme, toggleTheme, isDarkMode } = useTheme();

  // State quản lý theme (đồng bộ với Context theme)
  const [darkMode, setDarkMode] = useState(isDarkMode);

  // State quản lý các toggle cài đặt khác
  const [toggles, setToggles] = useState({
    allNotifications: true,
    courseProgress: true,
    teachingBreak: true,
    makeUpClass: false,
  });

  // --- Handlers ---

  const handleThemeToggle = () => {
    const newDarkMode = !darkMode;
    setDarkMode(newDarkMode);
    toggleTheme();
  };

  const handleToggle = (key) => {
    setToggles((prev) => ({ ...prev, [key]: !prev[key] }));
  };

  const handleLogout = async () => {
    Alert.alert("Đăng xuất", "Bạn có chắc chắn muốn đăng xuất?", [
      {
        text: "Hủy",
        style: "cancel",
      },
      {
        text: "Đăng xuất",
        onPress: async () => {
          try {
            await logout();
            setIsLoggedIn(false);
          } catch (e) {
            console.error("Logout API failed, logging out locally:", e);
            setIsLoggedIn(false);
          }
        },
        style: "destructive",
      },
    ]);
  };

  const handleAction = (key) => {
    if (key === "theme") {
      handleThemeToggle();
      return;
    }

    switch (key) {
      case "logout":
        handleLogout();
        break;
      case "language":
        // navigation.navigate('LanguageSettings');
        console.log("Navigate to Language Settings");
        break;
      case "profile":
        // navigation.navigate('ProfileSettings');
        console.log("Navigate to Profile Settings");
        break;
      case "security":
        // navigation.navigate('SecuritySettings');
        console.log("Navigate to Security Settings");
        break;
      default:
        console.log("Hành động không xác định:", key);
    }
  };

  return {
    theme,
    isDarkMode: darkMode,
    toggles,
    handleToggle,
    handleAction,
  };
};
