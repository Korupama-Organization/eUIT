import React, { useState } from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  TextInput,
} from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../../theme/ThemeProvider";

const ScheduleScreen = () => {
  const { theme } = useTheme(); // Sử dụng theme
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedDate, setSelectedDate] = useState("2");

  // Dữ liệu lịch tháng 11
  const calendarDays = [
    { day: "CN", date: "31", isCurrentMonth: false },
    { day: "T2", date: "1", isCurrentMonth: true },
    { day: "T3", date: "2", isCurrentMonth: true },
    { day: "T4", date: "3", isCurrentMonth: true },
    { day: "T5", date: "4", isCurrentMonth: true },
    { day: "T6", date: "5", isCurrentMonth: true },
    { day: "T7", date: "6", isCurrentMonth: true },
  ];

  // Dữ liệu các ngày trong tháng
  const scheduleDays = [
    { date: "2", month: "thg 9", day: "Thứ 3", hasEvents: false },
    { date: "3", month: "thg 9", day: "Thứ 4", hasEvents: false },
    { date: "4", month: "thg 9", day: "Thứ 5", hasEvents: false },
    { date: "5", month: "thg 9", day: "Thứ 6", hasEvents: false },
    { date: "6", month: "thg 9", day: "Thứ 7", hasEvents: false },
  ];

  // Bottom tab navigation
  const bottomTabs = [
    { id: 1, title: "Tài chính", icon: "dollar-sign" },
    { id: 2, title: "Diễn vụ", icon: "clipboard" },
    { id: 3, title: "Liên trình", icon: "calendar" },
    { id: 4, title: "Chi tiêu", icon: "credit-card" },
  ];

  const handleDateSelect = (date) => {
    setSelectedDate(date);
  };

  const handleBottomTabPress = (tab) => {
    console.log("Nhấn vào tab:", tab.title);
    // Xử lý chuyển tab
  };

  return (
    <View style={[styles.container, { backgroundColor: theme.background }]}>
      {/* Header */}
      <View
        style={[
          styles.header,
          {
            backgroundColor: theme.background,
            borderBottomColor: theme.border,
          },
        ]}
      >
        <Text style={[styles.headerTitle, { color: theme.textPrimary }]}>
          Lịch giảng dạy
        </Text>

        {/* Search Bar */}
        <View
          style={[
            styles.searchContainer,
            {
              backgroundColor: theme.card,
              borderColor: theme.border,
            },
          ]}
        >
          <Feather
            name="search"
            size={20}
            color={theme.textSecondary}
            style={styles.searchIcon}
          />
          <TextInput
            style={[styles.searchInput, { color: theme.textPrimary }]}
            placeholder="Tìm kiếm"
            placeholderTextColor={theme.textSecondary}
            value={searchQuery}
            onChangeText={setSearchQuery}
          />
          {searchQuery.length > 0 && (
            <TouchableOpacity onPress={() => setSearchQuery("")}>
              <Feather name="x" size={20} color={theme.textSecondary} />
            </TouchableOpacity>
          )}
        </View>
      </View>

      <ScrollView style={styles.content} showsVerticalScrollIndicator={false}>
        {/* Calendar Header */}
        <View style={styles.calendarHeader}>
          <Text style={[styles.monthTitle, { color: theme.textPrimary }]}>
            Tháng 11
          </Text>
        </View>

        {/* Calendar Grid */}
        <View style={styles.calendarGrid}>
          {calendarDays.map((day, index) => (
            <View key={index} style={styles.calendarDay}>
              <Text style={[styles.dayLabel, { color: theme.textSecondary }]}>
                {day.day}
              </Text>
              <TouchableOpacity
                style={[
                  styles.dateButton,
                  { backgroundColor: "transparent" },
                  day.date === selectedDate && [
                    styles.selectedDate,
                    { backgroundColor: theme.primary },
                  ],
                  !day.isCurrentMonth && styles.otherMonthDate,
                ]}
                onPress={() => handleDateSelect(day.date)}
              >
                <Text
                  style={[
                    styles.dateText,
                    { color: theme.textPrimary },
                    day.date === selectedDate && styles.selectedDateText,
                    !day.isCurrentMonth && [
                      styles.otherMonthDateText,
                      { color: theme.textSecondary },
                    ],
                  ]}
                >
                  {day.date}
                </Text>
              </TouchableOpacity>
            </View>
          ))}
        </View>

        <View style={[styles.divider, { backgroundColor: theme.border }]} />

        {/* Schedule List */}
        <View style={styles.scheduleList}>
          {scheduleDays.map((scheduleDay, index) => (
            <View key={index}>
              <View style={styles.scheduleDay}>
                <View style={styles.dateInfo}>
                  <Text
                    style={[styles.dateNumber, { color: theme.textPrimary }]}
                  >
                    {scheduleDay.date}
                  </Text>
                  <View style={styles.dateDetails}>
                    <Text
                      style={[styles.monthText, { color: theme.textSecondary }]}
                    >
                      {scheduleDay.month}
                    </Text>
                    <Text
                      style={[styles.dayText, { color: theme.textPrimary }]}
                    >
                      {scheduleDay.day}
                    </Text>
                  </View>
                </View>

                <View style={styles.eventsSection}>
                  {scheduleDay.hasEvents ? (
                    <Text style={[styles.hasEventsText, { color: "#10B981" }]}>
                      Có sự kiện
                    </Text>
                  ) : (
                    <View style={styles.noEvents}>
                      <Text
                        style={[
                          styles.noEventsTitle,
                          { color: theme.textSecondary },
                        ]}
                      >
                        Không có sự kiện nào xảy ra
                      </Text>
                    </View>
                  )}
                </View>
              </View>

              {index < scheduleDays.length - 1 && (
                <View
                  style={[styles.divider, { backgroundColor: theme.border }]}
                />
              )}
            </View>
          ))}
        </View>

        {/* Bottom Space */}
        <View style={styles.bottomSpace} />
      </ScrollView>

      {/* Bottom Navigation */}
      <View
        style={[
          styles.bottomNav,
          {
            backgroundColor: theme.card,
            borderTopColor: theme.border,
          },
        ]}
      >
        {bottomTabs.map((tab) => (
          <TouchableOpacity
            key={tab.id}
            style={styles.bottomTab}
            onPress={() => handleBottomTabPress(tab)}
          >
            <Feather
              name={tab.icon}
              size={20}
              color={
                tab.title === "Liên trình" ? theme.primary : theme.textSecondary
              }
            />
            <Text
              style={[
                styles.bottomTabText,
                { color: theme.textSecondary },
                tab.title === "Liên trình" && [
                  styles.activeBottomTabText,
                  { color: theme.primary },
                ],
              ]}
            >
              {tab.title}
            </Text>
          </TouchableOpacity>
        ))}
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 16,
    borderBottomWidth: 1,
  },
  headerTitle: {
    fontSize: 28,
    fontWeight: "bold",
    marginBottom: 16,
  },
  searchContainer: {
    flexDirection: "row",
    alignItems: "center",
    borderRadius: 12,
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderWidth: 1,
  },
  searchIcon: {
    marginRight: 8,
  },
  searchInput: {
    flex: 1,
    fontSize: 16,
    padding: 0,
  },
  content: {
    flex: 1,
  },
  calendarHeader: {
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  monthTitle: {
    fontSize: 18,
    fontWeight: "600",
    textAlign: "center",
  },
  calendarGrid: {
    flexDirection: "row",
    paddingHorizontal: 10,
    marginBottom: 8,
  },
  calendarDay: {
    flex: 1,
    alignItems: "center",
    padding: 4,
  },
  dayLabel: {
    fontSize: 12,
    fontWeight: "500",
    marginBottom: 8,
  },
  dateButton: {
    width: 36,
    height: 36,
    borderRadius: 18,
    justifyContent: "center",
    alignItems: "center",
  },
  selectedDate: {
    // backgroundColor sẽ được set dynamic từ theme
  },
  otherMonthDate: {
    opacity: 0.4,
  },
  dateText: {
    fontSize: 14,
    fontWeight: "500",
  },
  selectedDateText: {
    color: "#FFFFFF",
  },
  otherMonthDateText: {
    // color sẽ được set dynamic từ theme
  },
  divider: {
    height: 1,
    marginHorizontal: 20,
  },
  scheduleList: {
    paddingVertical: 8,
  },
  scheduleDay: {
    flexDirection: "row",
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  dateInfo: {
    flexDirection: "row",
    alignItems: "center",
    width: 100,
  },
  dateNumber: {
    fontSize: 24,
    fontWeight: "bold",
    marginRight: 8,
  },
  dateDetails: {
    flex: 1,
  },
  monthText: {
    fontSize: 14,
    marginBottom: 2,
  },
  dayText: {
    fontSize: 14,
    fontWeight: "500",
  },
  eventsSection: {
    flex: 1,
    justifyContent: "center",
  },
  noEvents: {
    paddingVertical: 8,
  },
  noEventsTitle: {
    fontSize: 16,
    fontStyle: "italic",
  },
  hasEventsText: {
    fontSize: 16,
    fontWeight: "500",
  },
  bottomSpace: {
    height: 80,
  },
  bottomNav: {
    flexDirection: "row",
    borderTopWidth: 1,
    paddingHorizontal: 16,
    paddingVertical: 12,
    position: "absolute",
    bottom: 0,
    left: 0,
    right: 0,
  },
  bottomTab: {
    flex: 1,
    alignItems: "center",
    paddingVertical: 8,
  },
  bottomTabText: {
    fontSize: 12,
    marginTop: 4,
  },
  activeBottomTabText: {
    fontWeight: "500",
  },
});

export default ScheduleScreen;
