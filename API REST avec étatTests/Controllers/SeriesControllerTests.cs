using Microsoft.VisualStudio.TestTools.UnitTesting;
using API_REST_avec_état.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_REST_avec_état.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace API_REST_avec_état.Controllers.Tests
{

    [TestClass()]
    public class SeriesControllerTests
    {
        public SeriesController controller;
        [TestInitialize]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<SeriesDBContext>().UseNpgsql("Server=51.83.36.122;port=5432;Database=goysim; SearchPath=schema_api; uid=goysim; password=PjQgs7"); // Chaine de connexion à mettre dans les ( )
            SeriesDBContext context = new SeriesDBContext(builder.Options);
            controller = new SeriesController(context);
        }
        
    }
}