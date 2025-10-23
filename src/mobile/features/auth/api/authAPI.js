/**
 * Auth API - Giao tiếp với backend để xác thực người dùng
 */

import AsyncStorage from '@react-native-async-storage/async-storage';
import { AUTH_ERRORS } from '../types/auth.types.js';

// Cấu hình API
// Thử các URL khác nhau tùy theo môi trường:
// - Android Emulator: 'http://10.0.2.2:5128/api'  
// - iOS Simulator: 'http://localhost:5128/api'
// - Real device: 'http://192.168.1.x:5128/api' (thay x bằng IP máy tính)
const API_BASE_URL = 'http://192.168.1.84:5128/api'; 
const AUTH_ENDPOINTS = {
  LOGIN: '/auth/login',
  REFRESH: '/auth/refresh', 
  LOGOUT: '/auth/logout',
  LOGOUT_ALL: '/auth/logout-all',
  PROFILE: '/auth/profile'
};

// Keys để lưu token trong AsyncStorage
const STORAGE_KEYS = {
  ACCESS_TOKEN: '@auth/access_token',
  REFRESH_TOKEN: '@auth/refresh_token', 
  ACCESS_TOKEN_EXPIRY: '@auth/access_token_expiry',
  REFRESH_TOKEN_EXPIRY: '@auth/refresh_token_expiry'
};

/**
 * Helper function để tạo request headers
 */
const createHeaders = async (includeAuth = false) => {
  const headers = {
    'Content-Type': 'application/json',
  };

  if (includeAuth) {
    const accessToken = await AsyncStorage.getItem(STORAGE_KEYS.ACCESS_TOKEN);
    if (accessToken) {
      headers.Authorization = `Bearer ${accessToken}`;
    }
  }

  return headers;
};

/**
 * Helper function để xử lý response
 */
const handleResponse = async (response) => {
  const data = await response.json();
  
  if (!response.ok) {
    throw {
      message: data.message || AUTH_ERRORS.SERVER_ERROR,
      status: response.status,
      data
    };
  }

  return data;
};

/**
 * Lưu tokens vào AsyncStorage
 */
const storeTokens = async (tokens) => {
  try {
    await AsyncStorage.multiSet([
      [STORAGE_KEYS.ACCESS_TOKEN, tokens.accessToken],
      [STORAGE_KEYS.REFRESH_TOKEN, tokens.refreshToken],
      [STORAGE_KEYS.ACCESS_TOKEN_EXPIRY, tokens.accessTokenExpiry],
      [STORAGE_KEYS.REFRESH_TOKEN_EXPIRY, tokens.refreshTokenExpiry || '']
    ]);
  } catch (error) {
    console.error('Error storing tokens:', error);
    throw error;
  }
};

/**
 * Xóa tokens khỏi AsyncStorage
 */
const clearTokens = async () => {
  try {
    await AsyncStorage.multiRemove([
      STORAGE_KEYS.ACCESS_TOKEN,
      STORAGE_KEYS.REFRESH_TOKEN,
      STORAGE_KEYS.ACCESS_TOKEN_EXPIRY,
      STORAGE_KEYS.REFRESH_TOKEN_EXPIRY
    ]);
  } catch (error) {
    console.error('Error clearing tokens:', error);
  }
};

/**
 * Lấy tokens từ AsyncStorage
 */
const getStoredTokens = async () => {
  try {
    const tokens = await AsyncStorage.multiGet([
      STORAGE_KEYS.ACCESS_TOKEN,
      STORAGE_KEYS.REFRESH_TOKEN,
      STORAGE_KEYS.ACCESS_TOKEN_EXPIRY,
      STORAGE_KEYS.REFRESH_TOKEN_EXPIRY
    ]);

    return {
      accessToken: tokens[0][1],
      refreshToken: tokens[1][1],
      accessTokenExpiry: tokens[2][1],
      refreshTokenExpiry: tokens[3][1]
    };
  } catch (error) {
    console.error('Error getting stored tokens:', error);
    return null;
  }
};

/**
 * API Functions
 */

/**
 * Đăng nhập
 * @param {import('../types/auth.types.js').LoginRequest} credentials 
 * @returns {Promise<import('../types/auth.types.js').LoginResponse>}
 */
