import React, { useState, useEffect, useRef } from 'react';
import { StyleSheet, View, SafeAreaView, StatusBar, ScrollView, FlatList } from 'react-native';

// Import tất cả các component bạn đã tạo
import Header from '../components/Header';
import NextClassCard from '../components/NextClassCard';
import StudentIdCard from '../components/StudentIdCard';
import GpaCard from '../components/GpaCard';
import SectionHeader from '../components/SectionHeader';
import NewsCard from '../components/NewsCard';
import QuickActionButton from '../components/QuickActionButton';

// Dữ liệu mẫu
const newsData = [
  { id: '1', subtitle: "On Campus", title: "Computing Lab Extends Hours for Finals Week" },
  { id: '2', subtitle: "Announcement", title: "New Parking Regulations Effective Next Month" },
  { id: '3', subtitle: "Event", title: "Annual Tech Conference Registration is Now Open" },
];

const quickActionsData = [
    { title: 'Grades', icon: 'award', color: '#FF7A94' },
    { title: 'Timetable', icon: 'calendar', color: '#8A4DFF' },
    { title: 'Tuition', icon: 'dollar-sign', color: '#3BD1B9' },
    { title: 'Events', icon: 'bell', color: '#FFB45B' },
    { title: 'Map', icon: 'map', color: '#4D9FFF' },
    { title: 'Shuttle Bus', icon: 'truck', color: '#FF7A94' },
];

// Component màn hình Home
export default function HomeScreen() { 
  const handlePressStudentId = () => alert('Mở thẻ sinh viên!');
  const handlePressGpaCard = () => alert('Đi đến trang kết quả học tập!');

  const flatListRef = useRef(null);
  const [activeIndex, setActiveIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setActiveIndex(prevIndex => (prevIndex + 1) % newsData.length);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    if (flatListRef.current) {
      flatListRef.current.scrollToIndex({ index: activeIndex, animated: true, viewPosition: 0.5 });
    }
  }, [activeIndex]);

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="light-content" />
      <View style={styles.content}>
        <Header />
        <ScrollView showsVerticalScrollIndicator={false}>
          <NextClassCard />
          <View style={styles.infoCardsContainer}>
            <StudentIdCard onPress={handlePressStudentId} />
            <GpaCard onPressCard={handlePressGpaCard} />
          </View>
          <SectionHeader title="Recent News" />
          <FlatList
             ref={flatListRef}
             data={newsData}
             renderItem={({ item }) => <NewsCard subtitle={item.subtitle} title={item.title} />}
             keyExtractor={item => item.id}
             horizontal={true}
             showsHorizontalScrollIndicator={false}
             pagingEnabled
          />
          <SectionHeader title="Quick Actions" />
          <View style={styles.quickActionsContainer}>
            {quickActionsData.map((item) => (
              <QuickActionButton
                key={item.title}
                title={item.title}
                iconName={item.icon}
                color={item.color}
                onPress={() => alert(`Mở ${item.title}`)}
              />
            ))}
          </View>
        </ScrollView>
      </View>
    </SafeAreaView>
  );
}

// Styles cho màn hình Home
const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#0C0C2D' },
  content: { 
    flex: 1,
    paddingHorizontal: 20, 
    paddingTop: 10,
  },
  infoCardsContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  quickActionsContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
});
