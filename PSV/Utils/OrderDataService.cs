using PSV.Models.DTOs;
using QRCoder;

namespace PSV.Utils;

public class OrderDataService
{
    private const string DataDir = "wwwroot/images/OrdersData";
    
    public async Task<string> SavePhotos(OrderPost request)
    {
        var ordersDataPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir);
        var orderDirectoryPath = Path.Combine(ordersDataPath, request.OrderNumber);
        Directory.CreateDirectory(orderDirectoryPath);
        foreach (var photo in request.Photos)
        {
            if (photo.Length > 0)
            {
                string filePath = Path.Combine(orderDirectoryPath, photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
            }
        }

        return orderDirectoryPath;
    }
    
    public List<string> GetPhotosFromDirectory(string path)
    {
        string[] files = Directory.GetFiles(path)
            .Where(file => Path.GetFileName(file) != "QRCode.png")
            .ToArray();
        return files.ToList();
    }

    public async Task<string> GenerateQrCode(string orderNumber)
    {
        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(orderNumber, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        string orderDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir, orderNumber);
        string filePath = Path.Combine(orderDirectoryPath, "QRCode.png");
        await File.WriteAllBytesAsync(filePath, qrCode.GetGraphic(20));
        return filePath;
    }
}