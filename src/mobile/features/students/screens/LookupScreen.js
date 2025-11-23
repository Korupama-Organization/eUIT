import React from 'react';
import { 
    View, 
    Text, 
    StyleSheet, 
    SafeAreaView, 
    TouchableOpacity, 
    ScrollView, 
    Image, 
    useColorScheme 
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
// Đã loại bỏ các import của react-native-vector-icons để thay thế bằng Text/Unicode

// ********************************************************************************
// LƯU Ý: Vấn đề hiển thị 'X' cho icon HÌNH ẢNH là do ĐƯỜNG DẪN SAI. 
// VUI LÒNG KIỂM TRA LẠI CẤU TRÚC THƯ MỤC.
// ********************************************************************************

// Icons cho Light Mode
import LightCalendarIcon from '../../students/assets/light-icons/calendar-dots.png';
import LightBookIcon from '../../students/assets/light-icons/book.png';
import LightLeafIcon from '../../students/assets/light-icons/leaf.png';
import LightGradHatIcon from '../../students/assets/light-icons/gradhat.png';
import LightCreditCardIcon from '../../students/assets/light-icons/credit-card.png';
import LightScrollIcon from '../../students/assets/light-icons/scroll.png';
import LightBlueprintIcon from '../../students/assets/light-icons/blueprint.png'; 

// Icons cho Dark Mode
import DarkCalendarIcon from '../../students/assets/dark-icons/calendar.png';
import DarkBookIcon from '../../students/assets/dark-icons/book.png';
import DarkLeafIcon from '../../students/assets/dark-icons/leaf.png';
import DarkGradHatIcon from '../../students/assets/dark-icons/gradhat.png';
import DarkCreditCardIcon from '../../students/assets/dark-icons/credit-card.png';
import DarkScrollIcon from '../../students/assets/dark-icons/scroll.png';
import DarkBlueprintIcon from '../../students/assets/dark-icons/blueprint.png'; 


// --- 2. Hằng số và Cấu hình Dữ liệu ---

const lookupItemsConfig = [
    {
        id: '1',
        title: 'Xem kế hoạch năm',
        subtitle: 'Phòng Công tác Sinh viên',
        lightIcon: LightCalendarIcon,
        darkIcon: DarkCalendarIcon,
        destination: 'PlanScreen', 
    },
    {
        id: '2',
        title: 'Tra cứu kết quả học tập',
        subtitle: 'Phòng Dữ liệu & Công nghệ thông tin',
        lightIcon: LightBookIcon,
        darkIcon: DarkBookIcon,
        destination: 'AcademicResult', 
    },
    {
        id: '3',
        title: 'Tra cứu điểm rèn luyện',
        subtitle: 'Phòng Đào tạo Đại học / VPCCTĐB',
        lightIcon: LightLeafIcon,
        darkIcon: DarkLeafIcon,
        destination: 'TrainingScoreScreen',
    },
    {
        id: '4',
        title: 'Theo dõi tiến độ đào tạo',
        subtitle: 'Phòng Đào tạo Đại học / VPCCTĐB',
        lightIcon: LightGradHatIcon,
        darkIcon: DarkGradHatIcon,
        destination: 'TrainingProgressScreen',
    },
    {
        id: '5',
        title: 'Kiểm tra học phí',
        subtitle: 'Phòng Đào tạo Đại học / VPCCTĐB',
        lightIcon: LightCreditCardIcon,
        darkIcon: DarkCreditCardIcon,
        destination: 'TuitionFeeScreen',
    },
    {
        id: '6',
        title: 'Xem quy chế đào tạo',
        subtitle: 'Phòng Đào tạo Đại học / VPCCTĐB',
        lightIcon: LightScrollIcon,
        darkIcon: DarkScrollIcon,
        destination: 'RegulationsScreen',
    },
    {
        id: '7',
        title: 'Xem chương trình đào tạo',
        subtitle: 'Phòng Đào tạo Đại học / VPCCTĐB',
        lightIcon: LightBlueprintIcon,
        darkIcon: DarkBlueprintIcon,
        destination: 'CurriculumScreen',
    },
];

// --- 3. Component Cho Mỗi Mục Tra Cứu ---
const LookupItem = ({ item, isDark }) => {
    const navigation = useNavigation();

    const handlePress = () => {
        if (item.destination) {
            navigation.navigate(item.destination); 
        }
    };

    const iconSource = isDark ? item.darkIcon : item.lightIcon;
    const itemStyles = getStyles(isDark);

    return (
        <TouchableOpacity style={itemStyles.itemContainer} onPress={handlePress}>
            <View style={itemStyles.iconWrapper}>
                <Image 
                    source={iconSource} 
                    style={itemStyles.itemIcon}
                    resizeMode="contain" 
                />
            </View>
            
            <View style={itemStyles.textContainer}>
                <Text style={itemStyles.title}>{item.title}</Text>
                <Text style={itemStyles.subtitle}>{item.subtitle}</Text>
            </View>
            
            {/* Mũi tên Điều hướng Chính (Unicode: ›) */}
            <Text style={[itemStyles.chevronArrowText, { color: itemStyles.colorSubText }]}>
                ›
            </Text>
        </TouchableOpacity>
    );
};


// --- 4. Màn Hình Chính ---
const LookupScreen = () => {
    const colorScheme = useColorScheme();
    const isDark = colorScheme === 'dark';
    
    const currentStyles = getStyles(isDark);

    return (
        <SafeAreaView style={currentStyles.safeArea}>
            <View style={currentStyles.header}>
                <Text style={currentStyles.mainTitle}>Tra cứu</Text>
                <Text style={currentStyles.description}>
                    Cung cấp các tiện ích tra cứu trực tuyến cho sinh viên
                </Text>
            </View>

            <ScrollView contentContainerStyle={currentStyles.listContainer}>
                {lookupItemsConfig.map((item) => (
                    <LookupItem key={item.id} item={item} isDark={isDark} />
                ))}
            </ScrollView>
        </SafeAreaView>
    );
};


// --- 5. Stylesheet (ĐÃ CẬP NHẬT paddingBottom) ---
const getStyles = (isDark) => {
    const COLORS = {
        DARK_TEXT: isDark ? '#FFFFFF' : '#171736',
        SUB_TEXT: isDark ? '#A1A1AA' : '#5A5A6B',
        BACKGROUND: isDark ? '#000000' : '#D8E6FF',
        CARD_BACKGROUND: isDark ? '#1C1C1E' : '#FFFFFF',
        ACCENT: isDark ? '#BB86FC' : '#2F6BFF', 
    };

    return StyleSheet.create({
        safeArea: {
            flex: 1,
            backgroundColor: COLORS.BACKGROUND,
        },
        header: {
            paddingHorizontal: 16,
            paddingTop: 10,
            paddingBottom: 20,
            backgroundColor: COLORS.CARD_BACKGROUND,
            borderBottomLeftRadius: 10,
            borderBottomRightRadius: 10,
        },
        mainTitle: {
            fontSize: 28,
            fontWeight: 'bold',
            color: COLORS.DARK_TEXT,
            marginBottom: 4,
        },
        description: {
            fontSize: 14,
            color: COLORS.SUB_TEXT,
        },
        listContainer: {
            paddingTop: 10,
            paddingHorizontal: 16,
            // ĐÃ THAY ĐỔI: Tăng paddingBottom để cuộn lên cao hơn
            paddingBottom: 80, 
        },
        itemContainer: {
            flexDirection: 'row',
            alignItems: 'center',
            backgroundColor: COLORS.CARD_BACKGROUND,
            paddingVertical: 18,
            paddingHorizontal: 16,
            borderRadius: 12,
            marginBottom: 10,
            shadowColor: isDark ? '#000' : '#000',
            shadowOffset: { width: 0, height: 1 },
            shadowOpacity: isDark ? 0.3 : 0.05,
            shadowRadius: 2,
            elevation: 1,
        },
        iconWrapper: {
            width: 36,
            height: 36,
            justifyContent: 'center',
            alignItems: 'center',
            marginRight: 15,
        },
        itemIcon: {
            width: 24, 
            height: 24,
        },
        textContainer: {
            flex: 1,
            marginRight: 10,
        },
        title: {
            fontSize: 16,
            fontWeight: '600',
            color: COLORS.DARK_TEXT,
        },
        subtitle: {
            fontSize: 12,
            color: COLORS.SUB_TEXT,
            marginTop: 2,
        },
        // Style cho ký tự chevron điều hướng (Unicode: ›)
        chevronArrowText: {
            fontSize: 30, 
            fontWeight: '300', 
            lineHeight: 30,
        },
        colorSubText: COLORS.SUB_TEXT,
        colorAccent: COLORS.ACCENT, 
    });
};

<<<<<<< HEAD
export default LookupScreen;
=======
export default LookupScreen;
>>>>>>> backup-main
