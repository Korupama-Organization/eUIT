import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';

const GpaCard = ({ onPressCard }) => {
  const [isGpaVisible, setIsGpaVisible] = useState(false);

  const handleToggleVisibility = () => {
    setIsGpaVisible(!isGpaVisible);
  };

  return (
    <TouchableOpacity onPress={onPressCard} style={styles.cardContainer} activeOpacity={0.8}>
      <View style={styles.topRow}>
        <Feather name="award" size={20} color="#A0A0A0" />
        <Text style={styles.title}>My GPA</Text>
      </View>
      <Text style={styles.mainText}>
        {isGpaVisible ? '3.58 / 4.0' : '**** / 4.0'}
      </Text>
      <Text style={styles.subText}>
        {isGpaVisible ? '92 Credits' : '*** Credits'}
      </Text>
      <TouchableOpacity onPress={handleToggleVisibility} style={styles.secondaryIcon}>
        <Feather 
          name={isGpaVisible ? 'eye-off' : 'eye'} 
          size={24} 
          color="#BEBEBE" 
        />
      </TouchableOpacity>
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
  topRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 12 },
  title: { color: '#A0A0A0', marginLeft: 8, fontWeight: 'bold' },
  mainText: { color: '#FFFFFF', fontSize: 18, fontWeight: 'bold' },
  subText: { color: '#BEBEBE', fontSize: 12, marginTop: 4 },
  secondaryIcon: { position: 'absolute', right: 15, bottom: 15 },
});

export default GpaCard;
