import React from "react";
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  ActivityIndicator,
} from "react-native";
import { Ionicons } from "@expo/vector-icons";

// Imports từ các file đã tách
import { useUserProfile } from "../hooks/useUserProfile";
import QuickItem from "../components/QuickItem";
import { homeStyles as styles } from "../styles/homeStyles";

import { useTheme } from "../../../App.js";

export default function HomeScreen() {
  const { theme } = useTheme(); // Lấy theme

  // 💥 CHỈ CẦN GỌI HOOK ĐỂ LẤY LOGIC VÀ DỮ LIỆU
  const { loading, username, initials } = useUserProfile();

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

          {/* Card Lịch trình mẫu */}
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

// **Lưu ý:** Component TabItem và các Styles không dùng nữa đã được loại bỏ khỏi file này.
