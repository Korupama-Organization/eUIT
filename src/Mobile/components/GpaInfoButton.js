import {React, useState} from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';

const GpaInfoButton = ({ onPressCard, onPressIcon }) => {
  const [isGpaVisible, setIsGpaVisible] = useState(false); //Mặc định là điểm đang bị ẩn (false).
  const handleToggleGpaVisibility = () =>
  {
    setIsGpaVisible(!isGpaVisible);// Đảo ngược trạng thái: nếu đang ẩn thì thành hiện, và ngược lại
  }
  return (
    <TouchableOpacity onPress={onPressCard} style={styles.cardContainer}>
      <View style={styles.topRow}>
        <Feather name="award" size={20} color="#A0A0A0" />
        <Text style={styles.title}>ĐTB của tôi</Text>
      </View>
      <Text style={styles.mainText}>
        {isGpaVisible ? '8.00/10' : '****/10'}
      </Text>
      <Text style={styles.subText}>
        {isGpaVisible ? '87 tín chỉ' : '*** tín chỉ'}
      </Text>
      <TouchableOpacity onPress={handleToggleGpaVisibility} style={styles.secondaryIcon}>
        <Feather name={isGpaVisible ? 'eye' : 'eye-off'}
          size={24}
          color="#BEBEBE" />
      </TouchableOpacity>
      <View style={{ position: 'absolute', right: 15, bottom: 22 }}>
        <Feather name="arrow-right" size={22} color="#BEBEBE" />
      </View>
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
  },
  topRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  title: {
    color: '#A0A0A0',
    marginLeft: 8,
    fontWeight: 'bold',
  },
  mainText: {
    color: '#FFFFFF',
    fontSize: 18,
    fontWeight: 'bold',
  },
  subText: {
    color: '#BEBEBE',
    fontSize: 12,
    marginTop: 4,
  },
  secondaryIcon: {
    position: 'absolute',
    right: 15,
    top: 15,
  },
});

export default GpaInfoButton;