using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ENAEJJMLRSFG.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ENAEJJMLRSFG.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ENAEJJMLRSFGContext _context;

        public UserController(ENAEJJMLRSFGContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(User user)
        {
            var query = _context.Users.AsQueryable();

            // Aplicar filtros si se proporcionan
            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                query = query.Where(s => s.UserName.Contains(user.UserName));
            }

            if (user.Status == 1 || user.Status == 2)
            {
                query = query.Where(s => s.Status == user.Status);
            }

            // Asegurar que siempre haya un valor para Take
            if (user.Take == 0)
                user.Take = 10;

            // Incluir la relación con el rol en la consulta principal
            query = query.Include(u => u.Role).Take(user.Take);

            return View(await query.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Login
        [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        // POST: User/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user, string ReturnUrl)
        {
            user.Password = CalcularHashMD5(user.Password);
            var usuarioAut = await _context.Users.FirstOrDefaultAsync(s => s.Email == user.Email && s.Password == user.Password && s.Status == 1);
            if (usuarioAut?.Id > 0 && usuarioAut.Email == user.Email)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, usuarioAut.Email),
                     //new Claim(ClaimTypes.Role, usuarioAut.Rol),
                    new Claim("Id", usuarioAut.Id.ToString())
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true }); ;
                var result = User.Identity.IsAuthenticated;
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.Error = "Credenciales incorrectas";
            ViewBag.pReturnUrl = ReturnUrl;
            return View(user);

        }
        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Email,Status,RoleId")] User user,IFormFile image)
        {

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    user.Image = memoryStream.ToArray();

                }
            }
            user.Password = CalcularHashMD5(user.Password);
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{

            //}
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            //return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Email,Status,RoleId")] User user,IFormFile image)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    user.Image = memoryStream.ToArray();
                }
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                var producFind = await _context.Users.FirstOrDefaultAsync(s => s.Id == user.Id);
                if (producFind?.Image?.Length > 0)
                    user.Image = producFind.Image;
                producFind.UserName = user.UserName;
                producFind.Password = user.Password;
                producFind.Email = user.Email;
                producFind.Status = user.Status;
                producFind.RoleId = user.RoleId;
                _context.Update(producFind);
                await _context.SaveChangesAsync();
            }
            try
            {
               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


            //if (ModelState.IsValid)
            //{

            //}
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            //return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ENAEJJMLRSFGContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private string CalcularHashMD5(string texto)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Convierte la cadena de texto a bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);

                // Calcula el hash MD5 de los bytes
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convierte el hash a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}