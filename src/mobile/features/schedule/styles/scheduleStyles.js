import { StyleSheet } from "react-native";

export const scheduleStyles = StyleSheet.create({
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
