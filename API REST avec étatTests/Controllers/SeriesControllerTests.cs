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
        [TestMethod()]
        public void GetSeriesTest()
        {
            // Arrange
            List<Serie> expected = new List<Serie>();
            expected.Add(new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                9, 184, 2001, "ABC (US)"));
            expected.Add(new Serie(2, "James May's 20th Century", "The world in 1999 would have been unrecognisable to anyone from 1900. James May takes a look at some of the greatest developments of the 20th century, and reveals how they shaped the times we live in now.",
                1, 6, 2007, "BBC Two"));
            expected.Add(new Serie(3, "True Blood", "Ayant trouvé un substitut pour se nourrir sans tuer (du sang synthétique), les vampires vivent désormais parmi les humains. Sookie, une serveuse capable de lire dans les esprits, tombe sous le charme de Bill, un mystérieux vampire. Une rencontre qui bouleverse la vie de la jeune femme...",
                7, 81, 2008, "HBO"));

            // Act
            List<Serie> result = controller.GetSeries().Result.Value.Where(s => s.Serieid <= 3).OrderBy(s => s.Serieid).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, result, "Pas la même liste");

        }

        [TestMethod()]
        public void GetSerieTest_IdCorrect_Reussi()
        {
            // Arrange
            Serie expected = new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                9, 184, 2001, "ABC (US)");

            // Act
            var result = controller.GetSerie(1).Result.Value;

            // Assert
            Assert.AreEqual(expected, result, "Les résultats sont différents");
        }

        [TestMethod()]
        public void GetSerieTest_IdIncorrectFail()
        {
            // Act
            var result = controller.GetSerie(9999999).Result.Value;

            // Assert
            Assert.IsNull(result, "Un resultat à été trouvé");
        }

        [TestMethod()]
        public void DeleteSerieTest_Ok()
        {
            // Act 
            var result = controller.DeleteSerie(1).Result;

            // Arrane
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "L'élément n'a pas été supprimé");
        }

        [TestMethod()]
        public void DeleteSerieTest_NotOk()
        {
            // Act 
            var result = controller.DeleteSerie(9999999).Result;

            // Arrane
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Un élément à été supprimé");
        }

        [TestMethod()]
        public void PostSerieTest_Ok()
        {
            // Arrange
            Serie ajout = new Serie(999, "Test", "Cette série n'existe pas", 1, 1, 200, "TF1");

            // Act 
            var result = controller.PostSerie(ajout).Result;
            var recherche = controller.GetSerie(999).Result;

            //Arrange
            Assert.IsInstanceOfType(result, result.GetType(), "Le type n'est pas bon");
            Assert.IsNotNull(recherche.Value, "La série n'a pas été ajoutée");
        }

        [TestMethod()]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostSerieTest_NotOk()
        {
            // Arrange
            Serie ajout = new Serie(1, "Test", "Cette série n'existe pas", 1, 1, 200, "TF1");

            // Act 
            var result = controller.PostSerie(ajout).Result;
        }

        [TestMethod()]
        public void PutSerieTest_Ok()
        {
            // Arrange
            string nouveau_nom = "Nouvelle Série";
            Serie ajout = new Serie(1, nouveau_nom, "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                 9, 184, 2001, "ABC (US)");
            
            // Act
            var resultat = controller.PutSerie(1, ajout).Result;
            var recherche = controller.GetSerie(1).Result.Value.Titre;

            //Assert
            Assert.IsInstanceOfType(resultat, typeof(IActionResult), "La série n'a pas changé");
            Assert.AreEqual(nouveau_nom, recherche, "Le nom n'a pas été modifié");

        }

        [TestMethod()]
        public void PutSerieTest_NonOk()
        {
            // Arrange
            Serie ajout = new Serie(2, null, "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                 9, 184, 2001, "ABC (US)");

            // Act
            var resultat = controller.PutSerie(1, ajout).Result;

            // Arrang
            Assert.IsInstanceOfType(resultat, typeof(BadRequestResult),"Le résultat n'est pas une BadRequest") ;

        }

        [TestCleanup]
        public void CleanUp()
        {
            var result = controller.GetSerie(1).Result;
            if (result != null && result.Value == null)
            {
                Serie ajout = new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                9, 184, 2001, "ABC (US)");
                controller.PostSerie(ajout);
            }
            controller.DeleteSerie(999);
        }
    }
}