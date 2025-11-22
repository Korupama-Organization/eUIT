import axios from "axios";
export const getTeachingSchedule = async (giangVienId, month, year) => {
  const res = await axios.get(`/api/Schedule/teaching`, {
    params: { giangVienId, month, year }
  });
  return res.data;
};
