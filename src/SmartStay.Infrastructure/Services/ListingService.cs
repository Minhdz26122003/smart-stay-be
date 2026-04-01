using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Listing;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class ListingService(
    IRepository<Listing> listingRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IListingService
{
    public async Task<ApiResponse<ListingDto>> CreateListingAsync(Guid landlordId, CreateListingRequest request)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId)
            ?? throw new NotFoundException(nameof(Room), request.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to create a listing for this room.");

        var listing = mapper.Map<Listing>(request);
        listing.IsActive = true;

        await listingRepository.AddAsync(listing);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ListingDto>(listing);
        return ApiResponse<ListingDto>.Ok(dto, "Listing created successfully.");
    }

    public async Task<ApiResponse<IEnumerable<ListingDto>>> GetListingsByLandlordAsync(Guid landlordId)
    {
        var properties = await propertyRepository.FindAsync(p => p.LandlordId == landlordId);
        var propertyIds = properties.Select(p => p.Id).ToList();

        var rooms = await roomRepository.FindAsync(r => propertyIds.Contains(r.PropertyId));
        var roomIds = rooms.Select(r => r.Id).ToList();

        var listings = await listingRepository.FindAsync(l => roomIds.Contains(l.RoomId));
        var dtos = mapper.Map<IEnumerable<ListingDto>>(listings.OrderByDescending(l => l.CreatedAt));
        return ApiResponse<IEnumerable<ListingDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ListingDto>> ToggleListingActiveAsync(Guid landlordId, Guid listingId)
    {
        var listing = await listingRepository.GetByIdAsync(listingId)
            ?? throw new NotFoundException(nameof(Listing), listingId);

        var room = await roomRepository.GetByIdAsync(listing.RoomId)
            ?? throw new NotFoundException(nameof(Room), listing.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to modify this listing.");

        listing.IsActive = !listing.IsActive;
        listingRepository.Update(listing);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ListingDto>(listing);
        return ApiResponse<ListingDto>.Ok(dto, $"Listing is now {(listing.IsActive ? "active" : "inactive")}.");
    }

    public async Task<ApiResponse<bool>> DeleteListingAsync(Guid landlordId, Guid listingId)
    {
        var listing = await listingRepository.GetByIdAsync(listingId)
            ?? throw new NotFoundException(nameof(Listing), listingId);

        var room = await roomRepository.GetByIdAsync(listing.RoomId)
            ?? throw new NotFoundException(nameof(Room), listing.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this listing.");

        await listingRepository.SoftDeleteAsync(listingId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Listing deleted successfully.");
    }
}
