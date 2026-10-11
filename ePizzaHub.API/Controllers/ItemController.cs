using ePizzaHub.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    public ItemController(IItemService itemService) => _itemService = itemService;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _itemService.GetItemsAsync());
}