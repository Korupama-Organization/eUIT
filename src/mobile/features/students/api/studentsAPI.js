/**
 * studentsAPI.js
 * * Giao tiếp với các API dành cho sinh viên (StudentsController)
 * * Sử dụng logic xác thực và làm mới token từ authAPI.js
 */

import { API_BASE_URL } from '@env';
// ĐÃ SỬA ĐƯỜNG DẪN: '../../auth/api/authAPI' để đi từ students/api/ lên features/ rồi vào auth/api/
import { 
    getStoredTokens, 
    refreshToken, 
    clearTokens,
    AUTH_ENDPOINTS 
} from '../../auth/api/authAPI'; 

// Định nghĩa các endpoints cho StudentController
const STUDENT_ENDPOINTS = {
    NEXT_CLASS: `${API_BASE_URL}/students/nextclass`,
    STUDENT_CARD: `${API_BASE_URL}/students/card`,
    QUICK_GPA: `${API_BASE_URL}/students/quickgpa`,
    ACADEMIC_RESULTS: `${API_BASE_URL}/students/academicresults`,
    ACADEMIC_SCHEDULE: `${API_BASE_URL}/students/schedule`,
};

/**
 * Hàm trợ giúp để tạo request headers có kèm Access Token
 * @returns {Promise<object>} Headers
 */
