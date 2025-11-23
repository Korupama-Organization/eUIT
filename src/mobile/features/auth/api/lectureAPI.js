/**
 * Lecture API - Quản lý dữ liệu giảng viên
 */
import AsyncStorage from "@react-native-async-storage/async-storage";
import { API_BASE_URL } from "@env";
import { handleResponse, createHeaders } from "./authAPI.js";

/**
 * Các endpoint liên quan đến giảng viên
 */
const LECTURE_ENDPOINTS = {
  // Thông tin giảng viên cơ bản
  BASE: `${API_BASE_URL}/api/giangvien`,
  CURRENT_USER: `${API_BASE_URL}/users/me`,

  // Lớp học
  CLASSES: `${API_BASE_URL}/classes`,
  MY_CLASSES: `${API_BASE_URL}/api/classes`,
  CLASS_DETAIL: `${API_BASE_URL}/api/classes`,

  // Phòng học
  ROOMS: `${API_BASE_URL}/rooms`,

  // Lịch giảng dạy
  TEACHING_SCHEDULE: `${API_BASE_URL}/api/teaching-schedule`,

  // Lịch thi
  EXAM_SCHEDULE: `${API_BASE_URL}/api/exam-schedule`,
  EXAM_PERIODS: `${API_BASE_URL}/api/exam-periods`,
  SEMESTERS: `${API_BASE_URL}/api/semesters`,

  // Lịch họp
  MEETINGS: `${API_BASE_URL}/api/meetings`,

  // Báo bù
  MAKEUP_CREATE: `${API_BASE_URL}/makeup/create`,
  MAKEUP_LIST: `${API_BASE_URL}/makeup/list`,
  MAKEUP_DETAIL: `${API_BASE_URL}/makeup`,
  MAKEUP_UPDATE: `${API_BASE_URL}/makeup`,
  MAKEUP_DELETE: `${API_BASE_URL}/makeup`,

  // Báo nghỉ
  ABSENCE_CREATE: `${API_BASE_URL}/absence/create`,
  ABSENCE_LIST: `${API_BASE_URL}/absence/list`,
  ABSENCE_DETAIL: `${API_BASE_URL}/absence`,
  ABSENCE_UPDATE: `${API_BASE_URL}/absence`,
  ABSENCE_DELETE: `${API_BASE_URL}/absence`,

  // Thông báo
  ANNOUNCEMENT_CREATE: `${API_BASE_URL}/announcement/create`,
  ANNOUNCEMENT_LIST: `${API_BASE_URL}/announcement/list`,
  ANNOUNCEMENT_DETAIL: `${API_BASE_URL}/announcement`,
  ANNOUNCEMENT_DELETE: `${API_BASE_URL}/announcement`,

  // Tra cứu
  STUDENTS: `${API_BASE_URL}/api/students`,
  STUDENT_DETAIL: `${API_BASE_URL}/api/students`,
  HOMEROOM_CLASSES: `${API_BASE_URL}/api/homeroom-classes`,

  // Thông báo hệ thống
  NOTIFICATIONS: `${API_BASE_URL}/api/notifications`,
};

// ==================== THÔNG TIN GIẢNG VIÊN ====================

/**
 * Lấy thông tin giảng viên theo mã
 * @param {string} maGiangVien - Mã giảng viên (vd: 80068)
 * @returns {Promise<object>}
 */
export const getGiangVienById = async (maGiangVien) => {
  try {
    const response = await fetch(`${LECTURE_ENDPOINTS.BASE}/${maGiangVien}`, {
      method: "GET",
      headers: await createHeaders(true),
    });
    const data = await handleResponse(response);

    return {
      maGiangVien: data.maGiangVien,
      hoTen: data.hoTen,
      khoaBoMon: data.khoaBoMon,
      soDienThoai: data.soDienThoai,
      diaChiThuongTru: data.diaChiThuongTru,
      initials: data.hoTen
        .split(" ")
        .slice(-2)
        .map((x) => x[0])
        .join("")
        .toUpperCase(),
    };
  } catch (error) {
    console.error("❌ Lỗi khi lấy giảng viên theo mã:", error);
    throw error;
  }
};

/**
 * Lấy thông tin giảng viên hiện tại
 */
export const getCurrentGiangVien = async () => {
  try {
    const maGiangVien = await AsyncStorage.getItem("ma_giang_vien");
    if (!maGiangVien) {
      throw new Error("Không tìm thấy mã giảng viên trong AsyncStorage");
    }
    return await getGiangVienById(maGiangVien);
  } catch (error) {
    console.error("❌ Lỗi khi lấy thông tin giảng viên hiện tại:", error);
    throw error;
  }
};

