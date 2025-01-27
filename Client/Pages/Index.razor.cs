using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Tripbuk.Client.Services;

namespace Tripbuk.Client.Pages;

public partial class Index
{
    public string Search { get; set; }

    private int _carouselSelectedIndex;
    private RadzenCarousel _carousel;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadPhotos();
    }


    private string Photo { get; set; } = "/images/banner-nature-5.jpg";
    private async Task LoadPhotos()
    {
        var photo = await UnsplashService.GetRandomPhoto();
        Photo = photo.Urls["full"];
        StateHasChanged();
    }
    [Inject]
    protected UnsplashService UnsplashService { get; set; }
}