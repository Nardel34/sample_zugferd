using LIB_ZUGFeRD.Models;
using Newtonsoft.Json;
using System.Text;
using TXTextControl;
using TXTextControl.DocumentServer;

namespace LIB_ZUGFeRD.Services
{
    public class FacturXService
    {
        public string ConvertDocxToFacturX(FacturXModel pFacturXmodel)
        {
            try
            {
                using (ServerTextControl _tx = new ServerTextControl())
                {
                    MailMerge mm = new MailMerge();

                    if (!string.IsNullOrEmpty(pFacturXmodel.DocxPath) &&
                        !string.IsNullOrEmpty(pFacturXmodel.PdfPath))
                    {
                        _tx.Create();
                        mm.TextComponent = _tx;

                        _tx.Load(@"D:\invoice.docx"/*pFacturXmodel.DocxPath*/, StreamType.WordprocessingML);

                        FacturXModel invoice = pFacturXmodel;

                        mm.MergeJsonData(JsonConvert.SerializeObject(invoice));

                        string xmlZugferd = invoice.CreateXml();

                        string metaData = File.ReadAllText($@"[PATH]\Metadata\zugferd-metadata.xml");

                        SaveSettings saveSettings = new SaveSettings();

                        var zugferdInvoice = new EmbeddedFile(
                            @"factur-x.xml",
                            Encoding.UTF8.GetBytes(xmlZugferd),
                            metaData);

                        zugferdInvoice.Description = "factur-x";
                        zugferdInvoice.Relationship = "Alternative";
                        zugferdInvoice.MIMEType = "text/xml";
                        zugferdInvoice.LastModificationDate = DateTime.Now;

                        saveSettings.EmbeddedFiles = new EmbeddedFile[] { zugferdInvoice };

                        _tx.Save(@"D:\invoice.pdf"/*pFacturXmodel.PdfPath*/, StreamType.AdobePDFA, saveSettings);

                        return "OK";
                    }
                    else
                    {
                        return "INVALID FILE(S) PATH";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ExtractXmlFromPdfA(string pPdfPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(pPdfPath))
                {
                    using (ServerTextControl _tx = new ServerTextControl())
                    {
                        _tx.Create();

                        LoadSettings ls = new LoadSettings()
                        {
                            PDFImportSettings = PDFImportSettings.LoadEmbeddedFiles
                        };

                        _tx.Load(pPdfPath, StreamType.AdobePDF, ls);

                        var embeddedFiles = ls.EmbeddedFiles;

                        foreach (EmbeddedFile embeddedFile in embeddedFiles)
                        {

                            if (embeddedFile.Relationship == "Alternative" &&
                                embeddedFile.MIMEType == "text/xml")
                            {
                                using (StreamWriter writer = new StreamWriter(
                                    pPdfPath.Replace(".pdf", ".xml")))
                                {
                                    writer.Write(Encoding.UTF8.GetString((byte[])embeddedFile.Data));
                                    return "OK";
                                }
                            }

                        }
                        return "NO XML EMBEDDED";
                    }
                }
                else
                {
                    return "INVALID FILE PATH";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
