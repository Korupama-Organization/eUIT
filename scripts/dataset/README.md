# Chatbot Training Dataset

## 📋 Mục tiêu
Tạo bộ dữ liệu để huấn luyện chatbot trả lời ngắn gọn, chính xác theo yêu cầu cụ thể của sinh viên.

## 📁 Cấu trúc Files

### 1. `intents.json`
Định nghĩa các intent (ý định) và slots (thông tin cần trích xuất)

### 2. `chatbot_train.jsonl` 
Dữ liệu training chính - mỗi dòng là một JSON object

### 3. `chatbot_test.jsonl`
Dữ liệu test để đánh giá chất lượng model

## 🎯 Format JSONL

```json
{
  "input": "Câu hỏi của user",
  "intent": "ý_định_được_nhận_dạng", 
  "slots": {
    "info_type": "loại_thông_tin_cần",
    "date": "thời_gian",
    "time": "buổi_học"
  },
  "desired_response": "Câu trả lời ngắn gọn mong muốn"
}
```

## 🏷️ Nguyên tắc Labeling

### Intent Categories:
- `greeting`: Chào hỏi
- `next_class`: Lớp học tiếp theo
- `schedule`: Lịch học tổng thể
- `gpa`: Điểm GPA
- `student_info`: Thông tin sinh viên
- `help`: Yêu cầu trợ giúp

### Response Guidelines:
- **Ngắn gọn**: Chỉ trả thông tin được hỏi
- **Chính xác**: Dựa trên dữ liệu thực
- **Thân thiện**: Giọng điệu lịch sự

## 📊 Mục tiêu Dataset
- Training: 200-500 samples
- Test: 50-100 samples
- Coverage: Tất cả intent chính
- Diversity: Đa dạng cách diễn đạt

## 🚀 Sử dụng
1. Mở rộng dữ liệu trong `chatbot_train.jsonl`
2. Thêm test cases vào `chatbot_test.jsonl`
3. Tích hợp vào ChatbotService để training/validation