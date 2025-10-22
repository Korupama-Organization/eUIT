import React from "react";
import { ScrollView, Text, View, Image, TouchableOpacity, StyleSheet } from "react-native";
import { Entypo } from "@expo/vector-icons";

const icons = {
  calendar: require("../assets/lookup_icons/calendar.png"),
  book: require("../assets/lookup_icons/book.png"),
  leaf: require("../assets/lookup_icons/leaf.png"),
  gradHat: require("../assets/lookup_icons/grad_hat.png"),
  creditCard: require("../assets/lookup_icons/credit-card.png"),
  scroll: require("../assets/lookup_icons/scroll.png"),
  blueprint: require("../assets/lookup_icons/blueprint.png"),
};

export default function LookUpScreen() {
  const features = [
    { icon: icons.calendar, title: "Xem kế hoạch năm", subtitle: "Phòng Công tác Sinh viên" },
    { icon: icons.book, title: "Tra cứu kết quả học tập", subtitle: "Phòng Dữ liệu & CNTT" },
    { icon: icons.leaf, title: "Tra cứu điểm rèn luyện", subtitle: "Phòng Đào tạo" },
    { icon: icons.gradHat, title: "Theo dõi tiến độ đào tạo", subtitle: "Phòng Đào tạo" },
    { icon: icons.creditCard, title: "Kiểm tra học phí", subtitle: "Phòng Tài chính" },
    { icon: icons.scroll, title: "Xem quy chế đào tạo", subtitle: "Phòng Đào tạo" },
    { icon: icons.blueprint, title: "Xem chương trình đào tạo", subtitle: "Phòng Đào tạo" },
  ];

  return (
    <ScrollView
      style={styles.container}
      contentContainerStyle={styles.scrollContent}
      showsVerticalScrollIndicator={false}
    >
      <Text style={styles.header}>Tra cứu</Text>
      <Text style={styles.subHeader}>
        Cung cấp các tiện ích tra cứu trực tuyến cho sinh viên
      </Text>

      {features.map((item, index) => (
        <TouchableOpacity key={index} style={styles.card}>
          <Image source={item.icon} style={styles.icon} />
          <View style={styles.textContainer}>
            <Text style={styles.title}>{item.title}</Text>
            <Text style={styles.subtitle}>{item.subtitle}</Text>
          </View>
          <Entypo name="chevron-right" size={20} color="#A1A1AA" />
        </TouchableOpacity>
      ))}

      <View style={{ height: 80 }} />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { backgroundColor: "#030E2C" },
  scrollContent: { paddingTop: 50, paddingHorizontal: 20, paddingBottom: 100 },
  header: { fontSize: 24, fontWeight: "700", color: "#FFFFFF", marginBottom: 5 },
  subHeader: { fontSize: 14, color: "#A1A1AA", marginBottom: 20 },
  card: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: "#171736",
    borderRadius: 12,
    padding: 16,
    marginBottom: 12,
    borderWidth: 1,
    borderColor: "rgba(122, 248, 255, 0.1)",
  },
  icon: { width: 30, height: 30, marginRight: 14 },
  textContainer: { flex: 1 },
  title: { fontSize: 16, color: "#FFFFFF", fontWeight: "600" },
  subtitle: { fontSize: 13, color: "#A1A1AA", marginTop: 2 },
});
