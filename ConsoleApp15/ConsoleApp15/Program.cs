using System;

using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace game_data
{
    class PenaltyKickGame
    {

        //static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Game_Log";
        static readonly string SpreadsheetId = "1asHqhQdgcmzZG7k1hWGGoNEs4p668cqiBIwK6qF_W3Y";
        static readonly string SheetName = "Sheet1";
       
        static double enhancedDopingSuccessRate = 0.6; // Enhanced doping success rate
        static bool enhanced = false; // Flag to check if enhancement is done

        static int dopingCost = 100;
        static double dopingSuccessRate = 0.5;
        static string IDinput()
        {
            int? UVcount = 0;
            int? dayCount = 0;
            string IDNum;
            Console.WriteLine("학번을 입력해주세요.");
            IDNum = Console.ReadLine();

            //visit(IDNum, UVcount, dayCount);

            return IDNum;

        }

        static void Main(string[] args)
        {

            ConsoleColor textColor;

            string idn;
            idn = IDinput();
            string dataid;
            DateTime intime = DateTime.Now;
            DateTime tokentime;
            textColor = Console.ForegroundColor;
            Random random = new Random();
            byte totalSuccess = 0;
            int money = 0;
            int doping = 0;
            byte count = 0;
            string final;
            string[] successCount = new string[5];
            string input;
            string result;
            int goalkeeperDirection;//키퍼의 랜덤값
            Guid? guid = null;
            bool dopingSuccess = false;
            //나중에 함수로 수정할 예정
            try
            {

                using (StreamReader reader = new StreamReader("game_data.txt"))
                {
                    dataid = reader.ReadLine();
                    guid = Guid.Parse(reader.ReadLine());
                    totalSuccess = byte.Parse(reader.ReadLine());
                    count = byte.Parse(reader.ReadLine());
                    money = int.Parse(reader.ReadLine());
                    doping = int.Parse(reader.ReadLine());
                    for (int i = 0; i < 5; i++)
                    {
                        successCount[i] = reader.ReadLine();
                        Console.WriteLine(successCount[i]);
                    }

                }
                while (dataid != idn)
                {

                    Console.WriteLine("입력하신 학번과 저장된 학번이 다릅니다. 아이디를 재입력해주세요.");
                    idn = Console.ReadLine();

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

            if (guid == null)
            {
                guid = Guid.NewGuid();
            }
            while (totalSuccess < 4)
            {

                input = Player();
                tokentime = DateTime.Now;
                if (input == "1")
                {

                    result = kick();


                    goalkeeperDirection = random.Next(0, 100) + 50;

                    keeper(goalkeeperDirection, result);
                    Console.WriteLine(goalkeeperDirection);
                    if (kickSucces(result, goalkeeperDirection, dopingSuccess))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("성공!\n");
                        totalSuccess++;
                        successCount[count] = "o";
                        money += 100;

                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("실패!\n");
                        successCount[count] = "x";
                    }
                    Console.ForegroundColor = textColor;
                    count++;
                    Console.WriteLine($"성공횟수 ({successCount[0]})({successCount[1]})({successCount[2]})({successCount[3]})({successCount[4]})");
                    //ggst(idn, input, result, goalkeeperDirection, totalSuccess, count, guid);
                    if ((count == 5) && (totalSuccess != 4))
                    {
                        Console.WriteLine("5번의 기회를 실패해 처음부터 골 카운트가 초기화 되었습니다.");
                        count = 0;
                        totalSuccess = 0;
                        guid = Guid.NewGuid();
                        for (int i = 0; i < successCount.Length; i++)
                        {
                            successCount[i] = null;
                        }
                    }


                }
                else if (input == "2")
                {
                    Console.WriteLine($"도핑을 시도합니다. 도핑 비용은 {dopingCost}원입니다.");

                    if (money < dopingCost)
                    {
                        Console.WriteLine("돈이 부족하여 도핑을 시도할 수 없습니다.");
                        continue;
                    }
                    money -= dopingCost; // 도핑 비용차감
                    dopingSuccess = random.NextDouble() <= dopingSuccessRate;
                    if (dopingSuccess)
                    {
                        Console.WriteLine("도핑 성공! 킥 성공률이 50% 상승합니다.");
                        
                    }
                    else
                    {
                        Console.WriteLine("도핑 적발로 퇴출되었습니다. 게임 오버!");

                        // 도핑 적발시 돈 초기화
                        money = 0;
                        count = 0; // Reset count to 0
                        totalSuccess = 0;
                        for (int i = 0; i < successCount.Length; i++)
                        {
                            successCount[i] = null;
                        }
                    }


                }
                else if (input == "3")
                {
                    enhance();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 1 또는 2를 입력하세요.");
                    continue;
                }
                SaveGameData(idn, guid, totalSuccess, count, money, doping, successCount);




            }
            Console.WriteLine($"게임 종료! 총 5번 중 {totalSuccess}번 성공하였습니다.");
            Console.WriteLine($"게임을 초기화하려면 1번을 입력해주세요. 다른 키를 입력 시 프로그램이 종료됩니다.(초기화시 세이브데이터는 삭제됩니다.");
            final = Console.ReadLine();
            if (final == "1")
            {
                try
                {
                    // Existing reset code...
                    enhanced = false; // Reset enhancement status
                    File.Delete("game_data.txt");
                    Console.WriteLine("게임이 초기화되었습니다.");
                    string appName = AppDomain.CurrentDomain.FriendlyName;
                    Process.Start(appName);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"파일을 삭제하는 중 오류가 발생했습니다: {ex.Message}");
                }
            }


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
            Console.WriteLine("페널티킥을 하려면 1번, 도핑을 하려면 2번 ,강화를 하려면 3번");
            Choice = Console.ReadLine();
            

            return Choice;
        }



        static string kick()
        {
            string kickerDirection;
            while (true)
            {
                Console.WriteLine("킥 방향을 선택하세요 (왼쪽: A, 중앙: S, 오른쪽: D):");
                kickerDirection = Console.ReadLine().ToUpper();
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



        static void keeper(int goalkeeperDirection, string result)
        {
            if (goalkeeperDirection < 33)
            {
                if (result == "A")
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
                if (result == "S")
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
                if (result == "D")
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
                if (result == "A")
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
                if (result == "S")
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
                if (result == "D")
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
                if (result == "A")
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
                if (result == "S")
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
                if (result == "D")
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
            if (!enhanced)
            {
                // 강화 성공 시 도핑 확률을 50% 올림
                dopingSuccessRate += 0.5;
                Console.WriteLine("강화 에 성공 하여  도핑 확률이 50% 상승하였습니다.");
                enhanced = true;
                //강화 실패
            }
            else
            {
                Console.WriteLine("이미 강화를 하여 더 이상 강화 를 할 수 없습니다.");
            }
        }
        static bool kickSucces(string result, int goalkeeperDirection, bool dopingSuccess)
        {
            double baseSuccessRate = 0.5;
            double enhancedSuccessRate = baseSuccessRate;

            if (dopingSuccess && goalkeeperDirection >= 100)
            {
                Random random = new Random();

                while (true)
                {
                    int newDirection = random.Next(0, 100);
                    if ((result != "A" && newDirection < 33) ||
                    (result != "S" && newDirection >= 33 && newDirection < 66) ||
                    (result != "D" && newDirection >= 66) &&
                       newDirection != goalkeeperDirection) // 겹치지 않는 경우
                    {
                        goalkeeperDirection = newDirection;
                        break;
                    }
                }
            }
            bool isSuccess = (result != "A" && goalkeeperDirection < 33) ||
                                  (result != "S" && goalkeeperDirection >= 33 && goalkeeperDirection < 66) ||
                                  (result != "D" && goalkeeperDirection >= 66);
            return isSuccess;
        }
        static void SaveGameData(string idn, Guid? guid, byte total, byte count, int money, int doping, string[] totalc)
        {
            totalc = new string[5];
            try
            {
                using (StreamWriter writer = new StreamWriter("game_data.txt"))
                {
                    writer.WriteLine(idn);
                    writer.WriteLine(guid);
                    writer.WriteLine(total);
                    writer.WriteLine(count);
                    writer.WriteLine(money);
                    writer.WriteLine(doping);
                    foreach (string line in totalc)
                    {
                        writer.WriteLine(line);
                        Console.WriteLine(line);
                    }


                }

                Console.WriteLine("-----게임 데이터가 저장되었습니다.-----");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
            }

        }
    }
}