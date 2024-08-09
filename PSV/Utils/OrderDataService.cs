using System.Runtime.CompilerServices;
using BarcodeStandard;
using PSV.Models.DTOs;
using QRCoder;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using Type = System.Type;

namespace PSV.Utils;

public class OrderDataService
{
    private const string DataDir = "wwwroot/images/OrdersData";
    private const string TempDir = "wwwroot/temp";
    private const string OrdersFiles = "wwwroot/ordersFiles";

    public async Task<string> SavePhotos(OrderPost request, int id)
    {
        var ordersDataPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir);
        var orderDirectoryPath = Path.Combine(ordersDataPath, id.ToString());
        Directory.CreateDirectory(orderDirectoryPath);

        if (request.Photos != null)
        {
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
            
        }
        return orderDirectoryPath;
    }

    public async Task SaveTemporaryFile(IFormFile? file)
    {
        if (file != null)
        {
            string filePath = Path.Combine(TempDir, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }

    public async Task<string?> GetTemporaryFile(string orderNumber)
    {
        string[] files = Directory.GetFiles(TempDir, $"{orderNumber}.*");

        if (files.Length == 0)
        {
            return null;
        }

        string tempFilePath = files[0];

        string destinationFilePath = Path.Combine(OrdersFiles, Path.GetFileName(tempFilePath));

        using (var sourceStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
        using (var destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
        {
            await sourceStream.CopyToAsync(destinationStream);
        }

        await Task.Run(() => File.Delete(tempFilePath));

        return destinationFilePath;
    }
    
    public List<string> GetPhotosFromDirectory(string path)
    {
        string[] files = Directory.GetFiles(path)
            .Where(file => Path.GetFileName(file) != "QRCode.png")
            .Where(file => Path.GetFileName(file) != "BarCode.png")
            .Select(file =>
            {
                string relativePath = file.Substring(file.IndexOf("/images"));
                return $"~{relativePath}"; 
            })
            .ToArray();
        return files.ToList();
    }

    public async Task<string> GenerateQrCode(int id)
    {
        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(id.ToString(), QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        string orderDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir, id.ToString());
        string filePath = Path.Combine(orderDirectoryPath, "QRCode.png");
        await File.WriteAllBytesAsync(filePath, qrCode.GetGraphic(20));
        return filePath;
    }

    public async Task<string> GenerateBarcode(int id, string orderNumber)
    {
        string filePath;
        var barcode = new Barcode();
        barcode.IncludeLabel = true;
        var img = barcode.Encode(BarcodeStandard.Type.Code128, orderNumber, SKColors.Black, SKColors.White, 290, 120);
        using var image = SKImage.FromBitmap(SKBitmap.FromImage(img));
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        string orderDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir, id.ToString());
        filePath = Path.Combine(orderDirectoryPath, "BarCode.png");
        await File.WriteAllBytesAsync(filePath, data.ToArray());

        return filePath;
    }

    public void DeleteOrderDirectory(int id)
    {
        string orderDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir, id.ToString());
        Directory.Delete(orderDirectoryPath, true);
    }

    public string ExtractOrderNumber(IFormFile file)
    {
        return file.FileName.Split(".")[0];
    }
}