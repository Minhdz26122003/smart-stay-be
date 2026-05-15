using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Listing;

namespace SmartStay.Application.Interfaces;

public interface IListingService
{
    Task<ApiResponse<ListingDto>> CreateListingAsync(Guid landlordId, CreateListingRequest request);
    Task<ApiResponse<IEnumerable<ListingDto>>> GetListingsByLandlordAsync(Guid landlordId, Guid? propertyId = null);
    Task<ApiResponse<ListingDto>> ToggleListingActiveAsync(Guid landlordId, Guid listingId);
    Task<ApiResponse<bool>> DeleteListingAsync(Guid landlordId, Guid listingId);
}
