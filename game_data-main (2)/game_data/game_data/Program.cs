using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace game_data
{
    internal class Program
    {
        // 게임 데이터 클래스

        // 게임 데이터 변수 초기화: 소지 돈, 강화 수치, 파괴 방지권 보유 여부
        static int money = 3000000; // 개인 소지 돈
        static int status = 20; // 강화 수치
        static bool safe = false; // 파괴 방지권 유무

        static void Main(string[] args)
        {
            LoadGameData(); // 저장된 게임 데이터 불러오기
            Console.WriteLine($"현재 가지고 있는 돈: {money}"); // 불러온 후 소지 돈 출력
            Console.WriteLine($"현재 강화 상태: {status}"); // 불러온 후 강화 상태 출력
            Console.WriteLine(safe ? "현재 파괴 방지권 상태: 보유 중" : "현재 파괴 방지권 상태: 보유하고 있지 않음");

            int cost = 1000, sell = 0; // 강화 비용과 판매 금액 초기화

            Random rnd = new Random(); // 랜덤 값 생성을 위한 Random 객체 생성

            while (status != 25) // 강화 수치가 25가 되면 반복문 종료
            {
                int successRate = Math.Max(30, 95 - (5 * status)); // 강화 성공 확률 계산

                int destroyRate = 0; // 파괴 확률 초기화
                if (status >= 13)
                {
                    destroyRate = Math.Min(20, (status - 12) * 3); // 13강 이상에서 파괴 확률이 3%씩 오름
                }
                else
                {
                    destroyRate = 0; // 12강 이하는 파괴 확률을 0%로 고정
                }

                Console.WriteLine($"\n성공확률: {successRate}%, 실패확률: {100 - successRate - destroyRate}%, 파괴확률: {destroyRate}%");

                Console.WriteLine($"소지 금액: {money}, 강화비용: {cost}, 판매 금액: {sell}\n");

                string input = GameChoice(); // 사용자의 선택 받기

                Console.Clear(); // 콘솔 화면 초기화

                if (input == "Y" || input == "y") // 강화 선택
                {
                    status = AttemptEnhancement(money, status, rnd); // 강화 시도
                    money -= cost; // 강화 비용 차감

                    // 다음 강화 비용과 판매 금액 설정
                    if (status <= 10)
                    {
                        cost = (status + 1) * 1000;
                        sell = (status + 1) * 5000;
                    }
                    else if (status <= 17)
                    {
                        cost = (status + 1) * 3000;
                        sell = (status + 1) * 10000;
                    }
                    else if (status <= 22)
                    {
                        cost = (status + 1) * 5000;
                        sell = (status + 1) * 20000;
                    }
                }
                else if (input == "N" || input == "n") // 판매 선택
                {
                    if (status >= 1)
                    {
                        Sell(status, sell); // 아이템 판매
                        status = 0; // 강화 수치 초기화
                        money += sell; // 소지 금액에 판매 금액 추가
                    }
                    else
                    {
                        Console.WriteLine("1강 이하까지는 판매할 수 없습니다.");
                    }
                }
                else if (input == "S" || input == "s") // 파괴 방지권 구매 선택
                {
                    BuySafeGuard(); // 파괴 방지권 구매
                    safe = true; // 파괴 방지권 보유 상태 업데이트
                }
                else if (input == "L" || input == "l") // 게임 데이터 저장 선택
                {
                    SaveGameData(); // 게임 데이터 저장
                }
                else // 잘못된 입력 처리
                {
                    Console.WriteLine("잘못 입력하였습니다. 다시 입력해주세요.");
                }
            }
        }

        // 아이템 판매 함수
        static void Sell(int statusN, int sellY)
        {
            Console.WriteLine($"{statusN}강 무기를 팔아 {sellY}원을 얻었습니다.");
        }

        // 파괴 방지권 구매 함수
        static void BuySafeGuard()
        {
            Console.WriteLine("파괴 방지 보호권을 샀습니다.");
        }

        // 사용자 선택 입력 함수
        static string GameChoice()
        {
            Console.WriteLine("행동을 선택 해 주세요\n");
            Console.WriteLine("강화: Y, 판매: N, 파괴방지권 구매: S,  저장: L");
            return Console.ReadLine();
        }

        // 강화 시도 함수
        static int AttemptEnhancement(int moneyY, int statusY, Random rnd)
        {
            int destroy = 0; // 파괴 확률 초기값
            if (statusY >= 13)
            {
                destroy = Math.Min(20, (statusY - 12) * 3); // 13강 이상에서 파괴 확률이 3%씩 오름
            }

            int randomNum = rnd.Next(1, 101); // 랜덤값(1~100)

            if (randomNum <= Math.Max(30, 95 - (5 * statusY))) // 성공 확률 계산
            {
                statusY++; // 강화 성공
                Console.WriteLine($"성공하였습니다. {statusY}강\n");
            }
            else if (randomNum <= 100 - destroy) // 실패할 경우
            {
                Console.WriteLine($"실패하였습니다. {statusY}강\n");
            }
            else // 파괴할 경우
            {
                if (safe) // 파괴 방지권이 있으면
                {
                    safe = false; // 방지권 사용
                    Console.WriteLine($"파괴되었지만 보호권으로 보호하였습니다. {statusY}강\n");
                }
                else
                {
                    statusY = 0; // 파괴됨
                    Console.WriteLine($"파괴되었습니다. {statusY}강\n");
                }
            }

            return statusY; // 강화 후 강화 수치 반환
        }

        // 게임 데이터 저장 함수
        static void SaveGameData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("game_data.txt"))
                {
                    writer.WriteLine(money);
                    writer.WriteLine(status);
                    writer.WriteLine(safe);
                }
                Console.WriteLine("-----게임 데이터가 저장되었습니다.-----");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
            }
        }

        // 게임 데이터 불러오기 함수
        static void LoadGameData()
        {
            try
            {
                using (StreamReader reader = new StreamReader("game_data.txt"))
                {
                    money = int.Parse(reader.ReadLine());
                    status = int.Parse(reader.ReadLine());
                    safe = bool.Parse(reader.ReadLine());
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

        // 게임 종료 여부 확인 함수
        static bool Game_over(int moneyO, int statusO)
        {
            if (moneyO <= 0)
            {
                Console.WriteLine("소지 금액을 전부 소모하였습니다,");
                return true;
            }
            else if (statusO == 25)
            {
                Console.WriteLine("축하합니다. 최대 강화횟수에 도달하였습니다");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
