using Microsoft.AspNetCore.Mvc;
using LIB_ZUGFeRD.Models;
using Microsoft.Extensions.Primitives;
using System.Text;
using LIB_ZUGFeRD.Services;

namespace Web.API.FacturX.Controllers
{
    [ApiController]
    [Route("api/")]
    public class FacturXController : ControllerBase
    {
        FacturXService _service;
        public FacturXController()
        {
            _service = new FacturXService();
        }

        [HttpGet]
        [Route("test/{value}")]
        public string Test(string value)
        {
            return value;
        }

        [HttpPost]
        [Route("tofacturx")]
        public string ConvertToFacturX(FacturXModel value)
        {
            return _service.ConvertDocxToFacturX(value);
        }

        [HttpPost]
        [Route("xmlextract")]
        public string ExtractXMLFromFacturX(string pPdfPath)
        {
            return _service.ExtractXmlFromPdfA(pPdfPath);
        }
    }
}
