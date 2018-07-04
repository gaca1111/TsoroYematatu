using System.Web.Mvc;
using TsoroYematatu.BLL;
using TsoroYematatu.Models;

namespace TsoroYematatu.Controllers
{
    public class HomeController : Controller
    {
        private readonly GameBoardLogic gameBoardLogic = new GameBoardLogic();
        private GameBoard gameBoard = new GameBoard();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult FieldClick(int field0 = 0, int field1 = 0, int field2 = 0, int field3 = 0, int field4 = 0, int field5 = 0, int field6 = 0,
            int stone0 = 0, int stone1 = 0, int turn = 1, int fieldNumber = 10, int gameType = 4)
        {
            gameBoard = new GameBoard
            {
                turn = turn,
                stones =
                {
                    [0] = stone0,
                    [1] = stone1
                },
                fields =
                {
                    [0] = field0,
                    [1] = field1,
                    [2] = field2,
                    [3] = field3,
                    [4] = field4,
                    [5] = field5,
                    [6] = field6
                }
            };
            gameBoard = gameBoardLogic.FieldClick(gameType, fieldNumber, gameBoard);
            if(gameBoard.winner != 0)
                    if(gameBoard.winner == 1) Response.Write("<script>alert('White wins')</script>");
                    else Response.Write("<script>alert('Black wins')</script>");
            if (gameBoard.wrongPawnClicked == 1)
            {
                Response.Write("<script>alert('Wrong pawn clicked!')</script>");
                gameBoard.wrongPawnClicked = 0;
            }
            switch (gameType)
            {
                case 1:
                    return View("~/Views/Home/PvP.cshtml", gameBoard);
                case 2:
                    return View("~/Views/Home/PvC.cshtml", gameBoard);
                case 3:
                    return View("~/Views/Home/CvC.cshtml", gameBoard);
                default:
                    return View();
            }
        }

        public ActionResult PvP()
        {
            ViewBag.Message = "Trb gry gracz kontra gracz.";

            return View(gameBoard);
        }

        public ActionResult PvC()
        {
            ViewBag.Message = "Tryb gry Gracz kontra komputer.";

            return View(gameBoard);
        }

        public ActionResult CvC()
        {
            ViewBag.Message = "Tryb gry komputer komntra komputer.";

            return View(gameBoard);
        }
    }
}