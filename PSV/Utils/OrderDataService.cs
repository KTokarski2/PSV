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
            .Select(file =>
            {
                string relativePath = file.Substring(file.IndexOf("/images"));
                return $"~{relativePath}"; 
            })
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

    public async Task<string> GenerateBarcode(string orderNumber)
    {
        string filePath;
        var barcode = new Barcode();
        barcode.IncludeLabel = true;
        var img = barcode.Encode(BarcodeStandard.Type.Code128, orderNumber, SKColors.Black, SKColors.White, 290, 120);


        using var image = SKImage.FromBitmap(SKBitmap.FromImage(img));
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        string orderDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DataDir, orderNumber);
        filePath = Path.Combine(orderDirectoryPath, "BarCode.png");
        await File.WriteAllBytesAsync(filePath, data.ToArray());

        return filePath;
    }
}