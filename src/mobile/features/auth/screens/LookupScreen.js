import React from "react";
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
} from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../../App";

const LookupScreen = () => {
  const { theme } = useTheme(); // Sử dụng theme

  // Danh sách các tính năng tra cứu
  const lookupFeatures = [
    {
      id: 1,
      title: "Xem lịch giảng dạy",
      description: "Phòng đào tạo Đại học / VPCTCBĐ",
      icon: "calendar",
      hasCheckbox: false,
    },
    {
      id: 2,
      title: "Tra cứu danh sách lớp",
      description: "Phòng đào tạo Đại học / VPCTCBĐ",
      icon: "users",
      hasCheckbox: true,
    },
    {
      id: 3,
      title: "Tra cứu kết quả học tập sinh viên",
      description: "Phòng đào tạo Đại học / VPCTCBĐ",
      icon: "bar-chart",
      hasCheckbox: true,
    },
    {
      id: 4,
      title: "Theo dõi tiến độ giảng dạy",
      description: "Phòng đào tạo Đại học / VPCTCBĐ",
      icon: "trending-up",
      hasCheckbox: true,
    },
    {
      id: 5,
      title: "Xem thông báo hội thảo",
      description: "Văn phòng khoa / Nhà trường",
      icon: "bell",
      hasCheckbox: true,
    },
    {
      id: 6,
      title: "Xem quy chế - văn bản giảng dạy",
      description: "Phòng đào tạo Đại học / VPCTCBĐ",
      icon: "file-text",
      hasCheckbox: true,
    },
    {
      id: 7,
      title: "Tra cứu đề tài nghiên cứu khoa học",
      description: "Phòng khoa học Công nghệ",
      icon: "search",
      hasCheckbox: true,
    },
  ];

  // Các nút tra cứu nhanh
  const quickActions = [
    {
      id: 1,
      title: "Giảng viên",
      icon: "user",
      color: "#2F6BFF",
    },
    {
      id: 2,
      title: "Sportfolio",
      icon: "briefcase",
      color: "#10B981",
    },
    {
      id: 3,
      title: "Chat site",
      icon: "message-circle",
      color: "#F59E0B",
    },
  ];

  const handleFeaturePress = (feature) => {
    console.log("Nhấn vào:", feature.title);
    // Xử lý navigation đến màn hình chi tiết tương ứng
  };

  const handleQuickActionPress = (action) => {
    console.log("Nhấn vào:", action.title);
    // Xử lý quick action
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
          Tra cứu
        </Text>
        <Text style={[styles.headerSubtitle, { color: theme.textSecondary }]}>
          Cung cấp các tiện ích tra cứu trực tuyến cho giảng viên
        </Text>
      </View>

      {/* Danh sách tính năng tra cứu */}
      <ScrollView
        style={styles.featuresList}
        showsVerticalScrollIndicator={false}
      >
        {lookupFeatures.map((feature, index) => (
          <TouchableOpacity
            key={feature.id}
            style={[
              styles.featureItem,
              { borderBottomColor: theme.border },
              index === lookupFeatures.length - 1 && styles.lastFeatureItem,
            ]}
            onPress={() => handleFeaturePress(feature)}
            activeOpacity={0.7}
          >
            {/* Icon */}
            <View
              style={[
                styles.featureIconContainer,
                { backgroundColor: `${theme.primary}15` },
              ]}
            >
              <Feather name={feature.icon} size={20} color={theme.primary} />
            </View>

            {/* Nội dung */}
            <View style={styles.featureContent}>
              <Text style={[styles.featureTitle, { color: theme.textPrimary }]}>
                {feature.title}
              </Text>
              <Text
                style={[
                  styles.featureDescription,
                  { color: theme.textSecondary },
                ]}
              >
                {feature.description}
              </Text>
            </View>

            {/* Checkbox hoặc mũi tên */}
            <View style={styles.featureAction}>
              {feature.hasCheckbox ? (
                <View
                  style={[
                    styles.checkbox,
                    { borderColor: theme.textSecondary },
                  ]}
                >
                  <View
                    style={[
                      styles.checkboxInner,
                      { backgroundColor: "transparent" },
                    ]}
                  />
                </View>
              ) : (
                <Feather
                  name="chevron-right"
                  size={20}
                  color={theme.textSecondary}
                />
              )}
            </View>
          </TouchableOpacity>
        ))}
      </ScrollView>

      {/* Quick Actions */}
      <View
        style={[
          styles.quickActions,
          { backgroundColor: theme.card, borderTopColor: theme.border },
        ]}
      >
        <Text
          style={[styles.quickActionsTitle, { color: theme.textSecondary }]}
        >
          TRA CỨU
        </Text>
        <View style={styles.quickActionsGrid}>
          {quickActions.map((action) => (
            <TouchableOpacity
              key={action.id}
              style={styles.quickActionButton}
              onPress={() => handleQuickActionPress(action)}
              activeOpacity={0.8}
            >
              <View
                style={[
                  styles.quickActionIcon,
                  { backgroundColor: action.color },
                ]}
              >
                <Feather name={action.icon} size={24} color="#FFFFFF" />
              </View>
              <Text
                style={[styles.quickActionText, { color: theme.textPrimary }]}
              >
                {action.title}
              </Text>
            </TouchableOpacity>
          ))}
        </View>
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
    marginBottom: 4,
  },
  headerSubtitle: {
    fontSize: 16,
    lineHeight: 20,
  },
  featuresList: {
    flex: 1,
  },
  featureItem: {
    flexDirection: "row",
    alignItems: "center",
    paddingHorizontal: 20,
    paddingVertical: 16,
    borderBottomWidth: 1,
  },
  lastFeatureItem: {
    borderBottomWidth: 0,
  },
  featureIconContainer: {
    width: 40,
    height: 40,
    borderRadius: 8,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  featureContent: {
    flex: 1,
  },
  featureTitle: {
    fontSize: 16,
    fontWeight: "500",
    marginBottom: 2,
  },
  featureDescription: {
    fontSize: 14,
  },
  featureAction: {
    paddingLeft: 8,
  },
  checkbox: {
    width: 20,
    height: 20,
    borderRadius: 4,
    borderWidth: 2,
    justifyContent: "center",
    alignItems: "center",
  },
  checkboxInner: {
    width: 10,
    height: 10,
    borderRadius: 2,
  },
  quickActions: {
    padding: 20,
    borderTopWidth: 1,
  },
  quickActionsTitle: {
    fontSize: 14,
    fontWeight: "600",
    marginBottom: 12,
    textAlign: "center",
    letterSpacing: 1,
  },
  quickActionsGrid: {
    flexDirection: "row",
    justifyContent: "space-around",
  },
  quickActionButton: {
    alignItems: "center",
    minWidth: 80,
  },
  quickActionIcon: {
    width: 56,
    height: 56,
    borderRadius: 12,
    justifyContent: "center",
    alignItems: "center",
    marginBottom: 8,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 3,
  },
  quickActionText: {
    fontSize: 12,
    fontWeight: "500",
    textAlign: "center",
  },
});

export default LookupScreen;
