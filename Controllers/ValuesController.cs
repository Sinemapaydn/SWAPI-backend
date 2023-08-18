using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars.Entities;
using StarWars;


namespace StarWars.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private Core core;
        public ValuesController()
        {
            core = new Core();
        }

        [HttpGet()]
        [Route("GetFilm/{id}")]
        public Film GetFilm(int id)
        {
            return Core.GetSingle<Film>("films/" + id);
        }


        [HttpGet()]
        [Route("GetPeople/{id}")]
        public People GetPeople(int id)
        {
            return Core.GetSingle<People>($"people/{id}/");
        }


        [HttpGet()]
        [Route("GetPlanet/{id}")]
        public Planet GetPlanet(int id)
        {
            return Core.GetSingle<Planet>("planets/" + id);
        }

        [HttpGet()]
        [Route("GetSpecie/{id}")]
        public Species GetSpecie(int id)
        {
            return Core.GetSingle<Species>("species/" + id);
        }

        [HttpGet()]
        [Route("GetStarship/{id}")]
        public Starship GetStarship(int id)
        {
            return Core.GetSingle<Starship>("starships/" + id);
        }

        [HttpGet()]
        [Route("GetVehicle/{id}")]
        public Vehicle GetVehicle(int id)
        {
            return Core.GetSingle<Vehicle>("vehicles/" + id);
        }

        [HttpGet()]
        [Route("GetAllFilms/")]
        public EntityResults<Film> GetAllFilms(string pageNumber = "1")
        {
            EntityResults<Film> result = core.GetAllPaginated<Film>("/films/", pageNumber);

            return result;
        }

        [HttpGet()]
        [Route("GetAllPeople/")]
        public EntityResults<People> GetAllPeople(string pageNumber = "1")
        {
            EntityResults<People> result = core.GetAllPaginated<People>("/people/", pageNumber);

            return result;
        }

        [HttpGet()]
        [Route("GetAllPlanets/")]
        public EntityResults<Planet> GetAllPlanets(string pageNumber = "1")
        {
            EntityResults<Planet> result = core.GetAllPaginated<Planet>("/planets/", pageNumber); 

            return result;
        }

        [HttpGet()]
        [Route("GetAllSpecies/")]
        public EntityResults<Species> GetAllSpecies(string pageNumber = "1")
        {
            EntityResults<Species> result = core.GetAllPaginated<Species>("/species/", pageNumber);

            return result;
        }

        [HttpGet()]
        [Route("GetAllStarships/")]
        public EntityResults<Starship> GetAllStarships(string pageNumber = "1")
        {
            EntityResults<Starship> result = core.GetAllPaginated<Starship>("/starships/", pageNumber);

            return result;
        }

        [HttpGet()]
        [Route("GetAllVehicles/")]
        public EntityResults<Vehicle> GetAllVehicles(string pageNumber = "1")
        {
            EntityResults<Vehicle> result = core.GetAllPaginated<Vehicle>("/vehicles/", pageNumber);

            return result;
        }
    }
}
