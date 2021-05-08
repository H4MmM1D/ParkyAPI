using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepository;
        public TrailController(INationalParkRepository npRepo, ITrailRepository trailRepository)
        {
            _npRepo = npRepo;
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(StaticDetail.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));
            var trailViewModel = new TrailViewModel()
            {
                NationalParksList = npList.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                return View(trailViewModel);
            }

            trailViewModel.Trail = await _trailRepository.GetAsync(StaticDetail.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (trailViewModel.Trail == null)
            {
                return NotFound();
            }
            return View(trailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailViewModel trailViewModel)
        {
            if (ModelState.IsValid)
            {
                if (trailViewModel.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(StaticDetail.TrailAPIPath, trailViewModel.Trail, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _trailRepository.UpdateAsync(StaticDetail.TrailAPIPath + trailViewModel.Trail.Id, trailViewModel.Trail, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(StaticDetail.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));
                var trailVM = new TrailViewModel()
                {
                    NationalParksList = npList.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    Trail = trailViewModel.Trail
                };

                return View(trailVM);
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(StaticDetail.TrailAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(StaticDetail.TrailAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
                return Json(new { success = true, message = "Deleted Successfuly" });
            else
                return Json(new { success = false, message = "Something went wrong" });
        }
    }
}

//See Information about HttpGet and HttpPost in this url https://www.completecsharptutorial.com/asp-net-mvc5/asp-net-mvc-5-httpget-and-httppost-method-with-example.php