/**
 * Lấy thông tin người dùng hiện tại (từ /users/me)
 */
export const getCurrentUser = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.CURRENT_USER, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy thông tin người dùng:", error);
    throw error;
  }
};

// ==================== LỚP HỌC ====================

/**
 * Lấy danh sách lớp học mà giảng viên phụ trách
 */
export const getMyClasses = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.MY_CLASSES, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách lớp học:", error);
    throw error;
  }
};

/**
 * Lấy thông tin chi tiết một lớp học
 * @param {string} classId - ID hoặc mã lớp học (vd: IE307.Q11)
 */
export const getClassDetail = async (classId) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.CLASS_DETAIL}/${classId}`,
      {
        method: "GET",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy thông tin lớp học:", error);
    throw error;
  }
};

// ==================== PHÒNG HỌC ====================

/**
 * Lấy danh sách phòng học
 */
export const getRooms = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.ROOMS, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách phòng học:", error);
    throw error;
  }
};

// ==================== LỊCH GIẢNG DẠY ====================

/**
 * Lấy lịch giảng dạy theo tháng hoặc tuần
 * @param {object} params - { month?, week?, year? }
 */
export const getTeachingSchedule = async (params = {}) => {
  try {
    const queryString = new URLSearchParams(params).toString();
    const url = queryString
      ? `${LECTURE_ENDPOINTS.TEACHING_SCHEDULE}?${queryString}`
      : LECTURE_ENDPOINTS.TEACHING_SCHEDULE;

    const response = await fetch(url, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy lịch giảng dạy:", error);
    throw error;
  }
};

// ==================== LỊCH THI ====================

/**
 * Lấy danh sách kỳ thi
 */
export const getExamPeriods = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.EXAM_PERIODS, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách kỳ thi:", error);
    throw error;
  }
};

/**
 * Lấy danh sách học kỳ
 */
export const getSemesters = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.SEMESTERS, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách học kỳ:", error);
    throw error;
  }
};

/**
 * Lấy lịch thi theo kỳ, học kỳ, năm học và mã giảng viên
 * @param {object} params - { examPeriodId?, semesterId?, year?, lecturerId? }
 */
export const getExamSchedule = async (params = {}) => {
  try {
    const queryString = new URLSearchParams(params).toString();
    const url = queryString
      ? `${LECTURE_ENDPOINTS.EXAM_SCHEDULE}?${queryString}`
      : LECTURE_ENDPOINTS.EXAM_SCHEDULE;

    const response = await fetch(url, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy lịch thi:", error);
    throw error;
  }
};

// ==================== LỊCH HỌP ====================

/**
 * Lấy danh sách lịch họp của giảng viên
 */
export const getMeetings = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.MEETINGS, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy lịch họp:", error);
    throw error;
  }
};

// ==================== BÁO BÙ ====================

/**
 * Gửi yêu cầu báo bù
 * @param {object} data - { ngay, lop, phongHoc, lyDo }
 */
export const createMakeup = async (data) => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.MAKEUP_CREATE, {
      method: "POST",
      headers: await createHeaders(true),
      body: JSON.stringify(data),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi gửi báo bù:", error);
    throw error;
  }
};

/**
 * Lấy danh sách báo bù đã gửi
 */
export const getMakeupList = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.MAKEUP_LIST, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách báo bù:", error);
    throw error;
  }
};

/**
 * Xem chi tiết báo bù
 * @param {string} id - ID báo bù
 */
export const getMakeupDetail = async (id) => {
  try {
    const response = await fetch(`${LECTURE_ENDPOINTS.MAKEUP_DETAIL}/${id}`, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy chi tiết báo bù:", error);
    throw error;
  }
};

/**
 * Cập nhật báo bù
 * @param {string} id - ID báo bù
 * @param {object} data - Dữ liệu cập nhật
 */
export const updateMakeup = async (id, data) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.MAKEUP_UPDATE}/${id}/update`,
      {
        method: "PUT",
        headers: await createHeaders(true),
        body: JSON.stringify(data),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi cập nhật báo bù:", error);
    throw error;
  }
};

/**
 * Xóa báo bù
 * @param {string} id - ID báo bù
 */
