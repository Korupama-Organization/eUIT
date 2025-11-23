import React, { useState, useEffect, useCallback } from 'react';
import {
    View,
    Text,
    ScrollView,
    StyleSheet,
    ActivityIndicator,
    RefreshControl,
    TouchableOpacity,
    useColorScheme,
} from 'react-native';
import { getQuickGpa, getAcademicResults } from '../api/studentsAPI';

const calculateSemesterGpa = (semesterSubjects) => {
    let totalScore = 0;
    let totalCredits = 0;

    semesterSubjects.forEach(item => {
        const score = item.diemTongKet;
        const credits = item.soTinChi;

        if (score !== null && score !== undefined && parseInt(credits) > 0) {
            totalScore += parseFloat(score) * parseInt(credits);
            totalCredits += parseInt(credits);
        }
    });

    if (totalCredits === 0) {
        return 'N/A';
    }

    const gpa = totalScore / totalCredits;
    return gpa.toFixed(2);
};

const formatSemesterTitle = (semesterKey) => {
    const parts = semesterKey.split('_');
    if (parts.length === 3) {
        const startYear = parts[0];
        const endYear = parts[1];
        const semesterNumber = parts[2];
        return `Học kỳ ${semesterNumber} ${startYear}-${endYear}`;
    }
    return `Học kỳ ${semesterKey}`;
};

const formatComponentScores = (item) => {
    return [
        { label: 'QT', value: item.diemQuaTrinh !== null && item.diemQuaTrinh !== undefined ? parseFloat(item.diemQuaTrinh).toFixed(1) : '--', isLow: item.diemQuaTrinh !== null && item.diemQuaTrinh !== undefined && parseFloat(item.diemQuaTrinh) < 5.0 },
        { label: 'GK', value: item.diemGiuaKi !== null && item.diemGiuaKi !== undefined ? parseFloat(item.diemGiuaKi).toFixed(1) : '--', isLow: item.diemGiuaKi !== null && item.diemGiuaKi !== undefined && parseFloat(item.diemGiuaKi) < 5.0 },
        { label: 'TH', value: item.diemThucHanh !== null && item.diemThucHanh !== undefined ? parseFloat(item.diemThucHanh).toFixed(1) : '--', isLow: item.diemThucHanh !== null && item.diemThucHanh !== undefined && parseFloat(item.diemThucHanh) < 5.0 },
        { label: 'CK', value: item.diemCuoiKi !== null && item.diemCuoiKi !== undefined ? parseFloat(item.diemCuoiKi).toFixed(1) : '--', isLow: item.diemCuoiKi !== null && item.diemCuoiKi !== undefined && parseFloat(item.diemCuoiKi) < 5.0 },
    ];
};

