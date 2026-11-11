using IronPdf;
using Microsoft.Extensions.Options;

namespace Nexam.Infrastructure;

public interface IPdfGenerator
{
    /// <exception cref="PdfUnavailableException">PDF generation is not set up (e.g. missing IronPdf license).</exception>
    Task<byte[]> FromHtmlAsync(string html);
}

public sealed class PdfSettings
{
    /// <summary>IronPdf license key (or trial key). IronPdf refuses to produce PDFs without one.</summary>
    public string IronPdfLicenseKey { get; set; }
}

public sealed class PdfUnavailableException(string message, Exception inner = null) : Exception(message, inner);

public sealed class IronPdfGenerator : IPdfGenerator
{
    public IronPdfGenerator(IOptions<PdfSettings> options)
    {
        var key = options.Value.IronPdfLicenseKey;
        if (!string.IsNullOrWhiteSpace(key))
            License.LicenseKey = key;
    }

    public Task<byte[]> FromHtmlAsync(string html)
    {
        // Rendering is CPU/IO bound and synchronous in IronPdf; keep it off the request thread.
        return Task.Run(() =>
        {
            try
            {
                var renderer = new ChromePdfRenderer();
                using var pdf = renderer.RenderHtmlAsPdf(html);
                return pdf.BinaryData;
            }
            catch (IronSoftware.Exceptions.LicensingException ex)
            {
                throw new PdfUnavailableException(
                    "IronPdf needs a license key. Set Pdf:IronPdfLicenseKey (user-secrets or environment variable Pdf__IronPdfLicenseKey).", ex);
            }
        });
    }
}
