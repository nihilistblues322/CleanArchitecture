using Asp.Versioning;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Application.Bookings.CancelBooking;
using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Application.Bookings.RejectBooking;
using Bookify.Application.Bookings.CompleteBooking;
using Bookify.Domain.Bookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookings;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/bookings")]
public class BookingsController(ISender sender) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }

    [Authorize]
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelBookingCommand(id);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error == BookingErrors.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new ConfirmBookingCommand(id);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error == BookingErrors.NotFound
                ? NotFound(result.Error)
                : Conflict(result.Error);
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new RejectBookingCommand(id);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error == BookingErrors.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new CompleteBookingCommand(id);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error == BookingErrors.NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
        }

        return NoContent();
    }
}