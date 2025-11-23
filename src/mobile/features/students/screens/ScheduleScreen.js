import React, { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import { 
    View, 
    Text, 
    StyleSheet, 
    FlatList, 
    ActivityIndicator, 
    TouchableOpacity,
    SafeAreaView,
    useColorScheme,
    Image
} from 'react-native';

// --- IMPORT ASSETS ---
import DarkBellIcon from '../assets/dark-icons/bell.png'; 
import LightBellIcon from '../assets/light-icons/bell.png';
import DarkCalendarIcon from '../assets/dark-icons/calendar.png';
import LightCalendarIcon from '../assets/light-icons/calendar-dots.png';

// --- IMPORT API ---
import { getAcademicSchedule } from '../api/studentsAPI';

// ======================================================
// 🎨 THEME & STYLING
// ======================================================

const DarkThemeColors = {
    BACKGROUND_MAIN: '#09092A',
    TEXT_PRIMARY: '#FFFFFF',
    TEXT_SECONDARY: '#A1A1AA',
    ACCENT_BLUE: '#7AF8FF', 
    SEGMENT_ACTIVE_BG: '#47526A',
    SEGMENT_INACTIVE_BG: '#2A445E',
    CARD_BG: '#10103E',
    LOCATION_TEXT: '#7AF8FF',
    ACCENT_DARK_TEXT: '#09092A',
    CALENDAR_BACKGROUND: '#09092A',
    SEGMENT_TEXT_INACTIVE: '#7AF8FF',
};

const LightThemeColors = {
    BACKGROUND_MAIN: '#FFFFFF',
    TEXT_PRIMARY: '#0032AF',
    TEXT_SECONDARY: '#666666',
    ACCENT_BLUE: '#6390ff',
    SEGMENT_ACTIVE_BG: '#0032AF',
    SEGMENT_INACTIVE_BG: '#FFFFFF',
    SEGMENT_TEXT_INACTIVE: '#0032AF',
    CARD_BG: '#FFFFFF',
    LOCATION_TEXT: '#0032AF',
    ACCENT_DARK_TEXT: '#FFFFFF',
    CALENDAR_BACKGROUND: '#D6E9FF',
};

const useThemeColors = () => {
    const colorScheme = useColorScheme(); 
    return colorScheme === 'dark' ? DarkThemeColors : LightThemeColors;
};

// --- CLASS TIMES ---
const CLASS_TIMES = {
    1: { start: '07:30', end: '08:15' }, 2: { start: '08:15', end: '09:00' }, 
    3: { start: '09:00', end: '09:45' }, 4: { start: '10:00', end: '10:45' }, 
    5: { start: '10:45', end: '11:30' }, 6: { start: '13:00', end: '13:45' },
    7: { start: '13:45', end: '14:30' }, 8: { start: '14:30', end: '15:15' },
    9: { start: '15:30', end: '16:15' }, 0: { start: '16:15', end: '17:00' },
};

const formatClassTime = (startTiet, endTiet) => {
    const startTime = CLASS_TIMES[startTiet]?.start;
    const endTime = CLASS_TIMES[endTiet]?.end;
    if (startTime && endTime) return `${startTime}`;
    return '';
};

// --- HELPERS ---
const isSameDate = (date1, date2) => {
    if (!(date1 instanceof Date) || !(date2 instanceof Date)) return false;
    return date1.getFullYear() === date2.getFullYear() &&
           date1.getMonth() === date2.getMonth() &&
           date1.getDate() === date2.getDate();
};

// ======================================================
// 💅 STYLES
// ======================================================

const createBaseStyles = (COLORS) => StyleSheet.create({
    container: { flex: 1, backgroundColor: COLORS.BACKGROUND_MAIN },
    loading: { marginTop: 50 },
    listContent: { padding: 10 },
    emptyContainer: { flex: 1, justifyContent: 'center', alignItems: 'center', marginTop: 100 },
    emptyText: { marginTop: 15, fontSize: 16, color: COLORS.TEXT_SECONDARY },
});

const createHeaderStyles = (COLORS) => StyleSheet.create({
    calendarStrip: { height: 47, backgroundColor: COLORS.CALENDAR_BACKGROUND },
    header: { paddingHorizontal: 15, paddingVertical: 10, backgroundColor: COLORS.BACKGROUND_MAIN },
    titleBar: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 10 },
    headerTitle: { fontSize: 26, fontWeight: 'bold', color: COLORS.TEXT_PRIMARY },
    headerSubtitle: { fontSize: 16, color: COLORS.TEXT_SECONDARY, marginBottom: 5 },
    dateItem: { alignItems: 'center', paddingVertical: 5, paddingHorizontal: 8 },
    dateItemSelected: { backgroundColor: COLORS.ACCENT_BLUE, padding: 5, borderRadius: 50 },
    dateDay: { fontSize: 12, color: COLORS.TEXT_SECONDARY, marginBottom: 3, fontWeight: '600' },
    dateDaySelected: { color: COLORS.BACKGROUND_MAIN, fontWeight: 'bold' },
    dateNumber: { fontSize: 18, fontWeight: 'bold', color: COLORS.TEXT_PRIMARY },
    dateNumberSelected: { color: COLORS.BACKGROUND_MAIN },
    bellIconStyle: { width: 24, height: 24 },
});

