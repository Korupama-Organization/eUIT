import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const Header = () => {
  return (
    <View style={styles.headerContainer}>
      <View>
        <Text style={styles.welcomeText}>Welcome back,</Text>
        <Text style={styles.nameText}>Alex Thompson</Text>
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
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingBottom: 20,
    paddingTop: 10,
  },
  welcomeText: {
    color: '#A0A0A0',
    fontSize: 16,
  },
  nameText: {
    color: '#FFFFFF',
    fontSize: 28,
    fontWeight: 'bold',
  },
  iconsContainer: {
    flexDirection: 'row',
  },
  iconPlaceholder: {
    width: 40,
    height: 40,
    backgroundColor: '#333333',
    borderRadius: 20,
    marginLeft: 15,
  },
});

export default Header;
