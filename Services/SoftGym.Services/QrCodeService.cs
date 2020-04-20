namespace SoftGym.Services
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    using QRCoder;
    using SoftGym.Services.Contracts;

    public class QrCodeService : IQrCodeService
    {
        private readonly ICloudinaryService cloudinaryService;

        public QrCodeService(ICloudinaryService cloudinaryService)
        {
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<string> GenerateQrCodeAsync(string textValue)
        {
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(@"https://softgym.azurewebsites.net/Administration/Cards/UserCard?userId=" + textValue, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            var image = code.GetGraphic(20);

            Bitmap barcodeBitmap = new Bitmap(image);
            byte[] bitmapArr;
            using (var stream = new MemoryStream())
            {
                barcodeBitmap.Save(stream, ImageFormat.Png);
                bitmapArr = stream.ToArray();
            }

            var result = await this.cloudinaryService.UploudAsync(bitmapArr);
            return result;
        }
    }
}
