using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        public NationalParkController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalPark = new NationalPark();
            if (id == null)
            {
                return View(nationalPark);
            }
            nationalPark = await _npRepo.GetAsync(StaticDetail.NationalParkAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (nationalPark == null)
            {
                return NotFound();
            }
            return View(nationalPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    byte[] p1 = null;
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", files[0].FileName);

                    using (var fileStream = new FileStream(savePath, FileMode.Create)) 
                    {
                        files[0].CopyTo(fileStream);
                    }

                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    nationalPark.Picture = p1;
                }
                else if (nationalPark.Id != 0)
                {
                    var npFromDb = await _npRepo.GetAsync(StaticDetail.NationalParkAPIPath, nationalPark.Id, HttpContext.Session.GetString("JWToken"));
                    nationalPark.Picture = npFromDb.Picture;
                }

                if (nationalPark.Id == 0)
                {
                    await _npRepo.CreateAsync(StaticDetail.NationalParkAPIPath, nationalPark, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _npRepo.UpdateAsync(StaticDetail.NationalParkAPIPath + nationalPark.Id, nationalPark, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _npRepo.GetAllAsync(StaticDetail.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(StaticDetail.NationalParkAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
                return Json(new { success = true, message = "Deleted Successfuly" });
            else
                return Json(new { success = false, message = "Something went wrong" });
        }
    }
}

//See Information about HttpGet and HttpPost in this url https://www.completecsharptutorial.com/asp-net-mvc5/asp-net-mvc-5-httpget-and-httppost-method-with-example.php
