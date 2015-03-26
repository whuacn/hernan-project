using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    public class PersonaController : ApiController
    {
        Persona[] personas = new Persona[] 
        { 
            new Persona { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
            new Persona { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new Persona { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<Persona> GetAllPersonas()
        {
            return personas;
        }

        public IHttpActionResult GetPersona(int id)
        {
            var persona = personas.FirstOrDefault((p) => p.Id == id);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }
    }
}
