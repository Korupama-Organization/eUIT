import React, { useState, useEffect } from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  ActivityIndicator,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { getProfile } from "../api/authAPI.js";
import { useTheme } from "../../../App";

export default function HomeScreen() {
  const { theme } = useTheme(); // Sử dụng theme
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const userData = await getProfile();
        setProfile(userData);
      } catch (error) {
        console.error("❌ Lỗi khi lấy thông tin người dùng:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  if (loading) {
    return (
      <View
        style={[
          styles.container,
          {
            justifyContent: "center",
            alignItems: "center",
            backgroundColor: theme.background,
          },
        ]}
      >
        <ActivityIndicator size="large" color={theme.primary} />
        <Text style={{ color: theme.textSecondary, marginTop: 10 }}>
          Đang tải thông tin...
        </Text>
      </View>
    );
  }

  const username = profile?.name || "Người dùng";
  const initials = username
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      <ScrollView contentContainerStyle={styles.scroll}>
        {/* Header */}
        <View style={styles.header}>
          <View>
            <Text style={[styles.welcome, { color: theme.textSecondary }]}>
              Chào mừng trở lại,
            </Text>
            <Text style={[styles.username, { color: theme.textPrimary }]}>
              {username}
            </Text>
          </View>
          <View style={styles.headerIcons}>
            <Ionicons
              name="notifications-outline"
              size={50}
              color={theme.textSecondary}
            />
            <View
              style={[styles.avatarCircle, { backgroundColor: theme.card }]}
            >
              <Text style={[styles.avatarText, { color: theme.primary }]}>
                {initials}
              </Text>
            </View>
          </View>
        </View>

        {/* Lịch trình */}
        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Lịch trình tiếp theo
          </Text>

          <View style={[styles.scheduleCard, { backgroundColor: theme.card }]}>
            <View style={styles.scheduleTime}>
              <Text style={[styles.timeText, { color: theme.primary }]}>
                10:00 AM - 11:30 AM
              </Text>
              <Text style={[styles.courseCode, { color: theme.textPrimary }]}>
                IE307.Q12
              </Text>
              <Text style={[styles.courseName, { color: theme.textSecondary }]}>
                Công nghệ lập trình...
              </Text>
              <Text style={[styles.room, { color: theme.textSecondary }]}>
                B1.22
              </Text>
            </View>
            <View style={styles.countdown}>
              <Text
                style={[styles.countdownText, { color: theme.textSecondary }]}
              >
                Bắt đầu trong
              </Text>
              <Text style={[styles.countdownTime, { color: theme.primary }]}>
                2h 15m
              </Text>
            </View>
          </View>

          <TouchableOpacity style={styles.viewScheduleBtn}>
            <Text style={[styles.viewScheduleText, { color: theme.primary }]}>
              Xem toàn bộ lịch trình
            </Text>
            <Ionicons name="chevron-forward" size={16} color={theme.primary} />
          </TouchableOpacity>
        </View>

        {/* Thông báo */}
        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Thông báo mới
          </Text>
          <TouchableOpacity
            style={[styles.noticeCard, { backgroundColor: theme.card }]}
          >
            <Ionicons
              name="document-text-outline"
              size={20}
              color={theme.primary}
            />
            <View style={{ flex: 1, marginLeft: 10 }}>
              <Text style={[styles.noticeTitle, { color: theme.textPrimary }]}>
                New Quantum Computing Lab Opens on Campus
              </Text>
              <Text style={[styles.noticeDate, { color: theme.textSecondary }]}>
                July 29, 2024
              </Text>
            </View>
            <Ionicons
              name="chevron-forward"
              size={16}
              color={theme.textSecondary}
            />
          </TouchableOpacity>
        </View>

        {/* Truy cập nhanh */}
        <View style={styles.section}>
          <View style={styles.quickAccessHeader}>
            <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
              Truy cập nhanh
            </Text>
            <Ionicons
              name="settings-outline"
              size={18}
              color={theme.textSecondary}
            />
          </View>

          <View style={styles.quickGrid}>
            <QuickItem
              icon="calendar-outline"
              label="Lịch giảng dạy"
              theme={theme}
            />
            <QuickItem
              icon="people-outline"
              label="Quản lý lớp"
              theme={theme}
            />
            <QuickItem
              icon="clipboard-outline"
              label="Giao bài tập"
              theme={theme}
            />
            <QuickItem
              icon="stats-chart-outline"
              label="Điểm chuyên cần"
              theme={theme}
            />
            <QuickItem icon="create-outline" label="Nhập điểm" theme={theme} />
            <QuickItem
              icon="megaphone-outline"
              label="Thông báo khoa/phòng"
              theme={theme}
            />
            <QuickItem
              icon="person-circle-outline"
              label="Hồ sơ giảng viên"
              theme={theme}
            />
            <QuickItem
              icon="bar-chart-outline"
              label="Thống kê giờ giảng"
              theme={theme}
            />
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

// Component truy cập nhanh
function QuickItem({ icon, label, theme }) {
  return (
    <TouchableOpacity style={styles.quickItem}>
      <Ionicons name={icon} size={26} color={theme.primary} />
      <Text style={[styles.quickText, { color: theme.textSecondary }]}>
        {label}
      </Text>
    </TouchableOpacity>
  );
}

// Component tab dưới (nếu cần)
function TabItem({ icon, label, active, theme }) {
  return (
    <TouchableOpacity style={styles.tabItem}>
      <Ionicons
        name={icon}
        size={24}
        color={active ? theme.primary : theme.textSecondary}
        style={
          active && [
            styles.activeIcon,
            { backgroundColor: `${theme.primary}20` },
          ]
        }
      />
      <Text
        style={[
          styles.tabLabel,
          { color: active ? theme.primary : theme.textSecondary },
        ]}
      >
        {label}
      </Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  scroll: {
    padding: 20,
    paddingBottom: 100,
  },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 25,
    marginTop: 20,
  },
  welcome: {
    fontSize: 20,
  },
  username: {
    fontSize: 23,
    fontWeight: "600",
  },
  headerIcons: {
    flexDirection: "row",
    alignItems: "center",
    gap: 10,
  },
  avatarCircle: {
    width: 50,
    height: 50,
    borderRadius: 25,
    justifyContent: "center",
    alignItems: "center",
  },
  avatarText: {
    fontSize: 25,
    fontWeight: "bold",
  },

  section: {
    marginBottom: 20,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: "600",
    marginBottom: 15,
  },

  scheduleCard: {
    borderRadius: 20,
    padding: 15,
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 10,
  },
  scheduleTime: {
    flex: 1,
  },
  timeText: {
    fontSize: 13,
  },
  courseCode: {
    fontSize: 17,
    fontWeight: "700",
    marginTop: 4,
  },
  courseName: {
    fontSize: 14,
  },
  room: {
    fontSize: 13,
    marginTop: 4,
  },
  countdown: {
    alignItems: "flex-end",
  },
  countdownText: {
    fontSize: 12,
  },
  countdownTime: {
    fontSize: 16,
    fontWeight: "bold",
  },

  viewScheduleBtn: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "flex-end",
  },
  viewScheduleText: {
    fontSize: 13,
    marginRight: 4,
  },

  noticeCard: {
    borderRadius: 14,
    padding: 12,
    flexDirection: "row",
    alignItems: "center",
  },
  noticeTitle: {
    fontSize: 14,
    fontWeight: "500",
  },
  noticeDate: {
    fontSize: 12,
  },

  quickAccessHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 15,
  },
  quickGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
    justifyContent: "space-between",
  },
  quickItem: {
    width: "22%",
    alignItems: "center",
    marginBottom: 20,
  },
  quickText: {
    fontSize: 11,
    marginTop: 6,
    textAlign: "center",
  },

  bottomTab: {
    flexDirection: "row",
    justifyContent: "space-around",
    alignItems: "center",
    paddingVertical: 10,
    borderTopWidth: 0.5,
    position: "absolute",
    bottom: 0,
    left: 0,
    right: 0,
  },
  tabItem: {
    alignItems: "center",
  },
  tabLabel: {
    fontSize: 11,
    marginTop: 3,
  },
  activeIcon: {
    borderRadius: 20,
    padding: 6,
  },
});
