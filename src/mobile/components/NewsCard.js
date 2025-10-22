import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';

const NewsCard = ({ title, subtitle }) => {
  return (
    <TouchableOpacity style={styles.cardContainer}>
      <View style={styles.textContainer}>
        <Text style={styles.subtitle}>{subtitle}</Text>
        <Text style={styles.title} numberOfLines={2}>
          {title}
        </Text>
      </View>
      <Feather name="chevron-right" size={28} color="#FFFFFF" />
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  cardContainer: {
    backgroundColor: '#1E1E3D',
    borderRadius: 20,
    padding: 20,
    marginRight: 15,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: 280,
    height: 100,
  },
  textContainer: {
    flex: 1,
    marginRight: 10,
  },
  title: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: 'bold',
  },
  subtitle: {
    color: '#BEBEBE',
    fontSize: 12,
    marginBottom: 4,
  },
});

export default NewsCard;
