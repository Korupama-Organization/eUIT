import React, { useEffect, useState } from "react";
import { View, Text, ScrollView, TouchableOpacity, StyleSheet } from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../App";
import QuickActions from "../components/QuickActions";
import { fetchHomeData } from "../features/auth/api/homeAPI";

export default function HomeScreen({ setIsLoggedIn }) {
  const { theme } = useTheme();
  const [loading, setLoading] = useState(true);
  const [card, setCard] = useState(null);
  const [nextClass, setNextClass] = useState(null);
  const [quickGpa, setQuickGpa] = useState(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        const data = await fetchHomeData();
        setCard(data.card);
        setNextClass(data.nextClass);
        setQuickGpa(data.quickGpa);
      } catch (err) {
        console.error("HomeScreen loadData error:", err);
      } finally {
        setLoading(false);
      }
  };
  loadData();
},[]);
  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      <ScrollView contentContainerStyle={styles.scroll}>
        {/* Header */}
        <View style={styles.header}>
          <View>
            <Text style={[styles.welcome, { color: theme.muted }]}>
              Chào mừng trở lại,
            </Text>
            <Text style={[styles.username, { color: theme.textSecondary }]}>
              {card?.HoTen || "Sinh viên"}
            </Text>
          </View>

          <View style={styles.avatarWrap}>
            <View style={[styles.avatarBorder, { borderColor: theme.accent }]}>
              <View style={[styles.avatarInner, { backgroundColor: theme.card }]}>
                <Text style={[styles.avatarText, { color: theme.textSecondary }]}>
                  AT
                </Text>
              </View>
            </View>
          </View>
        </View>

        {/* Lịch trình */}
        <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
          Lịch trình tiếp theo
        </Text>
        <View
          style={[
            styles.scheduleCard,
            { backgroundColor: theme.card, shadowColor: theme.accent },
          ]}
        >
          <View>
            <Text style={[styles.scheduleTime, { color: theme.textSecondary }]}>
              {nextClass?.ThoiGian || "--"}
            </Text>
            <Text style={[styles.scheduleTitle, { color: theme.textPrimary }]}>
              {nextClass?.TenMonHoc || "Chưa có lớp học"}
            </Text>
            <Text style={[styles.scheduleRoom, { color: theme.muted }]}>
              {nextClass?.PhongHoc || ""}
            </Text>
            <Text style={[styles.scheduleTeacher, { color: theme.muted }]}>
              {nextClass?.GiangVien || ""}
            </Text>
          </View>

          <View style={{ alignItems: "flex-end" }}>
            <Text style={[styles.scheduleSoon, { color: theme.muted }]}>
              Bắt đầu trong
            </Text>
            <Text style={[styles.scheduleCountdown, { color: theme.textSecondary }]}>
              2h 15m
            </Text>
          </View>
        </View>

        <TouchableOpacity style={[styles.viewAll, { borderColor: theme.border }]}>
          <Text style={[styles.viewAllText, { color: theme.accent }]}>
            Xem toàn bộ thời khóa biểu
          </Text>
          <Feather name="chevron-right" size={18} color={theme.accent} />
        </TouchableOpacity>

        {/* Thông báo + GPA */}
        <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
          Thông báo mới
        </Text>
        <View style={[styles.notificationCard, { backgroundColor: theme.card }]}>
          <Feather name="bell" color={theme.textSecondary} size={22} />
          <View style={{ flex: 1, marginLeft: 10 }}>
            <Text style={[styles.noticeTitle, { color: theme.textPrimary }]}>
              New Quantum Computing Lab Opens on Campus
            </Text>
            <Text style={[styles.noticeDate, { color: theme.textSecondary }]}>
              July 29, 2024
            </Text>
          </View>
        </View>

        <View style={styles.row}>
          <View style={[styles.infoBox, { backgroundColor: theme.card }]}>
            <Feather name="credit-card" size={20} color={theme.textSecondary} />
            <Text style={[styles.infoText, { color: theme.textPrimary }]}>
              {card?.MSSV || "****"}
            </Text>
          </View>
          <View style={[styles.infoBox, { backgroundColor: theme.card }]}>
            <Text style={[styles.infoLabel, { color: theme.textPrimary }]}>GPA</Text>
            <Text style={[styles.infoValue, { color: theme.textSecondary }]}>
              {quickGpa?.GPA?.toFixed(2) || "--"}/10.0
            </Text>
            <Text style={[styles.infoSub, { color: theme.textSecondary }]}>
              {quickGpa?.SoTinChiTichLuy || 0} tín chỉ
            </Text>
          </View>
        </View>

        {/* Thao tác nhanh */}
        
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: theme.textPrimary }]}>
            Thao tác nhanh
          </Text>
          <Feather name="settings" size={20} color={theme.icon} />
        </View>

        <QuickActions />
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scroll: { paddingHorizontal: 20, paddingBottom: 100 },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginTop: 40,
  },
  welcome: { fontSize: 14, fontFamily: "Inter", fontWeight: "400", lineHeight: 20 },
  username: { fontSize: 24, fontFamily: "Inter", fontWeight: "700", lineHeight: 32 },
  avatarWrap: { flexDirection: "row", alignItems: "center" },
  avatarBorder: { borderWidth: 2, borderRadius: 9999, padding: 2 },
  avatarInner: {
    width: 44,
    height: 44,
    borderRadius: 9999,
    alignItems: "center",
    justifyContent: "center",
  },
  avatarText: { fontSize: 16, fontFamily: "Inter", fontWeight: "400", lineHeight: 24 },
  sectionTitle: { fontSize: 18, fontFamily: "Inter", fontWeight: "700", marginTop: 20, marginBottom: 12 },
  scheduleCard: {
    borderRadius: 20,
    padding: 20,
    flexDirection: "row",
    justifyContent: "space-between",
    shadowOpacity: 0.3,
    shadowRadius: 6,
    shadowOffset: { width: 0, height: 3 },
  },
  scheduleTime: { fontSize: 14, fontWeight: "500", lineHeight: 20 },
  scheduleTitle: { fontSize: 24, fontWeight: "600", lineHeight: 32, marginVertical: 6 },
  scheduleRoom: { fontSize: 16, lineHeight: 24 },
  scheduleTeacher: { fontSize: 14, lineHeight: 20 },
  scheduleSoon: { fontSize: 14, lineHeight: 20 },
  scheduleCountdown: { fontSize: 24, fontWeight: "700", lineHeight: 32 },
  viewAll: {
    marginTop: 12,
    padding: 14,
    borderRadius: 10,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    borderWidth: 1,
  },
  viewAllText: { fontSize: 14, fontWeight: "500", lineHeight: 20 },
  notificationCard: {
    borderRadius: 20,
    padding: 16,
    flexDirection: "row",
    alignItems: "center",
  },
  noticeTitle: { fontSize: 16, fontWeight: "600", lineHeight: 20 },
  noticeDate: { fontSize: 12, lineHeight: 16, marginTop: 3 },
  row: { flexDirection: "row", justifyContent: "space-between", marginTop: 20 },
  infoBox: {
    flex: 1,
    borderRadius: 16,
    paddingVertical: 18,
    paddingHorizontal: 16,
    justifyContent: "center",
    alignItems: "center",
    marginHorizontal: 6,
    shadowColor: "#000",
    shadowOpacity: 0.15,
    shadowRadius: 8,
    shadowOffset: { width: 0, height: 3 },
  },
  infoLabel: { fontSize: 16, fontWeight: "600", lineHeight: 24, },
  infoValue: { fontSize: 14, fontWeight: "600", lineHeight: 20,},
  infoSub: { fontSize: 14, fontWeight: "400", lineHeight: 20 },
  infoText: { fontSize: 14, fontWeight: "600", lineHeight: 20 },
  sectionHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingRight: 12,
    marginTop: 20,
    marginBottom: 12,
  },
});
