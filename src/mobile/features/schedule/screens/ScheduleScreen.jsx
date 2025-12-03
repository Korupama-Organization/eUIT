import React, { useState } from "react";
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  TextInput,
} from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../../App";

// Imports từ các file đã tách
import {
  calendarDays,
  scheduleDays,
  bottomTabs,
} from "../constants/scheduleData";
import { scheduleStyles as styles } from "../styles/scheduleStyles";

const ScheduleScreen = () => {
  const { theme } = useTheme(); // Sử dụng theme
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedDate, setSelectedDate] = useState("2");

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

export default ScheduleScreen;