const AcademicResultScreen = ({ navigation }) => {
    const colorScheme = useColorScheme();
    const isDarkMode = colorScheme === 'dark';
    const styles = getStyles(isDarkMode);

    const [academicResults, setAcademicResults] = useState([]);
    const [gpaData, setGpaData] = useState({ gpa: 'N/A', credits: 0 });
    const [isLoading, setIsLoading] = useState(true);
    const [isRefreshing, setIsRefreshing] = useState(false);
    const [isError, setIsError] = useState(false);
    const [expandedSemesters, setExpandedSemesters] = useState([]);

    const loadData = useCallback(async () => {
        setIsError(false);
        try {
            const [results, quickGpa] = await Promise.all([
                getAcademicResults(),
                getQuickGpa(),
            ]);

            setAcademicResults(results || []);

            if (quickGpa && quickGpa.gpa !== undefined && quickGpa.gpa !== null) {
                setGpaData({
                    gpa: quickGpa.gpa,
                    credits: quickGpa.soTinChiTichLuy,
                });
            } else {
                setGpaData({ gpa: 'N/A', credits: 0 });
            }

            if (results && results.length > 0) {
                const uniqueSemesters = [...new Set(results.map(item => item.hocKy))];
                setExpandedSemesters(uniqueSemesters);
            } else {
                setExpandedSemesters([]);
            }
        } catch (error) {
            console.error('LOG Error loading academic results or GPA', error);
            setIsError(true);
        } finally {
            setIsLoading(false);
            setIsRefreshing(false);
        }
    }, []);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const onRefresh = () => {
        setIsRefreshing(true);
        loadData();
    };

    const toggleSemesterExpansion = (semesterKey) => {
        setExpandedSemesters(prevExpanded => {
            if (prevExpanded.includes(semesterKey)) {
                return prevExpanded.filter(key => key !== semesterKey);
            } else {
                return [...prevExpanded, semesterKey];
            }
        });
    };

    const renderLoadingOrError = () => {
        if (isLoading) {
            return (
                <View style={styles.centerContainer}>
                    <ActivityIndicator size="large" color={isDarkMode ? '#7788FF' : '#2f6bff'} />
                    <Text style={styles.statusText}>Đang tải dữ liệu học tập...</Text>
                </View>
            );
        }
        if (isError) {
            return (
                <View style={styles.centerContainer}>
                    <Text style={styles.errorText}>Lỗi kết nối hoặc tải dữ liệu. Vui lòng thử lại.</Text>
                </View>
            );
        }
        if (academicResults.length === 0) {
            return (
                <View style={styles.centerContainer}>
                <Text style={styles.statusText}>Không tìm thấy kết quả học tập nào.</Text>
                </View>
            );
        }
        return null;
    };

    const renderGpaCard = () => {
        const gpaValue = gpaData.gpa === 'N/A' ? 'N/A' : parseFloat(gpaData.gpa).toFixed(2);
        const credits = gpaData.credits;
        const rating = gpaValue === 'N/A' ? '---' : (gpaValue >= 3.6 ? 'Xuất sắc' : (gpaValue >= 3.2 ? 'Giỏi' : 'Khá'));

        return (
            <View style={styles.gpaContainer}>
                <View style={styles.gpaCardV2}>
                    <View>
                        <Text style={styles.gpaLabelV2}>Điểm trung bình chung tích lũy</Text>
                        <View style={styles.gpaDetailsRow}>
                            <Text style={styles.gpaValueV2}>{gpaValue}</Text>
                            <View style={styles.gpaMeta}>
                                <Text style={styles.gpaMetaText}>{credits} tín chỉ</Text>
                                <Text style={styles.gpaRatingText}>{rating}</Text>
                            </View>
                        </View>
                    </View>
                    <Text style={styles.starIcon}>⭐</Text>
                </View>
            </View>
        );
    };

    const renderAcademicList = () => {
        const resultsBySemester = academicResults.reduce((acc, item) => {
            const key = item.hocKy;
            if (!acc[key]) {
                acc[key] = [];
            }
            acc[key].push(item);
            return acc;
        }, {});

        const sortedSemesters = Object.keys(resultsBySemester).sort().reverse();

        return (
            <View style={styles.resultsContainer}>
                {sortedSemesters.map((semesterKey) => {
                    const subjects = resultsBySemester[semesterKey];
                    const semesterGpa = calculateSemesterGpa(subjects);
                    const isExpanded = expandedSemesters.includes(semesterKey);
                    const displayTitle = formatSemesterTitle(semesterKey);

                    return (
                        <View key={semesterKey} style={styles.semesterContainer}>
                            <TouchableOpacity
                                style={styles.semesterHeader}
                                onPress={() => toggleSemesterExpansion(semesterKey)}
                                activeOpacity={0.8}
                            >
                                <Text style={styles.semesterTitle}>{displayTitle}</Text>
                                <View style={styles.gpaAndChevronGroup}>
                                    <View style={styles.gpaPill}>
                                        <Text style={styles.semesterGpa}>TBHK: {semesterGpa}</Text>
                                    </View>
                                    <Text style={[styles.chevron, isExpanded && styles.chevronExpanded]}>▶</Text>
                                </View>
                            </TouchableOpacity>

                            {isExpanded && (
                                <View>
                                    {subjects.map((item, index) => {
                                        const score = item.diemTongKet !== null ? parseFloat(item.diemTongKet) : null;
                                        const isLowScore = score !== null && score < 5.0;
                                        const scoreDisplay = score !== null ? score.toFixed(2) : '---';
                                        const componentScores = formatComponentScores(item);

                                        return (
                                            <View
                                                key={item.maMonHoc + index}
                                                style={[
                                                    styles.subjectItem,
                                                    index === subjects.length - 1 && styles.noBorderBottom,
                                                ]}
                                            >
                                                <View style={styles.subjectContent}>
                                                    <View style={styles.subjectNameRow}>
                                                        <Text style={styles.subjectName}>{item.tenMonHoc}</Text>
                                                        <View
                                                            style={[
                                                                styles.scorePill,
                                                                styles.totalScorePill,
                                                                isLowScore ? styles.lowScorePill : styles.highScorePill,
                                                            ]}
                                                        >
                                                            <Text
                                                                style={[
                                                                    styles.scoreText,
                                                                    isLowScore ? styles.lowScoreText : styles.highScoreText,
                                                                ]}
                                                            >
                                                                {scoreDisplay}
                                                            </Text>
                                                        </View>
                                                    </View>
                                                    <Text style={styles.subjectDetailsText}>
                                                        {item.maMonHoc} - {item.soTinChi} tín chỉ
                                                    </Text>
                                                    <View style={styles.componentScoresContainer}>
                                                        {componentScores.map((score) => (
                                                            <View
                                                                key={score.label}
                                                                style={[
                                                                    styles.scorePill,
                                                                    styles.componentScorePill,
                                                                    score.isLow ? styles.lowScorePill : styles.highScorePill,
                                                                ]}
                                                            >
                                                                <Text
                                                                    style={score.isLow ? styles.lowScoreText : styles.highScoreText}
                                                                >
                                                                    {score.label}: {score.value}
                                                                </Text>
                                                            </View>
                                                        ))}
                                                    </View>
                                                </View>
                                            </View>
                                        );
                                    })}
                                </View>
                            )}
                        </View>
                    );
                })}
            </View>
        );
    };

    return (
        <View style={styles.screenContainer}>
            {renderGpaCard()}
            <ScrollView
                style={styles.scrollView}
                contentContainerStyle={styles.scrollViewContent}
                refreshControl={
                    <RefreshControl
                        refreshing={isRefreshing}
                        onRefresh={onRefresh}
                        tintColor={isDarkMode ? '#7788FF' : '#2f6bff'}
                    />
                }
            >
                {renderLoadingOrError()}
                {!isLoading && !isError && academicResults.length > 0 && renderAcademicList()}
            </ScrollView>
        </View>
    );
};

