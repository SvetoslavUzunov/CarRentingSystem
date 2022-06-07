namespace CarRentingSystem.Services.Dealers;

public interface IDealerService
{
    bool IsDealer(string userId);

    int IdByUser(string userId);
}
