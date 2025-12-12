using MediatR;
using Menufy.Application.Common.Models;
using Menufy.Application.Features.QRCodes.DTOs;

namespace Menufy.Application.Features.QRCodes.Commands.UpdateQRCodeImage;

public record UpdateQRCodeImageCommand(Guid RestaurantId, string ImageData) : IRequest<Result<QRCodeDto>>;

