/**
 * Home API - Giao tiếp với backend StudentsController
 */
import { API_BASE_URL } from "@env";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { getStoredTokens } from "../../auth/api/authAPI";

/* -------- Helper chung -------- */
const createHeaders = async () => {
  const tokens = await getStoredTokens();
  const headers = { "Content-Type": "application/json" };
  if (tokens?.accessToken) headers.Authorization = `Bearer ${tokens.accessToken}`;
  return headers;
};

const handleResponse = async (response) => {
  const data = await response.json().catch(() => ({}));
  if (!response.ok) throw { status: response.status, message: data.message || "Server error" };
  return data;
};

/* -------- API chính -------- */
const BASE = `${API_BASE_URL}/api/students`;
/**  Lấy lớp học kế tiếp (NextClass) */
export const fetchNextClass = async () => {
  const url = `${BASE}/nextclass`;
  try {
    const res = await fetch(url, { method: "GET", headers: await createHeaders() });
    const data = await handleResponse(res);
    await AsyncStorage.setItem("@home/nextclass", JSON.stringify(data));
    return data;
  } catch (error) {
    console.error("❌ fetchNextClass error:", error);
    const cached = await AsyncStorage.getItem("@home/nextclass");
    return cached ? JSON.parse(cached) : null;
  }
};

/** Lấy thẻ sinh viên (MSSV, họ tên, ngành, ảnh thẻ) */
export const fetchStudentCard = async () => {
  const url = `${BASE}/card`;
  try {
    const res = await fetch(url, { method: "GET", headers: await createHeaders() });
    const data = await handleResponse(res);
    await AsyncStorage.setItem("@home/student_card", JSON.stringify(data));
    return data;
  } catch (error) {
    console.error("❌ fetchStudentCard error:", error);
    const cached = await AsyncStorage.getItem("@home/student_card");
    return cached ? JSON.parse(cached) : null;
  }
};

/** Lấy GPA nhanh (QuickGpaDto) */
export const fetchQuickGpa = async () => {
  const url = `${BASE}/quickgpa`;
  try {
    const res = await fetch(url, { method: "GET", headers: await createHeaders() });
    const data = await handleResponse(res);
    await AsyncStorage.setItem("@home/quickgpa", JSON.stringify(data));
    return data;
  } catch (error) {
    console.error("❌ fetchQuickGpa error:", error);
    const cached = await AsyncStorage.getItem("@home/quickgpa");
    return cached ? JSON.parse(cached) : { gpa: 0, soTinChiTichLuy: 0 };
  }
};

/** Lấy toàn bộ kết quả học tập */
export const fetchAcademicResults = async () => {
  const url = `${BASE}/academicresults`;
  try {
    const res = await fetch(url, { method: "GET", headers: await createHeaders() });
    return await handleResponse(res);
  } catch (error) {
    console.error(" fetchAcademicResults error:", error);
    return [];
  }
};

/** Tổng hợp tất cả dữ liệu cho trang Home */
export const fetchHomeData = async () => {
  try {
    const [card, nextClass, quickGpa] = await Promise.all([
      fetchStudentCard(),
      fetchNextClass(),
      fetchQuickGpa(),
    ]);
    return { card, nextClass, quickGpa };
  } catch (error) {
    console.error(" fetchHomeData error:", error);
    throw error;
  }
};
