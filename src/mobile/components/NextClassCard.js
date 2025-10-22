import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { Feather, Ionicons } from '@expo/vector-icons';

const NextClassCard = () => {
  return (
    <LinearGradient
      colors={['#4F80FF', '#8A4DFF']}
      start={{ x: 0, y: 0 }}
      end={{ x: 1, y: 1 }}
      style={styles.gradientBorder}
    >
      <View style={styles.cardContainer}>
        <Ionicons
          name="calendar-outline"
          size={100}
          style={styles.watermarkIcon}
        />
        <View style={styles.topSection}>
            <View>
                <Text style={styles.secondaryText}>IT007.N11.CNCL</Text>
                <Text style={styles.primaryText}>Object-Oriented Programming</Text>
            </View>
            <View style={styles.countdownContainer}>
                <Text style={styles.accentText}>1h 30m</Text>
            </View>
        </View>

        <View style={styles.divider} />

        <View style={styles.bottomSection}>
            <View>
                <Text style={styles.accentText}>9:30 AM - 11:30 AM</Text>
                <View style={styles.iconTextRow}>
                    <Feather name="map-pin" size={16} color="#BEBEBE" style={styles.icon} />
                    <Text style={styles.primaryText}>Room A1.201</Text>
                </View>
            </View>
            <TouchableOpacity style={styles.button}>
                <Text style={styles.buttonText}>View full timetable</Text>
            </TouchableOpacity>
        </View>
      </View>
    </LinearGradient>
  );
};

const styles = StyleSheet.create({
  gradientBorder: {
    borderRadius: 22,
    marginTop: 20,
    padding: 2,
  },
  cardContainer: {
    backgroundColor: '#1E1E3D',
    borderRadius: 20,
    padding: 20,
    overflow: 'hidden',
  },
  watermarkIcon: {
    position: 'absolute',
    right: 15,
    top: 15,
    color: 'rgba(255, 255, 255, 0.05)',
  },
  topSection: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  secondaryText: { color: '#BEBEBE', fontSize: 14, marginBottom: 4 },
  primaryText: { color: '#FFFFFF', fontSize: 18, fontWeight: 'bold' },
  countdownContainer: { backgroundColor: 'rgba(255, 255, 255, 0.1)', borderRadius: 15, paddingHorizontal: 12, paddingVertical: 8 },
  accentText: { color: '#8294FF', fontWeight: 'bold', fontSize: 14 },
  divider: { height: 1, backgroundColor: '#444444', marginVertical: 15 },
  bottomSection: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-end' },
  iconTextRow: { flexDirection: 'row', alignItems: 'center', marginTop: 8 },
  icon: { marginRight: 8 },
  button: { backgroundColor: '#8A4DFF', paddingHorizontal: 15, paddingVertical: 10, borderRadius: 12 },
  buttonText: { color: '#FFFFFF', fontWeight: 'bold' },
});

export default NextClassCard;