export const login = async (credentials) => {
  const fullUrl = `${API_BASE_URL}${AUTH_ENDPOINTS.LOGIN}`;
  console.log('🔗 Login URL:', fullUrl);
  console.log('📤 Login data:', credentials);
  
  try {
    const headers = await createHeaders(false);
    console.log('📋 Headers:', headers);
    
    // Tạo AbortController để có timeout
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 10000); // 10 seconds timeout

    const response = await fetch(fullUrl, {
      method: 'POST',
      headers: headers,
      body: JSON.stringify(credentials),
      signal: controller.signal
    });

    clearTimeout(timeoutId);

    console.log('📥 Response status:', response.status);
    console.log('📥 Response ok:', response.ok);

    const data = await handleResponse(response);
    console.log('✅ Login success:', data);
    
    // Lưu tokens vào AsyncStorage
    await storeTokens(data);
    
    return data;
  } catch (error) {
    console.error('❌ Login error details:', {
      message: error.message,
      status: error.status,
      name: error.name,
      stack: error.stack
    });
    
    // Kiểm tra loại lỗi
    if (error.name === 'AbortError') {
      throw { 
        ...error, 
        message: 'Request timeout - Server không phản hồi sau 10 giây' 
      };
    } else if (error.name === 'TypeError' && (
      error.message.includes('Network request failed') || 
      error.message.includes('Failed to fetch') ||
      error.message.includes('fetch is not defined')
    )) {
      throw { 
        ...error, 
        message: `Network Error - Không kết nối được:\n• Server: ${fullUrl}\n• Kiểm tra: Backend có chạy? Network có ổn?` 
      };
    } else if (error.status === 401) {
      throw { ...error, message: AUTH_ERRORS.INVALID_CREDENTIALS };
    } else if (!error.status) {
      throw { ...error, message: AUTH_ERRORS.NETWORK_ERROR };
    }
    
    throw error;
  }
};

/**
 * Làm mới Access Token
 * @returns {Promise<import('../types/auth.types.js').RefreshTokenResponse>}
 */
export const refreshToken = async () => {
  try {
    const tokens = await getStoredTokens();
    if (!tokens?.refreshToken) {
      throw { message: AUTH_ERRORS.INVALID_TOKEN };
    }

    const response = await fetch(`${API_BASE_URL}${AUTH_ENDPOINTS.REFRESH}`, {
      method: 'POST',
      headers: await createHeaders(false),
      body: JSON.stringify({ refreshToken: tokens.refreshToken })
    });

    const data = await handleResponse(response);
    
    // Cập nhật Access Token mới
    await AsyncStorage.multiSet([
      [STORAGE_KEYS.ACCESS_TOKEN, data.accessToken],
      [STORAGE_KEYS.ACCESS_TOKEN_EXPIRY, data.accessTokenExpiry]
    ]);

    // Nếu có Refresh Token mới, cập nhật luôn
    if (data.refreshToken) {
      await AsyncStorage.setItem(STORAGE_KEYS.REFRESH_TOKEN, data.refreshToken);
    }
    
    return data;
  } catch (error) {
    console.error('Refresh token error:', error);
    
    if (error.status === 401) {
      // Refresh token hết hạn, xóa tất cả tokens
      await clearTokens();
      throw { ...error, message: AUTH_ERRORS.TOKEN_EXPIRED };
    }
    
    throw error;
  }
};

/**
 * Lấy thông tin profile người dùng
 * @returns {Promise<import('../types/auth.types.js').UserProfile>}
 */
export const getProfile = async () => {
  try {
    const response = await fetch(`${API_BASE_URL}${AUTH_ENDPOINTS.PROFILE}`, {
      method: 'GET',
      headers: await createHeaders(true)
    });

    return await handleResponse(response);
  } catch (error) {
    console.error('Get profile error:', error);
    
    if (error.status === 401) {
      // Token không hợp lệ, thử refresh
      try {
        await refreshToken();
        // Retry request với token mới
        return await getProfile();
      } catch (refreshError) {
        throw { ...refreshError, message: AUTH_ERRORS.TOKEN_EXPIRED };
      }
    }
    
    throw error;
  }
};

/**
 * Đăng xuất
 * @returns {Promise<void>}
 */
export const logout = async () => {
  try {
    const tokens = await getStoredTokens();
    
    if (tokens?.refreshToken) {
      // Gọi API logout để revoke refresh token
      await fetch(`${API_BASE_URL}${AUTH_ENDPOINTS.LOGOUT}`, {
        method: 'POST',
        headers: await createHeaders(true),
        body: JSON.stringify({ refreshToken: tokens.refreshToken })
      });
    }
  } catch (error) {
    // Ignore errors khi logout, vẫn xóa token local
    console.error('Logout API error (ignored):', error);
  } finally {
    // Luôn xóa tokens local
    await clearTokens();
  }
};

/**
 * Đăng xuất khỏi tất cả thiết bị
 * @returns {Promise<void>}
 */
export const logoutAll = async () => {
  try {
    await fetch(`${API_BASE_URL}${AUTH_ENDPOINTS.LOGOUT_ALL}`, {
      method: 'POST',
      headers: await createHeaders(true)
    });
  } catch (error) {
    console.error('Logout all API error (ignored):', error);
  } finally {
    await clearTokens();
  }
};

/**
 * Kiểm tra xem user có đăng nhập không
 * @returns {Promise<boolean>}
 */
export const isAuthenticated = async () => {
  try {
    const tokens = await getStoredTokens();
    if (!tokens?.accessToken) {
      return false;
    }

    // Kiểm tra token có hết hạn không
    if (tokens.accessTokenExpiry) {
      const expiry = new Date(tokens.accessTokenExpiry);
      if (expiry <= new Date()) {
        // Access token hết hạn, thử refresh
        try {
          await refreshToken();
          return true;
        } catch (error) {
          return false;
        }
      }
    }

    return true;
  } catch (error) {
    console.error('Check authentication error:', error);
    return false;
  }
};

// Export các utility functions
export { getStoredTokens, clearTokens, storeTokens };
