using eUIT.API.Services;

namespace eUIT.API.Services;

/// <summary>
/// Background service để dọn dẹp các Refresh Token hết hạn định kỳ
/// Chạy mỗi 24 giờ để xóa các token không còn sử dụng
/// </summary>
public class TokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TokenCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24); // Chạy mỗi 24 giờ

    public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Token cleanup service started");

        // Delay khởi động ngẫu nhiên để tránh tất cả instance chạy cùng lúc
        var initialDelay = TimeSpan.FromMinutes(Random.Shared.Next(1, 60));
        await Task.Delay(initialDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredTokens();
                
                // Chờ đến lần cleanup tiếp theo
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Service bị dừng, không cần log error
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token cleanup");
                
                // Chờ một chút trước khi thử lại
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("Token cleanup service stopped");
    }

    private async Task CleanupExpiredTokens()
    {
        using var scope = _serviceProvider.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var deletedCount = await tokenService.CleanupExpiredTokensAsync();
        
        if (deletedCount > 0)
        {
            _logger.LogInformation("Cleaned up {DeletedCount} expired tokens", deletedCount);
        }
        else
        {
            _logger.LogDebug("No expired tokens to cleanup");
        }
    }
}