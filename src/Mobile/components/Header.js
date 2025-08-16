// file: components/Header.js
import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const Header = () => {
  return (
    <View style={styles.headerContainer}>
      <View>
        <Text style={styles.welcomeText}>Chào mừng trở lại,</Text>
        <Text style={styles.nameText}>Cole Palmer</Text>
      </View>
      <View style={styles.iconsContainer}>
        <View style={styles.iconPlaceholder} />
        <View style={styles.iconPlaceholder} />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  headerContainer: {
    flexDirection: 'row', // Sắp xếp các phần tử theo chiều ngang
    justifyContent: 'space-between', // Đẩy các phần tử ra hai phía
    alignItems: 'center', // Căn giữa theo chiều dọc
    paddingBottom: 20,
  },
  welcomeText: {
    color: '#A0A0A0', // Màu xám nhạt
    fontSize: 16,
  },
  nameText: {
    color: '#FFFFFF', // Màu trắng
    fontSize: 28,
    fontWeight: 'bold',
  },
  iconsContainer: {
    flexDirection: 'row',
  },
  iconPlaceholder: {
    width: 40,
    height: 40,
    backgroundColor: '#333333', // Màu giữ chỗ cho icon
    borderRadius: 20,
    marginLeft: 15, // Khoảng cách giữa các icon
  },
});

export default Header;