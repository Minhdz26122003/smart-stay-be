using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using SmartStay.API.Controllers;

namespace SmartStay.Tests.Listings;

public class ListingControllerTests
{
    [Fact]
    public void GetLandlordListings_HasLandlordRouteAlias()
    {
        var method = typeof(ListingController).GetMethod(nameof(ListingController.GetLandlordListings));

        var httpGetAttributes = method!
            .GetCustomAttributes(typeof(HttpMethodAttribute), inherit: true)
            .Cast<HttpMethodAttribute>()
            .ToList();

        Assert.Contains(httpGetAttributes, attribute => attribute.Template == "landlord");
    }
}
