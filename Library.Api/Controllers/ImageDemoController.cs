using Library.Api.Data;
using Library.Api.Dto;
using Library.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace Library.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageDemoController : ControllerBase
{
    private readonly AppDbContext _context;
    public ImageDemoController(AppDbContext appDbContext)
    {
        _context= appDbContext;
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto model)
    {
        if (model.File ==null)
        {
            return BadRequest();

        }

        var path =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");


        Directory.CreateDirectory(path);


        var filepath = Path.Combine(path, model.File.FileName);

       await using var steram = new FileStream(filepath, FileMode.Create);

       await model.File.CopyToAsync(steram);

        return Ok(filepath);

    }

    [HttpGet("Download")]
    public async Task<IActionResult> Download(string path)
    {
        if (string.IsNullOrEmpty(path))
        {

            return BadRequest();
        }

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(bytes, "application/octet-stream",Path.GetFileName(path));

    }





    [HttpPost("UploadFile2")]
    public async Task<IActionResult> Upload2([FromForm] UploadFileDto data)
    {
        if (data.File ==null)
        {
            return BadRequest();

        }

       using var stream = new MemoryStream();

        await data.File.CopyToAsync(stream);

        var model = new MediaFile
        {
            Id=Guid.NewGuid(),

            FileName=data.File.FileName,

            ContetnType =  data.File.ContentType,

            Data = stream.ToArray()
        };
        _context.MediaFiles.Add(model);
        await _context.SaveChangesAsync();  

        return Ok(model);

    }

    [HttpGet("Download2/{id}")]
    public async Task<IActionResult> Download2(Guid id)
    {
        var model = _context.MediaFiles.FirstOrDefault(x=>x.Id == id);
        

        if (model == null)
        {
            return NotFound();  
        }

        return File(
            model.Data,
            model.ContetnType,
            model.FileName
            );


    }


}

