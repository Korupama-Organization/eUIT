import { StyleSheet } from "react-native";

export const settingsStyles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 16,
  },
  headerTitle: {
    fontSize: 28,
    fontWeight: "bold",
  },
  scrollViewContent: {
    paddingBottom: 100,
  },
  section: {
    marginBottom: 20,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: "600",
    paddingHorizontal: 20,
    marginBottom: 4,
  },
  sectionDescription: {
    fontSize: 14,
    paddingHorizontal: 20,
    marginBottom: 12,
    lineHeight: 18,
  },
  divider: {
    height: 1,
    marginHorizontal: 20,
  },
  settingItem: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  settingInfo: {
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
  },
  settingTitle: {
    fontSize: 16,
    fontWeight: "500",
    marginLeft: 12,
  },
  settingAction: {
    // Dành cho action/switch/chevron
  },
  actionText: {
    fontSize: 16,
    fontWeight: "500",
  },
});
