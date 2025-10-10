import React from 'react';
import { View, TouchableOpacity, StyleSheet } from 'react-native';
import { Feather } from '@expo/vector-icons';

const TabBarCustomButton = ({ onPress }) => (
  <TouchableOpacity
    style={styles.container}
    onPress={onPress}
    activeOpacity={0.9}
  >
    <View style={styles.button}>
      <Feather name="home" size={28} color="#FFFFFF" />
    </View>
  </TouchableOpacity>
);

const styles = StyleSheet.create({
  container: {
    // Đẩy nút lên trên
    top: -30,
    justifyContent: 'center',
    alignItems: 'center',
  },
  button: {
    width: 70,
    height: 70,
    borderRadius: 35,
    backgroundColor: '#8A4DFF',
    justifyContent: 'center',
    alignItems: 'center',
    // Tạo hiệu ứng đổ bóng
    shadowColor: '#8A4DFF',
    shadowOffset: {
      width: 0,
      height: 10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.5,
    elevation: 5,
  },
});

export default TabBarCustomButton;