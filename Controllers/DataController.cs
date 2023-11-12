using ASE_Election_Portal_G20.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASE_Election_Portal_G20.Controllers
{
    public class DataController : Controller
    {
        private readonly ElectionPortalG20Context _context;

        public DataController(ElectionPortalG20Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetCountiesByState(int stateId)
        {
            var counties = _context.Counties.Where(c => c.StateId == stateId).ToList();

            return Json(counties);
        }
        [HttpGet]
        public IActionResult GetPositionsByType(int typeId)
        {
            var positions = _context.Positions.Where(p => p.ElectionTypeId == typeId).ToList();
            return Json(positions);
        }
    }
}
