import { StyleSheet } from "react-native";

export const lookupStyles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 16,
    borderBottomWidth: 1,
  },
  headerTitle: {
    fontSize: 28,
    fontWeight: "bold",
    marginBottom: 4,
  },
  headerSubtitle: {
    fontSize: 16,
    lineHeight: 20,
  },
  featuresList: {
    flex: 1,
  },
  featureItem: {
    flexDirection: "row",
    alignItems: "center",
    paddingHorizontal: 20,
    paddingVertical: 16,
    borderBottomWidth: 1,
  },
  lastFeatureItem: {
    borderBottomWidth: 0,
  },
  featureIconContainer: {
    width: 40,
    height: 40,
    borderRadius: 8,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  featureContent: {
    flex: 1,
  },
  featureTitle: {
    fontSize: 16,
    fontWeight: "500",
    marginBottom: 2,
  },
  featureDescription: {
    fontSize: 14,
  },
  featureAction: {
    paddingLeft: 8,
  },
  checkbox: {
    width: 20,
    height: 20,
    borderRadius: 4,
    borderWidth: 2,
    justifyContent: "center",
    alignItems: "center",
  },
  checkboxInner: {
    width: 10,
    height: 10,
    borderRadius: 2,
  },
  quickActions: {
    padding: 20,
    borderTopWidth: 1,
  },
  quickActionsTitle: {
    fontSize: 14,
    fontWeight: "600",
    marginBottom: 12,
    textAlign: "center",
    letterSpacing: 1,
  },
  quickActionsGrid: {
    flexDirection: "row",
    justifyContent: "space-around",
  },
  quickActionButton: {
    alignItems: "center",
    minWidth: 80,
  },
  quickActionIcon: {
    width: 56,
    height: 56,
    borderRadius: 12,
    justifyContent: "center",
    alignItems: "center",
    marginBottom: 8,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 3,
  },
  quickActionText: {
    fontSize: 12,
    fontWeight: "500",
    textAlign: "center",
  },
});
