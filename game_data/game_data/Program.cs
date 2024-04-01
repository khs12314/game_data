using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool result = false, drt = false, safe = false;
            int money = 300000, status = 0,destroy =0, rn,en=0, cost = 1000, plus = 5, sell = 0;//money: 소지 금액, status: 현재 강화 수치,rn: 확률 계산을 위한 랜덤 값 변수, yy: 출력용 강화 확률(실제로도 yy와 비슷한 방식으로 확률 설정), cost: 강화 비용 변수, plus: 확률 조정을 위한 변수, sell: 판매 금액 변수
            string input; //input: 플레이어의 행동을 해당 변수로 받음 
            Random rnd = new Random();
            while (true)//특정 조건을 만족해 break 문법으로 빠져나가기 위해 무한 루프를 설정
            {
                en = 95 - (plus * status);//기본(0강) 강화 확률인 95%에서 강회 수치에 따라 plus(5)와 status(강화수치)를 곱한 값을 빼 확률 조정
                if (en <30)//최소 강화 성공확률을 30으로 잡았기 때문에 en이 30이하가 된다면 30으로 고정
                {
                    en = 30;
                }
                
                Console.WriteLine($"성공확률 {en}, {status}강");
                Console.WriteLine("강화를 하려면 Y, 판매를 하려면 N을 입력해주세요. 파괴방지권을 사려면 S를 누르세요. Y/N/S");//ReadLine을 통해 입력받을 값들을 지정해줌 추후 예외 처리를 통해 이 외에 다른 값들을 넣으면 안되게 할 예정
                Console.WriteLine($"소지 금액: {money} 강화비용: {cost} 판매 금액: {sell}\n");
                
                input = Console.ReadLine();


                if (input == "Y")
                {
                    money -= cost;//소지 금액(money)에서 강화 비용(cost)을 뺌
                    if (money < 0)//소지 금액이 고갈 시 루프 탈출
                    {
                        break;
                    }

                    cost = (status + 1) * 1000;//강회 수치(status)에 따라 강화 비용 조정
                    rn = rnd.Next(1, 100);//rn에 랜덤값(1~100)을 넣음

                    if (status == 0)//강화 수치가 0일 때 조건
                    {
                        if (rn <= 95)//0강일 때 강화 확률은 95%이기 때문에 95이하의 숫자가 나올 시 성공
                        {
                            sell += 5000;//판매 금액을 5000원 증가
                            result = true;//강화 결과 성공

                        }
                        else
                        {
                            result = false;//강화 결과 실패
                        }
                    }
                    //강화 수치 0일때 조건 끝
                    else if (status <= 10)//10강 이하일 때 조건
                    {
                        
                        if (rn <= 95 - (plus * status))//여기서부터 강화 수치 하락
                        {
                            sell += 10000;//판매 금액을 10000원 증가
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
                        destroy = 1;//파괴확률 1로 조정
                        if (status <= 13)//강회 수치가 13이하일 때의 조건 13강으로 잡은 이유는 강화 성공 확률 최소값을 30으로 잡았기 때문에 plus * status값이 65가 나오는 13으로 조건을 잡음
                        {
                            if (rn <= 95 - (plus * status))//기존의 확률 조정 방식과 동일
                            {
                                result = true;//강화 성공
                                sell += 20000;//판매 금액 20000원 증가
                                destroy += 1;//파괴확률 또한 +1씩 증가
                                

                            }

                            else
                            {
                                 if (rn >= 101 - destroy)//파괴됐는지 확인을 위한 if문 랜덤의 숫자를 1~100으로 잡았고 확률에 따라 파괴 확률이 증가하기 때문에 조정을 위해 101에서 파괴 확률 값(destroy)를 뺌
                                  {
                                      drt = true;//만약 rn의 값이 101-destroy의 값 이상일 때 파괴
                                  }
                                   else drt = false;//아닐 시 파괴 x
                                result = false;//위 조건을 지나 파괴 조건에 성립이 안될 시 단순 실패로 강화 유지
                            }
                        }
                        else//14강 이상부터 해당 조건문에 들어감
                        {
                            if (rn <= 95 - 65)//강회 최소값을 30으로 잡았기 때문에 -65를 해줌
                            {
                                result = true;// 강화 성공
                                sell += 20000;//판매금액 20000원 증가
                                destroy += 1;//파괴 확률 +1씩 증가

                            }

                            else
                            {
                                if (rn >= 101 - destroy)//파괴됐는지 확인을 위한 if문 랜덤의 숫자를 1~100으로 잡았고 확률에 따라 파괴 확률이 증가하기 때문에 조정을 위해 101에서 파괴 확률 값(destroy)를 뺌
                                {
                                    drt = true;//만약 rn의 값이 101-destroy의 값 이상일 때 파괴
                                }
                                else drt = false;//아닐 시 파괴 x
                                result = false;
                            }
                        }

                    }


                    if (result == true)//위 과정에서 받은 result값을 여기서 사용 성공하면 status값을 +1증가
                    {
                        status += 1;
                        Console.WriteLine($"성공하였습니다. {status}강");


                    }
                    else if (drt == true)//파괴가 활성화 되었을 때 status값을 0으로 조정
                    {
                        status = 0;
                        Console.WriteLine($"파괴되었습니다.{status}강");


                    }
                    else
                    {
                        Console.WriteLine($"실패하였습니다.{status}강");

                    }
                }else if(input == "N")//판매 커맨드
                {
                    
                    money += sell;//소지 금액에 위 조건문의 판매금액 조정값을 더함
                    sell = 0;//판매 후 판매금액 초기화
                    status = 0;//무기를 팔았기 때문에 강화 수치 초기화
                }
                else if(input == "S")//해당 부분은 파괴방지권을 구현하기 위해 만든곳 주석이 우선이라 현재 상태에선 구현을 안함
                {
                    safe = true;
                }
            }

            
            
        }


    }
}
