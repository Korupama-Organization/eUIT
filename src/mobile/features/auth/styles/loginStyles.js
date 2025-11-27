import { StyleSheet } from "react-native";

// ================= STYLES =================
export const loginStyles = StyleSheet.create({
  container: {
    flex: 1,
  },

  scrollContainer: {
    flexGrow: 1,
    justifyContent: "flex-start",
    alignItems: "center",
    paddingTop: 120,
    paddingBottom: 40,
  },

  logoTopContainer: {
    alignItems: "center",
    marginBottom: 20,
  },
  logoTop: {
    width: 120,
    height: 120,
  },
  card: {
    width: "90%",
    maxWidth: 420,
    borderRadius: 18,
    padding: 28,
    elevation: 6,
    shadowOpacity: 0.3,
    shadowOffset: { width: 0, height: 4 },
    shadowRadius: 12,
  },
  headerContainer: {
    position: "absolute",
    top: 50,
    right: 25,
    zIndex: 20,
  },
  toggleBtn: {
    padding: 6,
  },
  roleToggleContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    borderRadius: 10,
    padding: 3,
    marginBottom: 24,
  },
  roleButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 6,
    alignItems: "center",
    marginHorizontal: 2,
  },
  footer: {
    alignItems: "center",
    marginTop: 24,
  },
  footerText: {
    fontSize: 14,
    textAlign: "center",
  },
  footerLink: {
    fontWeight: "500",
  },
});
