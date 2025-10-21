using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eUIT.API.DTOs;
using eUIT.API.Services;
using System.Security.Claims;

namespace eUIT.API.Controllers;

/// <summary>
/// Chatbot API - Đơn giản và dễ sử dụng
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IChatbotService _chatbotService;
    private readonly ILogger<ChatbotController> _logger;

    public ChatbotController(IChatbotService chatbotService, ILogger<ChatbotController> logger)
    {
        _chatbotService = chatbotService;
        _logger = logger;
    }

    /// <summary>
    /// Hỏi chatbot - API chính duy nhất cần thiết
    /// </summary>
    /// <param name="message">Câu hỏi của bạn (ví dụ: "lớp học tiếp theo là gì?")</param>
    /// <returns>Phản hồi từ chatbot</returns>
    [HttpPost("ask")]
    public async Task<ActionResult<string>> Ask([FromBody] string message)
    {
        try
        {
            // Validate message
            if (string.IsNullOrWhiteSpace(message))
            {
                return Ok("Vui lòng nhập câu hỏi.");
            }

            if (message.Length > 1000)
            {
                return Ok("Câu hỏi quá dài. Vui lòng rút gọn dưới 1000 ký tự.");
            }

            // Lấy thông tin sinh viên từ JWT token
            var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedInMssv == null)
            {
                return Ok("Vui lòng đăng nhập để sử dụng chatbot.");
            }

            if (!int.TryParse(loggedInMssv, out int mssvInt))
            {
                return Ok("Thông tin đăng nhập không hợp lệ.");
            }

            _logger.LogInformation("Student {Mssv} asks: {Message}", mssvInt, message);

            // Tạo request DTO
            var request = new ChatbotRequestDTO
            {
                Message = message.Trim(),
                ConversationId = Guid.NewGuid().ToString()
            };

            // Xử lý tin nhắn thông qua ChatbotService
            var response = await _chatbotService.ProcessMessageAsync(request, mssvInt);

            _logger.LogInformation("Response sent to student {Mssv}", mssvInt);

            // Trả về phản hồi dạng string đơn giản
            return Ok(response.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request: {Message}", ex.Message);
            return Ok("Xin lỗi, tôi gặp sự cố. Vui lòng thử lại sau.");
        }
    }

    /// <summary>
    /// Lấy gợi ý câu hỏi cho sinh viên
    /// </summary>
    /// <returns>Danh sách câu hỏi gợi ý</returns>
    [HttpGet("suggestions")]
    public ActionResult<IEnumerable<string>> GetSuggestions()
    {
        var suggestions = new List<string>
        {
            "Buổi học tiếp theo của tôi là khi nào?",
            "GPA hiện tại của tôi là bao nhiêu?",
            "Kết quả học tập của tôi như thế nào?",
            "Thông tin thẻ sinh viên của tôi",
            "Hôm nay tôi có học gì không?",
            "Điểm trung bình tích lũy",
            "Xem bảng điểm",
            "Tôi cần hỗ trợ gì?",
            "Quy định về thi cử",
            "Cách đăng ký môn học"
        };

        return Ok(suggestions);
    }
}