const getStyles = (isDarkMode) => StyleSheet.create({
    // --- Colors ---
    colorBackgroundLight: '#f5f5f5',
    colorCardLight: '#fff',
    colorTextLight: '#333',
    colorTextSecondaryLight: '#666',
    colorPrimaryLight: '#2f6bff',
    colorGpaPillBackgroundLight: '#fff',
    colorGpaPillBorderLight: '#ccc',
    colorHeaderBackgroundLight: '#e6f0ff',
    colorScorePillBackgroundHighLight: '#e9f7ef',
    colorScoreTextHighLight: '#28a745',
    colorBackgroundDark: '#1F1F47',
    colorCardDark: '#1F1F47',
    colorBorderDark: '#47526A',
    colorTextDark: '#FFFFFF',
    colorTextSecondaryDark: '#A1A1AA',
    colorPrimaryDark: '#7788FF',
    colorGpaPillBackgroundDark: '#3B82F6',
    colorGpaPillBorderDark: '#22C55E',
    colorHeaderBackgroundDark: '#22C55E',
    colorScorePillBackgroundHighDark: '#09092A',
    colorScoreTextHighDark: '#7788FF',
    colorLowScoreBackground: '#ffeded',
    colorLowScoreText: '#dc3545',
    colorLowScoreBackgroundDark: '#7A8F8F',
    colorLowScoreTextDark: '#F6736C',

    // --- General Styles ---
    screenContainer: {
        flex: 1,
        backgroundColor: isDarkMode ? '#1F1F47' : '#f5f5f5',
    },
    scrollView: {
        flex: 1,
    },
    scrollViewContent: {
        paddingBottom: 80,
    },
    centerContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        paddingTop: 50,
        paddingBottom: 50,
    },
    statusText: {
        marginTop: 10,
        color: isDarkMode ? '#A1A1AA' : '#666',
    },
    errorText: {
        marginTop: 10,
        color: isDarkMode ? '#F6736C' : 'red',
        fontSize: 16,
    },
    noBorderBottom: {
        borderBottomWidth: 0,
    },

    // --- GPA Card Styles ---
    gpaContainer: {
        padding: 15,
        backgroundColor: isDarkMode ? '#1F1F47' : '#f5f5f5',
    },
    gpaCardV2: {
        backgroundColor: isDarkMode ? '#3B82F6' : '#e6f0ff',
        padding: 20,
        borderRadius: 12,
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        elevation: 2,
        shadowColor: isDarkMode ? '#7788FF' : '#000',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: isDarkMode ? 0.3 : 0.1,
        shadowRadius: 6,
    },
    gpaLabelV2: {
        color: isDarkMode ? '#FFFFFF' : '#2f6bff',
        fontSize: 15,
        fontWeight: '500',
        marginBottom: 8,
    },
    gpaDetailsRow: {
        flexDirection: 'row',
        alignItems: 'flex-end',
    },
    gpaValueV2: {
        color: isDarkMode ? '#FFFFFF' : '#2f6bff',
        fontSize: 38,
        fontWeight: 'bold',
        marginRight: 15,
        lineHeight: 38,
    },
    gpaMeta: {
        alignSelf: 'flex-end',
        marginBottom: 5,
    },
    gpaMetaText: {
        color: isDarkMode ? '#A1A1AA' : '#666',
        fontSize: 12,
    },
    gpaRatingText: {
        color: isDarkMode ? '#FFFFFF' : '#2f6bff',
        fontSize: 14,
        fontWeight: '600',
    },
    starIcon: {
        fontSize: 24,
    },

    // --- Academic List Styles ---
    resultsContainer: {
        paddingHorizontal: 15,
        paddingTop: 15,
    },
    semesterContainer: {
        marginBottom: 15,
        borderWidth: 1,
        borderColor: isDarkMode ? '#47526A' : '#ddd',
        borderRadius: 8,
        overflow: 'hidden',
        backgroundColor: isDarkMode ? '#282C58' : '#fff',
    },
    semesterHeader: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: 12,
        backgroundColor: isDarkMode ? '#22C55E' : '#e6f0ff',
    },
    gpaAndChevronGroup: {
        flexDirection: 'row',
        alignItems: 'center',
    },
    gpaPill: {
        backgroundColor: isDarkMode ? '#FFFFFF' : '#fff',
        borderRadius: 14,
        paddingVertical: 4,
        paddingHorizontal: 8,
        borderWidth: 1,
        borderColor: isDarkMode ? '#A1A1AA' : '#ccc',
        minWidth: 100,
        justifyContent: 'flex-start',
    },
    chevron: {
        fontSize: 12,
        color: isDarkMode ? '#FFFFFF' : '#2f6bff',
        marginLeft: 10,
        transform: [{ rotate: '0deg' }],
    },
    chevronExpanded: {
        transform: [{ rotate: '90deg' }],
    },
    semesterTitle: {
        fontSize: 16,
        fontWeight: 'bold',
        color: isDarkMode ? '#000000' : '#2f6bff',
    },
    semesterGpa: {
        fontSize: 16,
        fontWeight: 'bold',
        color: isDarkMode ? '#000000' : '#1a1a1a',
        lineHeight: 24,
    },
    subjectItem: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        paddingVertical: 12,
        paddingHorizontal: 15,
        borderBottomWidth: 1,
        borderBottomColor: isDarkMode ? '#47526A' : '#eee',
    },
    subjectContent: {
        flex: 1,
        marginRight: 10,
    },
    subjectNameRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    subjectName: {
        fontSize: 14,
        fontWeight: '600',
        color: isDarkMode ? '#FFFFFF' : '#333',
        flex: 1,
    },
    subjectDetailsText: {
        fontSize: 12,
        color: isDarkMode ? '#A1A1AA' : '#666',
        marginTop: 2,
    },
    componentScoresContainer: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginTop: 4,
    },
    scorePill: {
        paddingVertical: 4,
        paddingHorizontal: 6,
        borderRadius: 10,
        alignItems: 'center',
        justifyContent: 'center',
    },
    totalScorePill: {
        minWidth: 60,
    },
    componentScorePill: {
        flex: 1,
        marginHorizontal: 3,
    },
    highScorePill: {
        backgroundColor: isDarkMode ? '#09092A' : '#e9f7ef',
    },
    highScoreText: {
        color: isDarkMode ? '#7788FF' : '#28a745',
        fontSize: 12,
        fontWeight: '600',
    },
    lowScorePill: {
        backgroundColor: isDarkMode ? '#7A8F8F' : '#ffeded',
    },
    lowScoreText: {
        color: isDarkMode ? '#F6736C' : '#dc3545',
        fontSize: 12,
        fontWeight: '600',
    },
});

export default AcademicResultScreen;