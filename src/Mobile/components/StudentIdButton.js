// file: components/StudentIdCard.js
import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';

const StudentIdButton = ({ onPress }) => {
  return (
    <TouchableOpacity onPress={onPress} style={styles.cardContainer}>
      <Feather name="user" size={32} color="#FFFFFF" />
      <Text style={styles.title}>Thẻ sinh viên</Text>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  cardContainer: {
    backgroundColor: '#1E1E3D',
    borderRadius: 20,
    padding: 18,
    width: '48%',
    marginTop: 20,
    minHeight: 120,
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    color: '#FFFFFF',
    fontWeight: 'bold',
    marginTop: 12,
  },
});

export default StudentIdButton;