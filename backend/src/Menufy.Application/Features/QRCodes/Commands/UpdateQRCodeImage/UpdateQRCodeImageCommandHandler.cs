using MediatR;
using Menufy.Application.Common.Interfaces;
using Menufy.Application.Common.Models;
using Menufy.Application.Features.QRCodes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Menufy.Application.Features.QRCodes.Commands.UpdateQRCodeImage;

public class UpdateQRCodeImageCommandHandler : IRequestHandler<UpdateQRCodeImageCommand, Result<QRCodeDto>>
{
    private readonly IApplicationDbContext _context;

    public UpdateQRCodeImageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<QRCodeDto>> Handle(UpdateQRCodeImageCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _context.Restaurants
            .Include(r => r.QRCode)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (restaurant == null)
        {
            return Result<QRCodeDto>.FailureResult("Restaurant not found");
        }

        // Update existing QR code or create new one
        if (restaurant.QRCode != null)
        {
            // Update existing QR code
            restaurant.QRCode.ImageUrl = request.ImageData;
            restaurant.QRCode.CreatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync(cancellationToken);

            var dto = new QRCodeDto
            {
                Id = restaurant.QRCode.Id,
                ImageUrl = restaurant.QRCode.ImageUrl,
                Link = restaurant.QRCode.Link,
                RestaurantId = restaurant.QRCode.RestaurantId
            };

            return Result<QRCodeDto>.SuccessResult(dto, "QR Code updated successfully");
        }
        else
        {
            // Create new QR code
            var baseUrl = request.ImageData.Contains("localhost") ? "http://localhost:3000" : "https://yourdomain.com";
            var menuLink = $"{baseUrl}/menu/{restaurant.Slug}";

            var qrCode = new Domain.Entities.QRCode
            {
                Id = Guid.NewGuid(),
                ImageUrl = request.ImageData,
                Link = menuLink,
                RestaurantId = request.RestaurantId,
                CreatedAt = DateTime.UtcNow
            };

            _context.QRCodes.Add(qrCode);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = new QRCodeDto
            {
                Id = qrCode.Id,
                ImageUrl = qrCode.ImageUrl,
                Link = qrCode.Link,
                RestaurantId = qrCode.RestaurantId
            };

            return Result<QRCodeDto>.SuccessResult(dto, "QR Code created successfully");
        }
    }
}

