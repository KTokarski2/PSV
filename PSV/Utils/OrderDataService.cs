using PSV.Models.DTOs;
using QRCoder;

namespace PSV.Utils;

public class OrderDataService
{
    private const string DataDir = "OrdersData";
    
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
    
    public async Task<List<IFormFile>> GetPhotosFromDirectory(string directoryPath)
    {
        try
        {
            var photoFiles = new List<IFormFile>();

            if (!Directory.Exists(directoryPath))
            {
                return photoFiles;
            }

            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var fileStream = new FileStream(file, FileMode.Open);
                var formFile = new FormFile(fileStream, 0, fileStream.Length, null, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg" 
                };

                photoFiles.Add(formFile);
            }

            return photoFiles;
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Directory not found exception occurred while retrieving photos from directory: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while retrieving photos from directory: {ex.Message}");
            throw; 
        }
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