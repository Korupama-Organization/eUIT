import React from 'react';
import { StyleSheet, View, SafeAreaView, StatusBar, ScrollView, Text } from 'react-native';
import Feather from '@expo/vector-icons';
import Header from './components/Header'; 
import NextClassCard from './components/NextClassCard';
import StudentIdButton from './components/StudentIdButton';
import GpaInfoButton from './components/GpaInfoButton';
import SectionHeader from './components/SectionHeader';

export default function App() {
  const handlePressStudentIDButton = () => {
    alert('Mở thẻ sinh viên!');
  };
  const handlePressGpaInfoButton = () =>{
    alert('Di den trang ket qua hoc tap');
  };
  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="light-content" />
      <ScrollView>
        <View style={styles.content}>
          <Header /> 
          <SectionHeader title={'Lịch trình tiếp theo'} />
          <NextClassCard/>
          <View style={styles.infoCardsContainer}>
            <StudentIdButton onPress={handlePressStudentIDButton} />
            <GpaInfoButton 
              onPressCard={handlePressGpaInfoButton}
            />
          </View>
          <SectionHeader title={'Thông báo mới'} />          
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#0C0C2D',
  },
  infoCardsContainer : {
    flexDirection : 'row',
    justifyContent : 'space-between',
    marginBottom : 15,
  },
  content: {
    padding: 20, // Khoảng cách từ nội dung đến viền màn hình
    paddingTop: 30,
  },
});