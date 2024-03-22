using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ENAEJJMLRSFG.Models;
using Microsoft.AspNetCore.Authorization;

namespace ENAEJJMLRSFG.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ENAEJJMLRSFGContext _context;

        public CustomersController(ENAEJJMLRSFGContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'ENAEJJMLRSFGContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                //este es para que se muestre en la vista detalles
                .Include(s => s.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var customer = new Customer();

            customer.Addresses = new List<Address>();
            customer.Addresses.Add(new Address
            {

            });
            ViewBag.Accion = "Create";
            return View(customer);
            //var customer = new Customer();

            //customer.Addresses = new List<Address>();
            //customer.Addresses.Add(new Address
            //{

            //});
            //ViewBag.Accion = "Create";
            //return View(customer);

        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Phone,Addresses")] Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           //return View(customer);
        }

        //metodo agregar detalles

        [HttpPost]
        public IActionResult AgregarDetalles([Bind("Id,FirstName,LastName,Email,Phone,Addresses")] Customer customer, string accion)
        {
            customer.Addresses.Add(new Address { Street = "" });
            ViewBag.Accion = accion;
            //esto de aca abajo es fundamental (Accion), para editar
            return View(accion, customer); // Redirecciona a la vista Create después de agregar los detalles
        }

        //Metodo Eliminar detalles
        public IActionResult EliminarDetalles([Bind("Id,FirstName,LastName,Email,Phone,Addresses")] Customer customer, int index, string accion)
        {
            {
                var det = customer.Addresses[index];
                if (accion == "Edit" && det.Id > 0)
                {
                    det.Id = det.Id * -1;
                }
                else
                {
                    customer.Addresses.RemoveAt(index);
                }

                ViewBag.Accion = accion;
                return View(accion, customer);
            }
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                //este es para que se muestre en la vista detalles
                .Include(s => s.Addresses)
                .FirstAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,Addresses")] Customer customer)
        {
            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.Customers
                        .Include(s => s.Addresses)
                        .FirstAsync(s => s.Id == customer.Id);
                facturaUpdate.FirstName = customer.FirstName;
                facturaUpdate.LastName = customer.LastName; /*.Where(s => s.Id > -1).Sum(s => s.PrecioUnitario * s.Cantidad);*/
                facturaUpdate.Email = customer.Email;
                facturaUpdate.Phone = customer.Phone;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = customer.Addresses.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.Addresses.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = customer.Addresses.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.Addresses.FirstOrDefault(s => s.Id == d.Id);
                    det.Street = d.Street;
                    det.City = d.City;
                    det.State = d.State;
                    det.Country = d.Country;
                    det.PostalCode = d.PostalCode;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = customer.Addresses.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.Addresses.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(facturaUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
             
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                //este es para que se muestre en la vista detalles
                .Include(s => s.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Accion = "Delete";
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ENAEJJMLRSFGContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
