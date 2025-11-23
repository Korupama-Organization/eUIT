# Dataset Expansion Guidelines

## 🎯 Mục tiêu mở rộng
Tạo dataset phong phú để chatbot hiểu và trả lời chính xác theo yêu cầu cụ thể của sinh viên.

## 📋 Checklist mở rộng

### 1. Intent Coverage
- [ ] `greeting`: 10+ variations (xin chào, hello, hi, chào bạn, ...)
- [ ] `next_class`: 50+ samples covering all info_types
  - [ ] Room queries: 15+ samples
  - [ ] Time queries: 15+ samples  
  - [ ] Subject queries: 10+ samples
  - [ ] Teacher queries: 10+ samples
- [ ] `schedule`: 20+ samples (today, tomorrow, specific days)
- [ ] `gpa`: 15+ samples (cumulative, semester)
- [ ] `student_info`: 20+ samples (mssv, name, major, year)
- [ ] `help`: 10+ samples

### 2. Linguistic Diversity
- [ ] Formal language: "Xin cho biết lớp học tiếp theo ở phòng nào?"
- [ ] Informal language: "Mai mình học ở đâu?"
- [ ] Question variations: "Ở phòng nào?", "Phòng học là gì?", "Học ở nhà nào?"
- [ ] Time expressions: "hôm nay", "ngày mai", "sáng mai", "chiều nay"

### 3. Edge Cases
- [ ] Ambiguous questions: "Học ở đâu?" (cần context)
- [ ] Multiple info requests: "Mai học môn gì ở phòng nào?"
- [ ] Negative cases: "Hôm nay không có học"
- [ ] Specific time: "Thứ 2 tuần sau học gì?"

### 4. Response Quality
- [ ] Concise responses (< 10 words for single info)
- [ ] Structured responses for multiple info
- [ ] Consistent formatting
- [ ] Natural Vietnamese

## 📝 Template mẫu

### Single Info Query
```json
{
  "input": "Ngày mai học ở phòng nào?",
  "intent": "next_class", 
  "slots": {
    "date": "tomorrow",
    "info_type": "room"
  },
  "desired_response": "A203"
}
```

### Multiple Info Query
```json
{
  "input": "Mai học môn gì ở phòng nào?",
  "intent": "next_class",
  "slots": {
    "date": "tomorrow", 
    "info_type": ["subject", "room"]
  },
  "desired_response": "Toán rời rạc - A203"
}
```

### Time-specific Query
```json
{
  "input": "Sáng thứ 2 có lớp gì?",
  "intent": "schedule",
  "slots": {
    "day_of_week": "monday",
    "time": "morning"
  },
  "desired_response": "Sáng thứ 2: Thuật toán (7:30-9:00)"
}
```

## 🚀 Expansion Strategy

### Phase 1: Core Patterns (50-100 samples)
- Focus on most common questions
- Cover all basic info_types
- Simple, clear responses

### Phase 2: Variations (100-200 samples)  
- Add linguistic diversity
- Different ways to ask same thing
- Regional language variations

### Phase 3: Edge Cases (50+ samples)
- Complex, ambiguous queries
- Multi-intent requests
- Error handling scenarios

### Phase 4: Domain-specific (50+ samples)
- Course-specific queries
- Semester-specific patterns
- Academic calendar queries

## 📊 Quality Metrics

### Coverage
- Intent distribution should be balanced
- All slot combinations covered
- Response length appropriate

### Diversity
- Unique input phrasings
- Varied vocabulary usage
- Natural conversation flow

### Accuracy
- Responses match intent/slots
- Information is correct
- Format is consistent

## 💡 Tips for labeling

1. **Think like a student**: Use natural language patterns
2. **Be specific**: Clear intent and slot labeling
3. **Keep responses minimal**: Only requested information
4. **Test edge cases**: What happens with ambiguous inputs?
5. **Cultural context**: Vietnamese academic terms and customs