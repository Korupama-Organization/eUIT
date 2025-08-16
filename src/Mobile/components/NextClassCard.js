import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient'; 
import { Feather, Ionicons} from '@expo/vector-icons'


const NextClassCard = () => {
    const handleFullTimetableButton = () => {
        alert('Xem thoi khoa bieu');
    };
    return (
        <LinearGradient
            colors={['#4F80FF', '#8A4DFF']}
            start={{ x: 0, y: 0 }}
            end={{ x: 1, y: 1 }}
            style={styles.gradientBorder}
        >
            <View style={styles.cardContainer}>
                <Ionicons
                    name='calendar-outline'
                    size={100}
                    style = {styles.watermarkIcon}
                />
                <View style={styles.topSection}>
                    <View style={styles.leftSection}>
                        <Text style={styles.timeText}>7h30 - 10h45 (Tiết 1 - 4)</Text>
                        <Text style={styles.classText}>JAN07.Q11.CNVN</Text>
                        <View style={{ flexDirection: 'row', alignItems: 'center', marginBottom: 4 }}>
                            <Feather name="map-pin" size={16} color="#BEBEBE" style={styles.icon} />
                            <Text style={styles.locationText}>E11.2</Text>
                        </View>
                        <View style={{ flexDirection: 'row', alignItems: 'center', marginBottom: 4 }}>
                            <Feather name="user" size={16} color="#BEBEBE" style={styles.icon} />
                            <Text style={styles.instructorText}>Kaoru Mitoma</Text>
                        </View>
                    </View>
                    <View style={styles.rightSection}>
                        <Text style={styles.startsInLabel}>Bắt đầu trong</Text>
                        <Text style={styles.timeRemainingText}>2h 15m</Text>
                    </View>
                </View>
                <TouchableOpacity onPress={handleFullTimetableButton} style={styles.timetableButton}>
                    <Text style={styles.timetableButtonText}>Xem toàn bộ thời khóa biểu</Text>
                    <Text style={styles.arrow}>→</Text>
                </TouchableOpacity>
            </View>
        </LinearGradient>
    );
};

const styles = StyleSheet.create({
    gradientBorder: {
        borderRadius: 20,
        marginTop: 20,
        padding: 2, // Độ dày của đường viền gradient
        // Thêm shadow cho hiệu ứng blur
        shadowColor: '#4F80FF',
        shadowOffset: {
            width: 0,
            height: 4,
        },
        shadowOpacity: 0.3,
        shadowRadius: 8,
        elevation: 8, // Cho Android
    },
    watermarkIcon :
    {
        position: 'absolute', // Cho phép định vị tự do
        right: 15,
        top: 15,
        color: 'rgba(255, 255, 255, 0.05)', // Màu trắng rất mờ
    },
    titleText : {
        color: '#FFFFFF',
        fontWeight: 'bold',
        fontSize: 24,
        marginBottom: 8,
        lineHeight: 24,    
    },
    cardContainer: {
        backgroundColor: '#1e1e3d', // Màu nền tối giống trong hình
        borderRadius: 18,
        padding: 20,
        // Thêm shadow nhẹ cho card bên trong
        shadowColor: '#000000',
        shadowOffset: {
            width: 0,
            height: 2,
        },
        shadowOpacity: 0.25,
        shadowRadius: 3.84,
        elevation: 5, // Cho Android
        overflow : 'hidden',
    },
    topSection: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        marginBottom: 20,
    },
    leftSection: {
        flex: 1,
    },
    rightSection: {
        alignItems: 'flex-end',
    },
    timeText: {
        color: '#4FC3F7', // Màu xanh nhạt cho thời gian
        fontWeight: '600',
        fontSize: 14,
        marginBottom: 8,
    },
    classText: {
        color: '#FFFFFF',
        fontWeight: 'bold',
        fontSize: 20,
        marginBottom: 8,
        lineHeight: 24,
    },
    locationText: {
        color: '#A0A0A0', // Màu xám cho địa điểm
        fontSize: 14,
        marginBottom: 4,
    },
    instructorText: {
        color: '#A0A0A0', // Màu xám cho tên giảng viên
        fontSize: 14,
    },
    startsInLabel: {
        color: '#A0A0A0',
        fontSize: 12,
        marginBottom: 4,
    },
    timeRemainingText: {
        color: '#4FC3F7', // Màu xanh nhạt giống thời gian
        fontWeight: 'bold',
        fontSize: 18,
    },
    timetableButton: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingTop: 15,
        borderTopWidth: 1,
        borderTopColor: '#333366', // Đường phân cách nhạt
    },
    timetableButtonText: {
        color: '#4FC3F7', // Màu xanh cho button text
        fontSize: 14,
        fontWeight: '500',
    },
    arrow: {
        color: '#4FC3F7',
        fontSize: 16,
        fontWeight: 'bold',
    },
    icon : {
        marginRight: 8,
    }
});

export default NextClassCard;
