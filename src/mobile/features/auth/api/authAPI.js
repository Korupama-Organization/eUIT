const API_BASE_URL = "http://192.168.1.15:5128/api";

export class AuthApi {
  static async login(userId, password, role = "lecturer") {
    try {
      const response = await fetch(`${API_BASE_URL}/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          role,
          userId,
          password,
        }),
      });

      const text = await response.text();
      let data;
      try {
        data = text ? JSON.parse(text) : {};
      } catch {
        data = { message: "Phản hồi không hợp lệ từ server" };
      }

      if (!response.ok) {
        throw new Error(data.message || "Đăng nhập thất bại");
      }

      return { success: true, data };
    } catch (error) {
      return { success: false, error: error.message };
    }
  }
}