const createScheduleCardStyles = (COLORS) => StyleSheet.create({
    card: { 
        backgroundColor: COLORS.CARD_BG, 
        borderRadius: 10, 
        marginBottom: 15, 
        padding: 15,
        overflow: 'hidden',
        shadowColor: "#000",
        shadowOffset: { width: 0, height: 1 },
        shadowOpacity: 0.1,
        shadowRadius: 2,
        elevation: 1,
        borderLeftWidth: 5, 
        borderLeftColor: COLORS.ACCENT_BLUE, 
    },
    timeText: { fontSize: 15, fontWeight: 'bold', color: COLORS.TEXT_PRIMARY },
    subjectText: { fontSize: 16, fontWeight: 'bold', color: COLORS.TEXT_PRIMARY, marginTop: 5 },
    locationContainer: { 
        position: 'absolute', 
        top: 0, 
        right: 0, 
        backgroundColor: COLORS.ACCENT_BLUE, 
        paddingHorizontal: 10, 
        paddingVertical: 5, 
        borderBottomLeftRadius: 10 
    },
    locationText: { fontSize: 14, color: COLORS.ACCENT_DARK_TEXT, fontWeight: 'bold' },
});

// ======================================================
// 💳 COMPONENTS
// ======================================================

const ScheduleCard = React.memo(({ item, COLORS }) => {
    const cardStyles = createScheduleCardStyles(COLORS);
    return (
        <View style={cardStyles.card}>
            <Text style={cardStyles.timeText}>{item.ThoiGianThuc}</Text>
            {/* Tên môn học */}
            <Text style={cardStyles.subjectText}>{item.tenLop}</Text>
            {/* Mã môn học */}
            <Text style={[cardStyles.subjectText, { fontSize: 14, fontWeight: '500', color: COLORS.TEXT_SECONDARY }]}>
                {item.maLop}
            </Text>
            <View style={cardStyles.locationContainer}>
                <Text style={cardStyles.locationText}>{item.phongHoc}</Text>
            </View>
        </View>
    );
});


const SegmentedControl = ({ selected, onSelect, COLORS }) => {
    return (
        <View style={{
            flexDirection: 'row',
            marginHorizontal: 15,
            borderWidth: 1,
            borderColor: COLORS.ACCENT_BLUE,
            borderRadius: 10,
            overflow: 'hidden',
            marginBottom: 10,
        }}>
            {['Lên lớp', 'Kiểm tra', 'Cá nhân'].map((segment) => (
                <TouchableOpacity
                    key={segment}
                    style={{
                        flex: 1,
                        paddingVertical: 10,
                        backgroundColor: selected === segment ? COLORS.SEGMENT_ACTIVE_BG : COLORS.SEGMENT_INACTIVE_BG,
                        alignItems: 'center',
                    }}
                    onPress={() => onSelect(segment)}
                >
                    <Text style={{
                        color: selected === segment ? COLORS.ACCENT_DARK_TEXT : COLORS.SEGMENT_TEXT_INACTIVE,
                        fontWeight: '600',
                        fontSize: 15,
                    }}>
                        {segment}
                    </Text>
                </TouchableOpacity>
            ))}
        </View>
    );
};

