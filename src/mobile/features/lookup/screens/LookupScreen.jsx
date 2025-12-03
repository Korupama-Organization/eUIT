import React from "react";
import { View, Text, ScrollView, TouchableOpacity } from "react-native";
import { Feather } from "@expo/vector-icons";
import { useTheme } from "../../../App";

// Imports từ các file đã tách
import { lookupFeatures, quickActions } from "../constants/lookupData";
import { lookupStyles as styles } from "../styles/lookupStyles";

const LookupScreen = () => {
  const { theme } = useTheme(); // Sử dụng theme

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
                // Hiện tại là một checkbox không chọn, có thể dùng Icon thay View
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

export default LookupScreen;
