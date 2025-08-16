import React from "react";
import { View, Text, StyleSheet } from 'react-native';

const SectionHeader = ({title}) => {
    return (
        <View style = {styles.headerContainer}>
            <Text style = {styles.headerText}>{title}</Text>
        </View>
    );
};

const styles = StyleSheet.create({
    headerContainer : {
        marginTop : 10,
        marginBottom : 5, 
    },
    headerText : {
        color: '#FFFFFF',
        fontSize: 22,
        fontWeight: 'bold',
    }
})

export default SectionHeader;