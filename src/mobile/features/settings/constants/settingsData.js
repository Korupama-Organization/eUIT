// Cấu trúc Menu Cài đặt
export const settingsMenu = [
  {
    id: "general",
    title: "Cài đặt chung",
    description: "Quản lý giao diện, ngôn ngữ, và hỗ trợ.",
    items: [
      {
        key: "theme",
        title: "Chế độ tối (Dark Mode)",
        type: "toggle",
        icon: "moon",
      },
      { key: "language", title: "Ngôn ngữ", type: "navigate", icon: "globe" },
      {
        key: "help",
        title: "Trung tâm trợ giúp",
        type: "navigate",
        icon: "help-circle",
      },
    ],
  },
  {
    id: "notifications",
    title: "Thông báo & Cập nhật",
    description: "Quản lý các loại thông báo nhận từ hệ thống.",
    items: [
      {
        key: "allNotifications",
        title: "Nhận tất cả thông báo đẩy",
        type: "toggle",
      },
      {
        key: "courseProgress",
        title: "Tiến độ học tập/Giảng dạy",
        type: "toggle",
      },
      { key: "teachingBreak", title: "Thông báo nghỉ bù", type: "toggle" },
      { key: "makeUpClass", title: "Thông báo dạy bù", type: "toggle" },
    ],
  },
  {
    id: "account",
    title: "Tài khoản",
    description: "Quản lý thông tin tài khoản và đăng xuất.",
    items: [
      {
        key: "profile",
        title: "Cập nhật hồ sơ",
        type: "navigate",
        icon: "user",
      },
      { key: "security", title: "Bảo mật", type: "navigate", icon: "lock" },
      { key: "logout", title: "Đăng xuất", type: "action", icon: "log-out" },
    ],
  },
];
