import { useState, useEffect } from "react";
import { getProfile } from "../../auth/api/authAPI.js"; // Giả định đường dẫn

/**
 * Custom hook xử lý logic lấy và định dạng profile người dùng
 * @returns {object} { profile, loading, username, initials }
 */
export const useUserProfile = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const userData = await getProfile();
        setProfile(userData);
      } catch (error) {
        // Có thể thêm logic logout nếu lỗi là 401 (token hết hạn)
        console.error("❌ Lỗi khi lấy thông tin người dùng:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  const username = profile?.name || "Người dùng";
  const initials = username
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);

  return {
    profile,
    loading,
    username,
    initials,
  };
};