export const deleteMakeup = async (id) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.MAKEUP_DELETE}/${id}/delete`,
      {
        method: "DELETE",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi xóa báo bù:", error);
    throw error;
  }
};

// ==================== BÁO NGHỈ ====================

/**
 * Gửi yêu cầu báo nghỉ
 * @param {object} data - { lyDo, ngayBatDau, ngayKetThuc, ... }
 */
export const createAbsence = async (data) => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.ABSENCE_CREATE, {
      method: "POST",
      headers: await createHeaders(true),
      body: JSON.stringify(data),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi gửi báo nghỉ:", error);
    throw error;
  }
};

/**
 * Lấy danh sách báo nghỉ
 */
export const getAbsenceList = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.ABSENCE_LIST, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách báo nghỉ:", error);
    throw error;
  }
};

/**
 * Xem chi tiết báo nghỉ
 * @param {string} id - ID báo nghỉ
 */
export const getAbsenceDetail = async (id) => {
  try {
    const response = await fetch(`${LECTURE_ENDPOINTS.ABSENCE_DETAIL}/${id}`, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy chi tiết báo nghỉ:", error);
    throw error;
  }
};

/**
 * Cập nhật báo nghỉ
 * @param {string} id - ID báo nghỉ
 * @param {object} data - Dữ liệu cập nhật
 */
export const updateAbsence = async (id, data) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.ABSENCE_UPDATE}/${id}/update`,
      {
        method: "PUT",
        headers: await createHeaders(true),
        body: JSON.stringify(data),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi cập nhật báo nghỉ:", error);
    throw error;
  }
};

/**
 * Xóa báo nghỉ
 * @param {string} id - ID báo nghỉ
 */
export const deleteAbsence = async (id) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.ABSENCE_DELETE}/${id}/delete`,
      {
        method: "DELETE",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi xóa báo nghỉ:", error);
    throw error;
  }
};

// ==================== THÔNG BÁO ====================

/**
 * Gửi thông báo đến lớp
 * @param {object} data - { lopId, tieuDe, noiDung, ... }
 */
export const createAnnouncement = async (data) => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.ANNOUNCEMENT_CREATE, {
      method: "POST",
      headers: await createHeaders(true),
      body: JSON.stringify(data),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi gửi thông báo:", error);
    throw error;
  }
};

/**
 * Lấy danh sách thông báo đã gửi
 */
export const getAnnouncementList = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.ANNOUNCEMENT_LIST, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách thông báo:", error);
    throw error;
  }
};

/**
 * Xem chi tiết thông báo
 * @param {string} id - ID thông báo
 */
export const getAnnouncementDetail = async (id) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.ANNOUNCEMENT_DETAIL}/${id}`,
      {
        method: "GET",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy chi tiết thông báo:", error);
    throw error;
  }
};

/**
 * Xóa thông báo
 * @param {string} id - ID thông báo
 */
export const deleteAnnouncement = async (id) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.ANNOUNCEMENT_DELETE}/${id}`,
      {
        method: "DELETE",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi xóa thông báo:", error);
    throw error;
  }
};

// ==================== TRA CỨU ====================

/**
 * Tìm kiếm sinh viên
 * @param {object} params - { maSinhVien?, hoTen?, lopId? }
 */
export const searchStudents = async (params = {}) => {
  try {
    const queryString = new URLSearchParams(params).toString();
    const url = queryString
      ? `${LECTURE_ENDPOINTS.STUDENTS}?${queryString}`
      : LECTURE_ENDPOINTS.STUDENTS;

    const response = await fetch(url, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi tìm kiếm sinh viên:", error);
    throw error;
  }
};

/**
 * Lấy thông tin chi tiết sinh viên
 * @param {string} studentId - Mã sinh viên
 */
export const getStudentDetail = async (studentId) => {
  try {
    const response = await fetch(
      `${LECTURE_ENDPOINTS.STUDENT_DETAIL}/${studentId}`,
      {
        method: "GET",
        headers: await createHeaders(true),
      }
    );
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy thông tin sinh viên:", error);
    throw error;
  }
};

/**
 * Lấy danh sách lớp sinh hoạt
 */
export const getHomeroomClasses = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.HOMEROOM_CLASSES, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy danh sách lớp sinh hoạt:", error);
    throw error;
  }
};

// ==================== THÔNG BÁO HỆ THỐNG ====================

/**
 * Lấy thông báo mới gửi tới giảng viên
 */
export const getNotifications = async () => {
  try {
    const response = await fetch(LECTURE_ENDPOINTS.NOTIFICATIONS, {
      method: "GET",
      headers: await createHeaders(true),
    });
    return await handleResponse(response);
  } catch (error) {
    console.error("❌ Lỗi khi lấy thông báo:", error);
    throw error;
  }
};

// Export endpoints
export { LECTURE_ENDPOINTS };
