using System;
using System.IO;

namespace ConsoleApp15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 초기 변수 설정
            int gold = 150000; // 초기 골드
            int bookLevel = 0; // 초기 책 레벨
            double successRate = 0.9; // 초기 강화 성공 확률

            // 파일에서 데이터 불러오기
            LoadGameData(ref gold, ref bookLevel, ref successRate);

            // 시작 메시지 출력
            Console.WriteLine("===== 책 구매 게임에 오신 것을 환영합니다! =====");
            Console.WriteLine($"현재 골드: {gold}, 현재 책 레벨: {bookLevel}");

            // 메뉴 반복해서 실행
            while (true)
            {
                Console.WriteLine("\n어떤 작업을 하시겠습니까?");
                Console.WriteLine("1. 책 구매하기");
                Console.WriteLine("2. 책 판매하기");
                Console.WriteLine("3. 강화파괴 방지권 구매하기");
                Console.WriteLine("4. 게임 데이터 저장하기");
                Console.WriteLine("5. 종료");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Enhancebook(ref gold, ref bookLevel, ref successRate);
                        break;
                    case "2":
                        Sellbook(ref gold, ref bookLevel);
                        break;
                    case "3":
                        BuyProtection(ref gold, ref successRate);
                        break;
                    case "4":
                        SaveGameData(gold, bookLevel, successRate); // 게임 데이터 파일에 저장
                        Console.WriteLine("게임 데이터를 저장했습니다.");
                        break;
                    case "5":
                        Console.WriteLine("게임을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        break;
                }
            }
        }

        // 책 강화 메서드
        static void Enhancebook(ref int gold, ref int bookLevel, ref double successRate)
        {
            Console.WriteLine("\n===== 책 강화 시작 =====");

            // 강화 비용 계산
            int enhancementCost = (bookLevel + 1) * 1000;

            // 골드 부족 시 처리
            if (gold < enhancementCost)
            {
                Console.WriteLine("골드가 부족합니다!");
                return;
            }

            // 강화파괴 방지권 사용 여부 확인
            bool useProtection = AskForProtection();

            if (useProtection)
            {
                Console.WriteLine("강화파괴 방지권을 사용합니다.");
                // 강화파괴 방지권 사용 시 강화 성공 확률 증가
                successRate += 0.1;
            }

            // 강화 성공 여부 랜덤으로 결정
            Random rand = new Random();
            double randomValue = rand.NextDouble();

            if (randomValue < successRate)
            {
                Console.WriteLine("책 강화에 성공하였습니다!");

                // 책 레벨 증가 및 골드 차감
                bookLevel++;
                gold -= enhancementCost;

                // 강화 성공 시 확률 감소
                successRate -= 0.05;
                if (successRate < 0.1) // 최소 강화 성공 확률 설정 (예: 10%)
                {
                    successRate = 0.1;
                }

                // 최대 레벨에 도달 시 처리
                if (bookLevel == 15)
                {
                    Console.WriteLine("최대 강화 레벨에 도달하였습니다!");
                    successRate = 0.1; // 최대 레벨 달성 시 강화 성공 확률 초기화
                }
            }
            else
            {
                Console.WriteLine("책 강화에 실패하였습니다...");

                // 강화파괴 여부 확인 및 처리
                bool destroyed = CheckForDestruction(ref gold, ref bookLevel);
                if (destroyed)
                {
                    Console.WriteLine("책이 손상되어 버렸습니다... 새 책을 구매하세요");
                    successRate = 0.9; // 실패 시 강화 성공 확률 초기화
                }
                else
                {
                    // 실패 시 골드 차감 및 확률 증가
                    gold -= enhancementCost / 2;
                    successRate += 0.1;
                }
            }

            // 현재 상태 출력
            Console.WriteLine($"현재 골드: {gold}, 현재 책 레벨: {bookLevel}, 강화 성공 확률: {successRate:P}");
        }

        // 강화파괴 방지권 구매 메서드
        static void BuyProtection(ref int gold, ref double successRate)
        {
            Console.WriteLine("\n===== 강화파괴 방지권 구매 =====");

            int protectionCost = 10000; // 강화파괴 방지권 가격

            // 골드 부족 시 처리
            if (gold < protectionCost)
            {
                Console.WriteLine("골드가 부족하여 강화파괴 방지권을 구매할 수 없습니다.");
                return;
            }

            Console.WriteLine("강화파괴 방지권을 구매하시겠습니까? (Y/N)");
            string input = Console.ReadLine().ToUpper();

            if (input == "Y")
            {
                gold -= protectionCost;
                Console.WriteLine("강화파괴 방지권을 구매했습니다!");
                successRate += 0.1; // 강화파괴 방지권 구매 시 강화 성공 확률 증가
            }
            else
            {
                Console.WriteLine("강화파괴 방지권 구매를 취소하였습니다.");
            }

            Console.WriteLine($"현재 골드: {gold}");
        }

        // 강화파괴 방지권 사용 여부 묻는 메서드
        static bool AskForProtection()
        {
            Console.WriteLine("강화파괴 방지권을 사용하시겠습니까? (Y/N)");
            string input = Console.ReadLine().ToUpper();

            return (input == "Y");
        }

        // 강화 실패 시 강화파괴 여부 확인 메서드
        static bool CheckForDestruction(ref int gold, ref int bookLevel)
        {
            Random rand = new Random();
            double destroyChance = 0.4;

            if (rand.NextDouble() < destroyChance)
            {
                bookLevel = 0;
                return true;
            }

            return false;
        }

        // 책 판매 메서드
        static void Sellbook(ref int gold, ref int bookLevel)
        {
            if (bookLevel == 0)
            {
                Console.WriteLine("판매할 책이 없습니다!");
                return;
            }

            int sellPrice = bookLevel * 1000;

            Console.WriteLine($"\n===== 책 판매 =====");
            Console.WriteLine($"책 레벨 {bookLevel}을(를) 판매합니다. 판매 가격: {sellPrice} 골드");

            gold += sellPrice;
            bookLevel = 0;

            Console.WriteLine($"현재 골드: {gold}, 현재 책 레벨: {bookLevel}");
        }

        // 게임 데이터 저장 메서드
        static void SaveGameData(int gold, int bookLevel, double successRate)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("GameData.txt"))
                {
                    writer.WriteLine(gold);
                    writer.WriteLine(bookLevel);
                    writer.WriteLine(successRate);
                }
                Console.WriteLine("게임 데이터를 저장했습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터 저장 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        // 게임 데이터 불러오기 메서드
        static void LoadGameData(ref int gold, ref int bookLevel, ref double successRate)
        {
            try
            {
                if (File.Exists("GameData.txt"))
                {
                    using (StreamReader reader = new StreamReader("GameData.txt"))
                    {
                        gold = int.Parse(reader.ReadLine());
                        bookLevel = int.Parse(reader.ReadLine());
                        successRate = double.Parse(reader.ReadLine());
                    }
                    Console.WriteLine("게임 데이터를 불러왔습니다.");
                }
                else
                {
                    Console.WriteLine("저장된 게임 데이터가 없습니다. 새로운 게임을 시작합니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터 불러오기 중 오류가 발생했습니다: {ex.Message}");
            }
        }
    }
}