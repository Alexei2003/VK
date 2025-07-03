using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Other
{
    public class ImageTransfer
    {
        private const int DefaultMaxRetries = 3;

        public static async Task<bool> DownloadImageAsync(
            HttpClient httpClient,
            Uri imageUrl,
            string savePath,
            int maxRetries = DefaultMaxRetries,
            int? timeoutSeconds = null,
            CancellationToken cancellationToken = default)
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                    {
                        if (timeoutSeconds.HasValue)
                            timeoutCts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds.Value));

                        return await DownloadAndValidateImage(
                            httpClient,
                            imageUrl,
                            savePath,
                            timeoutCts.Token);
                    }
                }
                catch when (attempt < maxRetries - 1)
                {
                    if (File.Exists(savePath))
                    {
                        File.Delete(savePath);
                    }
                    await Task.Delay(1000 * (attempt + 1), cancellationToken);
                }
            }
            return false;
        }

        public static async Task<string?> UploadImageAsync(
            HttpClient httpClient,
            Uri uploadUrl,
            string imagePath,
            string formDataName = "file",
            int maxRetries = DefaultMaxRetries,
            CancellationToken cancellationToken = default)
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using (var fileStream = File.OpenRead(imagePath))
                    using (var content = new MultipartFormDataContent())
                    {
                        var fileContent = new StreamContent(fileStream);
                        content.Add(fileContent, formDataName, Path.GetFileName(imagePath));

                        var response = await httpClient.PostAsync(
                            uploadUrl,
                            content,
                            cancellationToken);

                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                catch when (attempt < maxRetries - 1)
                {
                    await Task.Delay(1000 * (attempt + 1), cancellationToken);
                }
            }
            return null;
        }

        private static async Task<bool> DownloadAndValidateImage(
            HttpClient httpClient,
            Uri imageUrl,
            string savePath,
            CancellationToken ct)
        {
            using (var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, ct))
            {
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("image/"))
                    throw new InvalidOperationException("URL does not point to an image");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return ValidateDownloadedImage(savePath);
            }
        }

        private static bool ValidateDownloadedImage(string filePath)
        {
            try
            {
                using (var image = Image.Load<Rgb24>(filePath))
                {
                    var y = image.Height - 1;
                    for (var x = 0; x < image.Width; x++)
                    {
                        if (image[x, y] != image[0, y])
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}