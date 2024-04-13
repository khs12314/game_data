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
        static int money = 300000;//개인 소지 돈
        static int status = 20;//강화 수치
        static bool safe = false;//파괴 방지권 유무
        static int[] destroy = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 30, 30, 30, 30, 30, 30, 30 };
        static void Main(string[] args)
        {
            LoadGameData();//데이터 불러오기
            Console.WriteLine ($"현재 가지고 있는 돈:{money}");//불러온 후 재화 량
            Console.WriteLine($"현재 강화 상태: {status}");//불러온 후 강화 상태
            if (safe)
            {
                Console.WriteLine("현재 파괴 방지권 상태:보유 중");//방지권 보유 시 출력
            }
            else
            {
                Console.WriteLine("현재 파괴 방지권 상태: 보유하고 있지 않음");//방지권 보유하고 있지 않을 시 출력
            }
            int  cost = 1000, sell = 0, EnforceNum = 0;//cost: 강화 시 드는 재화, sell
            
            string input;
            

            Random rndN = new Random();
            while (status != 25)
            {
                EnforceNum = 95 - (5 * status);
                if (EnforceNum < 30)//최소 강화 성공확률을 30으로 잡았기 때문에 en이 30이하가 된다면 30으로 고정
                {
                    EnforceNum = 30;

                }
                Console.WriteLine($"\n성공확률: {EnforceNum}, {status}강\n");
                Console.WriteLine($"소지 금액: {money} 강화비용: {cost} 판매 금액: {sell}\n");
                input = Game_Choice();//input변수를 행동함수로 초기화해 반환값을 넣음
                Console.Clear();
                
                
                
                if ((input == "Y")||(input == "y"))//일반적으로 소문자를쓰기때문에 소문자 또한 넣음
                {
                    
                    status = InputY(money, status, rndN, destroy[status]);
                    money -= cost;
                    
                    if (status <= 10)
                    {
                        cost = (status + 1) * 1000;
                        sell = (status + 1) * 5000;
                    }
                    else if(status <= 17) 
                    {
                        cost = (status + 1) * 3000;
                        sell = (status + 1) * 10000;
                    }
                    else if(status <= 22)
                    {
                        cost = (status + 1) * 5000;
                        sell = (status + 1) * 20000;
                    }
                }
                else if((input == "N")||(input == "n")) 
                {
                    if (status >= 1)
                    {
                        InputN(status, sell);
                        status = 0;
                        money += sell;
                    }
                    else
                    {
                        Console.WriteLine("1강이하까지는 판매할 수 없습니다.");//회의를 통해 1강 이하까지는 판매하지 못하게 설정
                    }
                }
                else if((input == "S")||(input == "s"))
                {
                    money -= 10000;
                    InputS();
                    safe = true;
                }else if((input == "L")||(input == "l"))
                {
                    SaveGameData();
                }else
                {
                    Console.WriteLine("잘못 입력하였습니다. 다시 입력해주세요.");
                }
            }
                

           }
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
            catch(FileNotFoundException)
            {
                Console.WriteLine("-------저장된 데이터가 없습니다. 새로운 게임을 시작합니다.--------");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"게임 데이터를 불러오는 동안 오류가 발생했습니다.{ex.Message}");
            }

        }
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
        static void InputS()
        {
            Console.WriteLine("파괴 방지 보호권을 샀습니다.");
        }
        static void InputN(int statusN, int sellY)
        {
            Console.WriteLine($"{statusN}강 무기를 팔아{sellY}원을 얻었습니다.");
            
        }
            
        static string Game_Choice()
        {
            
            string inputC;
            Console.WriteLine("행동을 선택 해 주세요\n");
            Console.WriteLine("강화: Y, 판매: N, 파괴방지권 구매: S,  저장: L");//ReadLine을 통해 입력받을 값들을 지정해줌 추후 예외 처리를 통해 이 외에 다른 값들을 넣으면 안되게 할 예정


            return inputC = Console.ReadLine();

        }
        static bool Game_over(int moneyO, int statusO)
        {
            bool game_over = false;
            if(moneyO <=0) {
                Console.WriteLine("소지 금액을 전부 소모하였습니다,");
                game_over = true;
                return game_over;

            }
            else if(statusO == 25)
            {
                Console.WriteLine("축하합니다. 최대 강화횟수에 도달하였습니다");
                game_over = true;
                return game_over;
            }
            else
            {
                return game_over;
            }
        }

        static int InputY(int moneyY,int statusY, Random rnd, int destroy)
        {
            int  plus = 5;// destroy: 파괴 확률, plus: 강화 수치 조정 변수
            bool result = false, drt = false;//result: 강화 결과, drt: 파괴 결과
            int RandomNum;//랜덤
            
            while (!Game_over(moneyY, statusY))
            {


                
                RandomNum = rnd.Next(1, 100);//rn에 랜덤값(1~100)을 넣음

                Console.WriteLine($"{RandomNum}");
                




                if (statusY == 0)//강화 수치가 0일 때 조건
                {
                    if (RandomNum <= 95)//0강일 때 강화 확률은 95%이기 때문에 95이하의 숫자가 나올 시 성공
                    {
                       
                        result = true;//강화 결과 성공

                    }
                    else
                    {
                        result = false;//강화 결과 실패
                    }
                }
                //강화 수치 0일때 조건 끝

                else if (statusY <= 10)//10강 이하일 때 조건
                {

                    if (RandomNum <= 95 - (plus * statusY))//여기서부터 강화 수치 하락
                    {
                       
                        result = true;//강화 결과 성공

                    }
                    else
                    {
                        result = false;//강화 결과 실패
                    }
                }
                //강화 수치 10이하일 때 조건 끝 이후 조건문은 11강부터 시작
                else
                {


                    if (statusY <= 13)//강회 수치가 13이하일 때의 조건 13강으로 잡은 이유는 강화 성공 확률 최소값을 30으로 잡았기 때문에 plus * status값이 65가 나오는 13으로 조건을 잡음
                    {
                        if (RandomNum <= 95 - (plus * statusY))//기존의 확률 조정 방식과 동일
                        {
                            result = true;//강화 성공
                            
                            destroy += 1;//파괴확률 또한 +1씩 증가


                        }

                        else
                        {
                            if (RandomNum >= 101 - destroy)//파괴됐는지 확인을 위한 if문 랜덤의 숫자를 1~100으로 잡았고 확률에 따라 파괴 확률이 증가하기 때문에 조정을 위해 101에서 파괴 확률 값(destroy)를 뺌
                            {
                                drt = true;//만약 rn의 값이 101-destroy의 값 이상일 때 파괴
                            }
                            else
                            {
                                drt = false;//아닐 시 파괴 x
                                result = false;//위 조건을 지나 파괴 조건에 성립이 안될 시 단순 실패로 강화 유지
                            }
                        }
                    }
                    else//14강 이상부터 해당 조건문에 들어감
                    {
                        if (RandomNum <= 95 - 65)//강회 최소값을 30으로 잡았기 때문에 -65를 해줌
                        {
                            result = true;// 강화 성공
                            
                            destroy += 1;//파괴 확률 +1씩 증가


                        }

                        else
                        {
                            if (RandomNum >= 101 - destroy)//파괴됐는지 확인을 위한 if문 랜덤의 숫자를 1~100으로 잡았고 확률에 따라 파괴 확률이 증가하기 때문에 조정을 위해 101에서 파괴 확률 값(destroy)를 뺌
                            {
                                drt = true;//만약 rn의 값이 101-destroy의 값 이상일 때 파괴
                            }
                            else
                            {
                                drt = false;//아닐 시 파괴 x
                                result = false;
                            }
                        }
                    }

                }
                if (result == true)//위 과정에서 받은 result값을 여기서 사용 성공하면 status값을 +1증가
                {
                    Console.WriteLine($"{destroy}");
                    statusY += 1;
                    Console.WriteLine($"성공하였습니다. {statusY}강\n");
                    return statusY;

                }
                else if (drt == true)//파괴가 활성화 되었을 때 status값을 0으로 조정
                {
                    
                    if (safe == true)
                    {
                        Console.WriteLine($"{destroy}");
                        safe = false;
                        Console.WriteLine($"파괴되었지만 보호권으로 보호하였습니다.{statusY}강\n");
                        return statusY;
                    }
                    else
                    {
                        Console.WriteLine($"{destroy}");
                        statusY = 0;
                        Console.WriteLine($"파괴되었습니다.{statusY}강\n"); 
                        return statusY;
                    }
                }
                else
                {
                    Console.WriteLine($"{destroy}");
                    Console.WriteLine($"실패하였습니다.{statusY}강\n");
                    return statusY;

                }

            }
            return 25;
        }

 


    }
}
