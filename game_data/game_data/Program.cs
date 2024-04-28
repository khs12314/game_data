using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace game_data
{
    class PenaltyKickGame
    {

        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Gane_Log";
        static readonly string SpreadsheetId = "1asHqhQdgcmzZG7k1hWGGoNEs4p668cqiBIwK6qF_W3Y";
        static readonly string SheetName = "Sheet1";
        private static ConsoleColor textColor;
        static int money = 0;
        static int doping = 0;

        static void Main(string[] args)
        {

            textColor = Console.ForegroundColor;
            Random random = new Random();
            int totalSuccess = 0;

            int count = 0;
            string[] sucessCount = new string[5];
            string input;
            string result;
            int goalkeeperDirection;//키퍼의 랜덤값
                                    // 강화 시도 횟수와 성공 횟수를 저장할 변수 추가

            while (totalSuccess < 4)
            {

                input = Player();
                if (input == "1")
                {

                    result = kick();
                    SaveLog(input, result);
                    ggst(input, result);
                    goalkeeperDirection = random.Next(0, 100);
                    keeper(goalkeeperDirection, result);
                    Console.WriteLine(goalkeeperDirection);
                    if (kickSucces(result, goalkeeperDirection))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("성공!\n");
                        totalSuccess++;
                        sucessCount[count] = "o";
                        money += 100;

                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("실패!\n");
                        sucessCount[count] = "x";
                    }
                    Console.ForegroundColor = textColor;
                    count++;
                    Console.WriteLine($"성공횟수 ({sucessCount[0]})({sucessCount[1]})({sucessCount[2]})({sucessCount[3]})({sucessCount[4]})");
                    if ((count == 5) && (totalSuccess != 4))
                    {
                        Console.WriteLine("5번의 기회를 실패해 처음부터 골 카운트가 초기화 되었습니다.");
                        count = 0;
                    }

                }

            }
            // 최종 결과 출력
            Console.WriteLine($"게임 종료! 총 5번 중 {totalSuccess}번 성공하였습니다.");
        }
        //여기부터 함수
        static void ggst(string input, string result)
        {
            GoogleCredential credential;
            // 사용자 인증 정보 로드
            using (var stream = new FileStream("gamelog-420512-010a0df65b4a.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // 데이터 작성
            ValueRange valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> {
            new List<object> { "Time",DateTime.Now.ToString("G"), "","킥 or 도핑", input,"방향",result }
        };

            SpreadsheetsResource.ValuesResource.AppendRequest request =
                service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, "Sheet1!A1:G1");
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var response = request.Execute();

        }

        static string Player()
        {
            Console.WriteLine("                             +---------------------------+    ");
            Console.WriteLine("                             |\\                          |\\   ");
            Console.WriteLine("                             | \\         0               | \\  ");
            Console.WriteLine("                             |  \\       (X)              |  \\ ");
            Console.WriteLine("_____________________________|___|_______LL______________|-|\\|__________________");
            Console.WriteLine("         /                   /                             /                  / ");
            Console.WriteLine("        /                   /                             /                  /  ");
            Console.WriteLine("       /                   /                             /                  /  ");
            Console.WriteLine("      /                   /_____________________________/                  /    ");
            Console.WriteLine("     /                                                                    /     ");
            Console.WriteLine("    /                            O/                                      /      ");
            Console.WriteLine("   /                           _/x                                      /       ");
            Console.WriteLine("  /                            _/ \\                                   /        ");
            Console.WriteLine(" /________________________________/___________________________________/         ");
            string Choice;
            Console.WriteLine("페널티킥을 하려면 1번, 도핑을 하시려면 2번을 누르세요,");
            Choice = Console.ReadLine();
            return Choice;
        }



        static string kick()
        {
            string kickerDirection;
            while (true)
            {
                Console.WriteLine("킥 방향을 선택하세요 (왼쪽: A, 중앙: S, 오른쪽: D):");
                kickerDirection = Console.ReadLine();
                Console.WriteLine();
                if (kickerDirection != "A" && kickerDirection != "S" && kickerDirection != "D")
                {
                    Console.WriteLine("잘못된 입력입니다. A, S, D 중 하나를 입력하세요.");

                }
                else
                {
                    break;
                }
            }
            return kickerDirection;
        }



        static void keeper(int goalkeeperDirection, string reslut)
        {
            if (goalkeeperDirection < 33)
            {
                if (reslut == "A")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\ ____   /                | \\  ");
                    Console.WriteLine("                             |  \\ ___0-<_/               |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /    >@<                      /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 왼쪽으로 뛰었습니다.");
                    Console.ReadKey();
                    Console.WriteLine();
                }
                if (reslut == "S")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\ ____   /                | \\  ");
                    Console.WriteLine("                             |  \\ ___0-<_/               |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /           >@<                /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 왼쪽으로 뛰었습니다.");
                    Console.ReadKey();
                }
                if (reslut == "D")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\ ____   /                | \\  ");
                    Console.WriteLine("                             |  \\ ___0-<_/               |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /                     >@<     /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 왼쪽으로 뛰었습니다.");
                    Console.ReadKey();
                }
            }
            //===================================================================
            else if (goalkeeperDirection < 66)
            {
                if (reslut == "A")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\           0             | \\  ");
                    Console.WriteLine("                             |  \\         (X)            |  \\ ");
                    Console.WriteLine("_____________________________|___|_________LL____________|-|\\|__________________");
                    Console.WriteLine("         /                   /    >@<                      /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 중간으로 뛰었습니다.");
                    Console.ReadKey();
                }
                if (reslut == "S")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\         0               | \\  ");
                    Console.WriteLine("                             |  \\       (X)              |  \\ ");
                    Console.WriteLine("_____________________________|___|_______LL______________|-|\\|__________________");
                    Console.WriteLine("         /                   /           >@<               /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                            O/                                      /      ");
                    Console.WriteLine("   /                           _/x                                      /       ");
                    Console.WriteLine("  /                            _/ \\                                    /        ");
                    Console.WriteLine(" /________________________________/___________________________________/          ");
                    Console.WriteLine($"골키퍼가 중간으로 뛰었습니다.");
                    Console.ReadKey();
                }
                if (reslut == "D")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\         0               | \\  ");
                    Console.WriteLine("                             |  \\       (X)              |  \\ ");
                    Console.WriteLine("_____________________________|___|_______LL______________|-|\\|__________________");
                    Console.WriteLine("         /                   /                     >@<     /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                            O/                                      /      ");
                    Console.WriteLine("   /                           _/x                                      /       ");
                    Console.WriteLine("  /                            _/ \\                                    /        ");
                    Console.WriteLine(" /________________________________/___________________________________/         ");
                    Console.WriteLine($"골키퍼가 중간으로 뛰었습니다.");
                    Console.ReadKey();
                }
            }
            else
            {
                if (reslut == "A")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\                \\   ____ | \\  ");
                    Console.WriteLine("                             |  \\              \\_>-0____ |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /    >@<                      /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 오른쪽으로 뛰었습니다.");
                    Console.ReadKey();
                }
                if (reslut == "S")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\                \\   ____ | \\  ");
                    Console.WriteLine("                             |  \\              \\_>-0____ |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /           >@<                /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 오른쪽으로 뛰었습니다.");
                    Console.ReadKey();
                }
                if (reslut == "D")
                {
                    Console.WriteLine("                             +---------------------------+    ");
                    Console.WriteLine("                             |\\                          |\\   ");
                    Console.WriteLine("                             | \\                \\   ____ | \\  ");
                    Console.WriteLine("                             |  \\              \\_>-0____ |  \\ ");
                    Console.WriteLine("_____________________________|___|_______________________|-|\\|__________________");
                    Console.WriteLine("         /                   /                     >@<     /                  / ");
                    Console.WriteLine("        /                   /                             /                  /  ");
                    Console.WriteLine("       /                   /                             /                  /  ");
                    Console.WriteLine("      /                   /_____________________________/                  /    ");
                    Console.WriteLine("     /                                                                    /     ");
                    Console.WriteLine("    /                                                                    /      ");
                    Console.WriteLine("   /                                                                    /       ");
                    Console.WriteLine("  /                                                                    /        ");
                    Console.WriteLine(" /____________________________________________________________________/         ");
                    Console.WriteLine($"골키퍼가 오른쪽으로 뛰었습니다.");
                    Console.ReadKey();
                }
            }
        }



        static void enhance()
        {

        }
        static bool kickSucces(string result, int goalkeeperDirection)
        {
            bool isSuccess = (result != "A" && goalkeeperDirection < 33) ||
                                  (result != "S" && goalkeeperDirection >= 33 && goalkeeperDirection < 66) ||
                                  (result != "D" && goalkeeperDirection >= 66);
            return isSuccess;
        }
        static void SaveGameData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("game_data.txt"))
                {
                    writer.WriteLine(money);
                    writer.WriteLine(dobing);
                }
                Console.WriteLine("-----게임 데이터가 저장되었습니다.-----");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
            }

        }
        static void SaveLog(string PlayerChoice, string PlayerKick)
        {
            string Logfile = "game_Log.txt";
            try
            {
                using (StreamWriter LogWriter = new StreamWriter(Logfile, true))
                {
                    LogWriter.WriteLine($"{DateTime.Now}|{PlayerChoice}|{PlayerKick}");

                }
                Console.WriteLine($"{DateTime.Now}-----게임 로그가 저장되었습니다.-----");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"로그를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
            }
        }
        static void LoadGameData()
        {
            try
            {
                using (StreamReader reader = new StreamReader("game_data.txt"))
                {
                    money = int.Parse(reader.ReadLine());
                    dobing = int.Parse(reader.ReadLine());

                }
                Console.WriteLine("-----게임 데이터를 불러왔습니다.-----");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("-------저장된 데이터가 없습니다. 새로운 게임을 시작합니다.--------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터를 불러오는 동안 오류가 발생했습니다.{ex.Message}");
            }

        }


    }

}
