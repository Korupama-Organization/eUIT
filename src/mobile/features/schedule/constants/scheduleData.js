// Dữ liệu lịch tháng
export const calendarDays = [
  { day: "CN", date: "31", isCurrentMonth: false },
  { day: "T2", date: "1", isCurrentMonth: true },
  { day: "T3", date: "2", isCurrentMonth: true },
  { day: "T4", date: "3", isCurrentMonth: true },
  { day: "T5", date: "4", isCurrentMonth: true },
  { day: "T6", date: "5", isCurrentMonth: true },
  { day: "T7", date: "6", isCurrentMonth: true },
];

// Dữ liệu các ngày có/không có sự kiện
export const scheduleDays = [
  { date: "2", month: "thg 9", day: "Thứ 3", hasEvents: false },
  { date: "3", month: "thg 9", day: "Thứ 4", hasEvents: false },
  { date: "4", month: "thg 9", day: "Thứ 5", hasEvents: false },
  { date: "5", month: "thg 9", day: "Thứ 6", hasEvents: false },
  { date: "6", month: "thg 9", day: "Thứ 7", hasEvents: false },
];

// Dữ liệu thanh điều hướng dưới cùng
export const bottomTabs = [
  { id: 1, title: "Tài chính", icon: "dollar-sign" },
  { id: 2, title: "Diễn vụ", icon: "clipboard" },
  { id: 3, title: "Liên trình", icon: "calendar" },
  { id: 4, title: "Chi tiêu", icon: "credit-card" },
];
