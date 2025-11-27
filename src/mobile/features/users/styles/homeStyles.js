import { StyleSheet } from "react-native";

export const homeStyles = StyleSheet.create({
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