const DateSelector = ({ selectedDate, onSelectDate, COLORS, displayedMonth, dateListRef }) => {
    const headerStyles = createHeaderStyles(COLORS);
    const ITEM_WIDTH = 50; // chiều rộng của mỗi item ngày
    const daysInMonth = useMemo(() => {
        const days = [];
        const year = displayedMonth.getFullYear();
        const month = displayedMonth.getMonth();
        const numDays = new Date(year, month + 1, 0).getDate();
        for (let i = 1; i <= numDays; i++) {
            days.push(new Date(year, month, i));
        }
        return days;
    }, [displayedMonth]);

    useEffect(() => {
        const index = daysInMonth.findIndex(d => isSameDate(d, selectedDate));
        if (index >= 0 && dateListRef.current) {
            dateListRef.current.scrollToIndex({ index, animated: true, viewPosition: 0.5 });
        }
    }, [daysInMonth, selectedDate]);

    return (
        <FlatList
    ref={dateListRef}
    horizontal
    showsHorizontalScrollIndicator={false}
    data={daysInMonth}
    keyExtractor={(item) => item.getTime().toString()}
    getItemLayout={(data, index) => ({
        length: ITEM_WIDTH + 8, // width + margin
        offset: (ITEM_WIDTH + 8) * index,
        index,
    })}
    onScrollToIndexFailed={(info) => {
        // Scroll lại sau 50ms nếu bị lỗi
        setTimeout(() => {
            dateListRef.current?.scrollToIndex({ index: info.index, animated: true, viewPosition: 0.5 });
        }, 50);
    }}
    renderItem={({ item }) => {
        const isSelected = isSameDate(selectedDate, item);
        const dayLabel = item.toLocaleDateString('vi-VN', { weekday: 'narrow' }).toUpperCase();
        return (
            <TouchableOpacity
                style={[headerStyles.dateItem, { width: ITEM_WIDTH, marginHorizontal: 4 }]}
                onPress={() => onSelectDate(item)}
            >
                <Text style={[headerStyles.dateDay, isSelected && headerStyles.dateDaySelected]}>{dayLabel}</Text>
                <View style={isSelected ? headerStyles.dateItemSelected : {}}>
                    <Text style={[headerStyles.dateNumber, isSelected && headerStyles.dateNumberSelected]}>
                        {item.getDate()}
                    </Text>
                </View>
            </TouchableOpacity>
        );
    }}
/>

    );
};


// ======================================================
// 🖥️ MAIN SCREEN
// ======================================================

