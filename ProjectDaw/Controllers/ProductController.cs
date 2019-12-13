﻿using Microsoft.AspNet.Identity;
using ProjectDaw.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectDaw.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();        // GET: Product
                                                                             //GET
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Index()
        {
            var products = db.Products.Include("Category").Include("User");
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Products = products;

            return View();
        }


       [Authorize(Roles = "User,Editor,Administrator")]

        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View(product);

        }
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            Product product = new Product();

            product.Categories = GetAllCategories();
            //Preluam Id-ul utilizatorului curent
            product.UserId = User.Identity.GetUserId();

            return View(product);
        }


        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Product product)
        {
            product.Categories = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["message"] = "Produsul a fost adaugat!";

                    return RedirectToAction("Index");
                } 
                else
                {
                    return View(product);
                }
            }
            catch (Exception e)
            {
                return View(product);
            }
        }
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            ViewBag.Product = product;
            product.Categories = GetAllCategories();

            if (product.UserId == User.Identity.GetUserId() ||
               User.IsInRole("Administrator"))
            {
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, Product requestProduct)
        {
            requestProduct.Categories = GetAllCategories();

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);
                    if (product.UserId == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(product))
                        {
                            product.Title = requestProduct.Title;
                            product.Description = requestProduct.Description;
                            product.ImagePath = requestProduct.ImagePath;
                            product.Price = requestProduct.Price;
                            product.Rating = requestProduct.Rating;
                            product.Review = requestProduct.Review;
                            product.CategoryId = requestProduct.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Produsul a fost modificat!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return View(requestProduct);
                }

            }
            catch (Exception e)
            {
                return View(requestProduct);
            }
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }
        [HttpDelete]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }
    }
}