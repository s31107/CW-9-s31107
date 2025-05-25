using CW_9_s31107.DTOs;
using CW_9_s31107.Exceptions;
using CW_9_s31107.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_9_s31107.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionsController(IDbService service) : ControllerBase
{
    [HttpPost("addPrescription")]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionPostDto prescription)
    {
        try
        {
            return Ok(await service.CreatePrescription(prescription));
        }
        catch (NotFoundException exc)
        {
            return NotFound(exc.Message);
        }
        catch (Exception exc) when (exc is PrescriptionDateException or ExceededNumberOfMedicaments)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpGet("getPatient/{id:int}")]
    public async Task<IActionResult> GetPatient([FromRoute] int id)
    {
        try
        {
            return Ok(await service.GetPatient(id));
        }
        catch (NotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }
}