const ScheduleScreen = () => {
    const COLORS = useThemeColors();
    const colorScheme = useColorScheme();
    const baseStyles = createBaseStyles(COLORS);
    const headerStyles = createHeaderStyles(COLORS);
    const bellIconSource = colorScheme === 'dark' ? DarkBellIcon : LightBellIcon;
    const calendarIconSource = colorScheme === 'dark' ? DarkCalendarIcon : LightCalendarIcon;

    const currentDay = new Date();
    currentDay.setHours(0,0,0,0);

    const [scheduleData, setScheduleData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedDate, setSelectedDate] = useState(currentDay);
    const [scheduleType, setScheduleType] = useState('Lên lớp');
    const [currentSemester] = useState('2025_2026_1');
    const [displayedMonth, setDisplayedMonth] = useState(currentDay);

    const dateListRef = useRef(null);

    const fetchSchedule = useCallback(async () => {
        setLoading(true);
        try {
            const data = await getAcademicSchedule(currentSemester);
            const processedData = (data || []).map(item => ({
                ...item,
                ngayHoc: new Date(item.ngayHoc),
                ThoiGianThuc: formatClassTime(item.tietBatDau, item.tietKetThuc),
            }));
            setScheduleData(processedData);
        } catch (error) {
            console.error('Lỗi khi lấy lịch học:', error);
        } finally {
            setLoading(false);
        }
    }, [currentSemester]);

    useEffect(() => { fetchSchedule(); }, [fetchSchedule]);

    const filteredScheduleList = useMemo(() => {
        if (!scheduleData.length) return [];
        if (scheduleType !== 'Lên lớp') return [];
        return scheduleData
            .filter(item => isSameDate(item.ngayHoc, selectedDate))
            .sort((a, b) => a.tietBatDau - b.tietBatDau);
    }, [scheduleData, selectedDate, scheduleType]);

    const displayMonthYear = displayedMonth.toLocaleDateString('vi-VN', { month: 'long', year: 'numeric' });

    const goToPreviousMonth = () => {
        const prevMonth = new Date(displayedMonth.getFullYear(), displayedMonth.getMonth() - 1, 1);
        setDisplayedMonth(prevMonth);
        setSelectedDate(prevMonth);
    };

    const goToNextMonth = () => {
        const nextMonth = new Date(displayedMonth.getFullYear(), displayedMonth.getMonth() + 1, 1);
        setDisplayedMonth(nextMonth);
        setSelectedDate(nextMonth);
    };

    return (
        <SafeAreaView style={baseStyles.container}>
            <View style={headerStyles.calendarStrip} /> 

            <View style={headerStyles.header}>
                <View style={headerStyles.titleBar}>
                    <Text style={headerStyles.headerTitle}>Lịch</Text>
                    <TouchableOpacity>
                        <Image source={bellIconSource} style={headerStyles.bellIconStyle} resizeMode="contain" />
                    </TouchableOpacity>
                </View>

                {/* THÁNG NĂM + NÚT < > */}
                <View style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginBottom: 5 }}>
                    <TouchableOpacity onPress={goToPreviousMonth}>
                        <Text style={{ fontSize: 20, color: COLORS.TEXT_PRIMARY }}>{'<'}</Text>
                    </TouchableOpacity>
                    <Text style={headerStyles.headerSubtitle}>{displayMonthYear}</Text>
                    <TouchableOpacity onPress={goToNextMonth}>
                        <Text style={{ fontSize: 20, color: COLORS.TEXT_PRIMARY }}>{'>'}</Text>
                    </TouchableOpacity>
                </View>

                <DateSelector
                    selectedDate={selectedDate}
                    onSelectDate={setSelectedDate}
                    COLORS={COLORS}
                    displayedMonth={displayedMonth}
                    dateListRef={dateListRef}
                />
            </View>

            <SegmentedControl selected={scheduleType} onSelect={setScheduleType} COLORS={COLORS} />

            {loading ? (
                <ActivityIndicator style={baseStyles.loading} size="large" color={COLORS.ACCENT_BLUE} />
            ) : filteredScheduleList.length > 0 ? (
                <FlatList
                    data={filteredScheduleList}
                    keyExtractor={(item) => `${item.maLop}_${item.ngayHoc.getTime()}_${item.tietBatDau}`}
                    renderItem={({ item }) => <ScheduleCard item={item} COLORS={COLORS} />}
                    contentContainerStyle={baseStyles.listContent}
                />
            ) : (
                <View style={baseStyles.emptyContainer}>
                    <Image source={calendarIconSource} style={{ width: 50, height: 50 }} resizeMode="contain" />
                    <Text style={baseStyles.emptyText}>
                        {scheduleType === 'Lên lớp' ? 'Không có lịch lên lớp vào ngày này.' : 'Hiện tại chưa có dữ liệu cho loại lịch này.'}
                    </Text>
                </View>
            )}
        </SafeAreaView>
    );
};

export default ScheduleScreen;