const createAuthHeaders = async () => {
    const tokens = await getStoredTokens();
    const accessToken = tokens?.accessToken;

    if (!accessToken) {
        // Nếu không có token, người dùng cần phải đăng nhập lại
        throw new Error("Access Token not found. Please log in again.");
    }

    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`,
    };
};

/**
 * Helper function để đọc body response lỗi, an toàn cho cả JSON và TEXT, 
 * và xử lý lỗi "Already read" bằng cách sử dụng clone.
 * @param {Response} response - Đối tượng Response bị lỗi
 * @returns {Promise<string>} Chi tiết lỗi dưới dạng chuỗi JSON hoặc Text.
 */
const getErrorBodyDetails = async (response) => {
    const responseClone = response.clone();
    let errorBody = "No details available";
    
    // 1. Kiểm tra Content-Type
    const contentType = response.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
        try {
            // Thử đọc JSON
            errorBody = await response.json();
            // Chuyển đối tượng JSON thành chuỗi để hiển thị
            errorBody = JSON.stringify(errorBody);
        } catch (e) {
            // Nếu JSON parsing thất bại (ví dụ: body rỗng), thử đọc text
            try {
                errorBody = await responseClone.text();
            } catch (e) {
                errorBody = "Failed to parse error body (JSON/Text).";
            }
        }
    } else {
        // Nếu không phải JSON, đọc dưới dạng text
        try {
            errorBody = await responseClone.text();
        } catch (e) {
            errorBody = "Failed to read error body as text.";
        }
    }
    return errorBody;
};


/**
 * Helper function chung để thực hiện request và xử lý lỗi 401 (Interceptor cơ bản)
 * @param {string} url - URL API đầy đủ
 * @param {object} options - Các tùy chọn fetch (method, body,...)
 * @returns {Promise<any>} Dữ liệu JSON từ API.
 */
const fetchWithAuth = async (url, options = {}) => {
    
    // --- Lần gọi đầu tiên ---
    try {
        const headers = await createAuthHeaders();
        const response = await fetch(url, { ...options, headers: { ...options.headers, ...headers } });

        if (response.ok) {
            // Nếu response là OK (200-299), chỉ cố gắng đọc JSON.
            return response.status === 204 ? null : await response.json();
        }

        if (response.status === 401 || response.status === 403) {
            // Token có thể hết hạn hoặc không hợp lệ -> Cần Refresh
            console.warn(`[${url}] Unauthorized (401/403). Trying to refresh token...`);
            throw { status: response.status, isAuthError: true }; 
        }

        // Xử lý các lỗi HTTP khác (non-OK, non-401/403)
        const errorBody = await getErrorBodyDetails(response);
        
        // Throw Error với thông tin chi tiết (đã được định dạng thành chuỗi)
        throw new Error(`API Error: ${response.status} from ${url}. Details: ${errorBody}`);

    } catch (error) {
        
        // --- Xử lý Auth Error và Retry ---
        if (error.isAuthError) {
            try {
                // 1. Cố gắng làm mới token
                await refreshToken(); 
                console.log(`[${url}] Token refreshed successfully. Retrying request...`);

                // 2. Thử lại request với token mới
                const newHeaders = await createAuthHeaders();
                const retryResponse = await fetch(url, { ...options, headers: { ...options.headers, ...newHeaders } });

                if (retryResponse.ok) {
                    return retryResponse.status === 204 ? null : await retryResponse.json();
                }
                
                // Nếu vẫn lỗi 401/403 sau khi refresh -> Refresh token cũng đã hết hạn
                if (retryResponse.status === 401 || retryResponse.status === 403) {
                    await clearTokens(); // Xóa tất cả token và buộc đăng nhập lại
                    throw new Error("Session expired. Please log in again.");
                }

                // Lỗi HTTP khác trong lần retry
                const retryErrorBody = await getErrorBodyDetails(retryResponse);

                throw new Error(`API Retry Error: ${retryResponse.status} from ${url}. Details: ${retryErrorBody}`);

            } catch (refreshError) {
                // Xử lý lỗi khi làm mới token (ví dụ: Refresh token hết hạn)
                console.error(`[${url}] Failed to refresh token or session expired:`, refreshError);
                await clearTokens(); 
                throw new Error("Session expired. Please log in again.");
            }
        }
        
        // Lỗi mạng hoặc lỗi không liên quan đến Auth
        console.error(`[${url}] Network or unknown error:`, error);
        throw error;
    }
};


// -------------------------------------------------------------------
// --- Hàm API công khai ---
// -------------------------------------------------------------------

/**
 * 1. Lấy thông tin lớp học tiếp theo của sinh viên.
 * Endpoint: GET /nextclass
 * @returns {Promise<object | null>} NextClassDto hoặc null.
 */
export const getNextClass = async () => {
    console.log('Fetching next class info...');
    return fetchWithAuth(STUDENT_ENDPOINTS.NEXT_CLASS, { method: 'GET' });
};

/**
 * 2. Lấy thông tin thẻ sinh viên (Card Info).
 * Endpoint: GET /card
 * @returns {Promise<object>} StudentCardDto.
 */
export const getStudentCard = async () => {
    console.log('Fetching student card info...');
    return fetchWithAuth(STUDENT_ENDPOINTS.STUDENT_CARD, { method: 'GET' });
};

/**
 * 3. Lấy GPA nhanh và số tín chỉ tích lũy.
 * Endpoint: GET /quickgpa
 * @returns {Promise<object>} QuickGpaDto.
 */
export const getQuickGpa = async () => {
    console.log('Fetching quick GPA...');
    return fetchWithAuth(STUDENT_ENDPOINTS.QUICK_GPA, { method: 'GET' });
};

/**
 * 4. Lấy chi tiết kết quả học tập của sinh viên.
 * Endpoint: GET /academicresults
 * @returns {Promise<Array<object>>} Mảng AcademicResultDTO.
 */
export const getAcademicResults = async () => {
    console.log('Fetching academic results...');
    return fetchWithAuth(STUDENT_ENDPOINTS.ACADEMIC_RESULTS, { method: 'GET' });
};

/**
 * 5. Lấy lịch học đầy đủ cho một học kỳ cụ thể.
 * Endpoint: GET /schedule/{hocKy}
 * @param {string} hocKy 
 * @returns {Promise<Array<object> | null>} Mảng FullScheduleDto hoặc null nếu 204.
 */
export const getAcademicSchedule = async (hocKy) => {
    if (!hocKy) {
        throw new Error("hocKy parameter is required for getAcademicSchedule.");
    }
    console.log(`Fetching full schedule for semester ${hocKy}...`);
    
    const url = `${STUDENT_ENDPOINTS.ACADEMIC_SCHEDULE}/${hocKy}`; 
    
    return fetchWithAuth(url, { method: 'GET' });
};

