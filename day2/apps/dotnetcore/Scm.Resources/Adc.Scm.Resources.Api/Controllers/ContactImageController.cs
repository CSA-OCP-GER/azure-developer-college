using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Adc.Scm.Resources.Api.Repositories;
using Adc.Scm.Resources.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adc.Scm.Resources.Api.Controllers
{
    [ApiController]
    [Route("contactimages")]
    [Produces("application/json")]
    public class ContactImageController : ControllerBase
    {
        private readonly ImageRepository _repository;
        private readonly StorageQueueService _service;

        public ContactImageController(ImageRepository repository, StorageQueueService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UploadImage([FromForm(Name = "imageupload")]IFormFile file)
        {
            using (var memstream = new MemoryStream())
            {
                await file.CopyToAsync(memstream);

                var name = await _repository.Add(file.FileName, memstream.ToArray());
                await _service.NotifyImageCreated(name);

                return CreatedAtAction(nameof(DownloadImage), new { image = name }, null);
            }            
        }

        [HttpGet("{image}")]
        [Produces("application/octet-stream")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadImage(string image)
        {
            var data = await _repository.Get(image);
            if (null == image)
                return NotFound();

            return File(data, "application/octet-stream");            
        }
    }
}